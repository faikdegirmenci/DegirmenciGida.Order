using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Interfaces;
using DegirmenciGida.Order.Domain;
using Infrastructure.LoggingServices;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Infrastructure.TransactionSaga
{
    public class CreateOrderDetailSagaStep : ISagaStep<CreateOrderCommand, bool>
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly CreateOrderSagaStep _createOrderSagaStep;
        private readonly IProductServiceAccessService _productServiceAccessService;
        private readonly IServiceProvider _serviceProvider;
        private readonly OrderDbContext _orderDbContext;
        public OrderDetail DetailOrder { get; set; }
        public Orders Order { get; set; }

        public CreateOrderDetailSagaStep(IOrderDetailService orderDetailService, CreateOrderSagaStep createOrderSagaStep, IProductServiceAccessService productServiceAccessService, IServiceProvider serviceProvider, OrderDbContext orderDbContext)
        {
            _orderDetailService = orderDetailService;
            _createOrderSagaStep = createOrderSagaStep;
            _productServiceAccessService = productServiceAccessService;
            _serviceProvider = serviceProvider;
            _orderDbContext = orderDbContext;
        }

        public async Task CompensateAsync(CreateOrderCommand request)
        {
            var order = _createOrderSagaStep.Orders;
            using (var scope = _serviceProvider.CreateScope())
            {
                var newOrderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                await newOrderService.CancelOrderAsync(order.Id);
            }
        }

        public async Task<bool> ExecuteAsync(CreateOrderCommand request)
        {
            using (var transaction = await _orderDbContext.Database.BeginTransactionAsync())
            {
                Order = _createOrderSagaStep.Orders;
                LoggingService.LogInformation($"Create Order Detail process Started! Step 3: OrderId: {Order.Id} ");

                try
                {

                    foreach (var orderRequest in request.OrderRequest)
                    {
                        var product = await _productServiceAccessService.GetProductByIdAsync(orderRequest.ProductId);
                        var orderDetail = new OrderDetail();
                        orderDetail.ProductId = product.Id;
                        orderDetail.ProductPrice = product.Price;
                        orderDetail.ProductName = product.Name;
                        orderDetail.OrdersId = Order.Id;
                        orderDetail.IsDeleted = false;
                        orderDetail.CreatedDate = DateTime.Now;
                        orderDetail.Quantity = orderRequest.Quantity;

                        orderDetail = await _orderDetailService.AddAsync(orderDetail);
                        DetailOrder = orderDetail;

                        await transaction.CommitAsync();

                        LoggingService.LogInformation($"Create Order process Started! Step 3 : Create order detail. || CustomerId:{request.CustomerId} - OrderId:{Order.Id} - Product: {product} - OrderDetailId: {orderDetail.Id}");

                    }
                    return true;
                }
                catch (Exception ex)
                {
                    LoggingService.LogError(ex, $"Create Order process error! Step 3 : Create detail order error. OrderId:{Order.Id}");
                    await transaction.RollbackAsync();
                    return false;
                }
            }
                
        }
    }
}
