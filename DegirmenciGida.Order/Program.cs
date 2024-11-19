using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Interfaces;
using DegirmenciGida.Order.Infrastructure;
using DegirmenciGida.Order.Infrastructure.Services.OrderProcessService;
using DegirmenciGida.Order.Infrastructure.Services.Product;
using DegirmenciGida.Order.Infrastructure.TransactionSaga;
using Infrastructure.LoggingServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Saga;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Microsoft.Extensions.Http;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMediatR(configuration =>
//{
//    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
//});
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Saga orchestrator dependency injection
builder.Services.AddScoped<SagaOrchestrator<CreateOrderCommand>>();
builder.Services.AddScoped<OrderOrchestrator>();

// Application Layer'daki servisler
builder.Services.AddScoped<IOrderProcessService, OrderProcessService>();

builder.Services.AddApplicationServices();

builder.Services.AddDbContext<OrderDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDB")), ServiceLifetime.Scoped);

LoggingService.ConfigureLogging(builder.Configuration.GetConnectionString("OrderDB"));

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderRabbitMQService, OrderRabbitMQService>();


//var retryPolicy = Policy<HttpResponseMessage>
//    .Handle<HttpRequestException>() // Sadece HttpRequestException hatalarýný ele alýr
//    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

//var circuitBreakerPolicy = Policy<HttpResponseMessage>
//    .Handle<HttpRequestException>() // Ayný þekilde HttpRequestException ele alýnýr
//    .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));

//// Politikalarý birleþtirerek AsyncPolicyWrap oluþturun
//var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

//builder.Services.AddHttpClient<IProductServiceAccessService, ProductServiceAccessService>(client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7106/");
//})
//.AddPolicyHandler(policyWrap); // Doðrudan policyWrap eklenir


builder.Services.AddHttpClient<IProductServiceAccessService, ProductServiceAccessService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7106/");
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError() // Daha kapsamlý bir hata yönetimi saðlar
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));


//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    LoggingService.LogInformation("Starting the web application");
    app.Run();
}
catch (Exception ex)
{
    LoggingService.LogError(ex, "Application start-up failed");
    throw;
}
finally
{
    LoggingService.CloseAndFlush();
}