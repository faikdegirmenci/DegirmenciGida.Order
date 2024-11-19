using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Domain;
using Infrastructure.LoggingServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Infrastructure.TransactionSaga
{
    public class CreateOrderSagaStep : ISagaStep<CreateOrderCommand, bool>
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly OrderDbContext _orderDbContext;
        private readonly IServiceProvider _serviceProvider;
        public Orders Orders { get; private set; }

        public CreateOrderSagaStep(IOrderService orderService, IOrderDetailService orderDetailService, OrderDbContext orderDbContext, IServiceProvider serviceProvider)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _orderDbContext = orderDbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task CompensateAsync(CreateOrderCommand request)
        {
            if (Orders != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var newOrderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    await newOrderService.CancelOrderAsync(Orders.Id);
                }
            }
        }

        public async Task<bool> ExecuteAsync(CreateOrderCommand request)
        {
            using (var transaction = await _orderDbContext.Database.BeginTransactionAsync())
            {
                LoggingService.LogInformation($"Create Order process Started! Step 2 : Create order. CustomerId:{request.CustomerId} - OrderRequest: {request.OrderRequest.ToArray().ToString()}");

                var order = new Orders(request.CustomerId);

                order.CreatedDate = DateTime.Now;
                order.IsDeleted = false;
                order.TotalAmount = request.OrderRequest.Sum(x => x.Quantity); // Basit hesaplama

                try
                {
                    // Siparişi kaydet
                    order = await _orderService.AddAsync(order);
                    await transaction.CommitAsync();
                    Orders = order;
                    LoggingService.LogInformation($"Order {order.Id} process successful!");
                }
                catch (Exception ex)
                {
                    LoggingService.LogError(ex, "Create Order process Started! Step 2 : Create order error!");
                    await transaction.RollbackAsync();
                    return false;
                }
                return true;
            }

        }
    }
}
