using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Domain;
using Infrastructure.LoggingServices;
using Persistence.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Infrastructure.TransactionSaga
{
    public class OrderCompletedStep : ISagaStep<CreateOrderCommand, bool>
    {
        private readonly IOrderService _orderService;
        private readonly CreateOrderSagaStep _createOrderSagaStep;

        public OrderCompletedStep(IOrderService orderService, CreateOrderSagaStep createOrderSagaStep)
        {
            _orderService = orderService;
            _createOrderSagaStep = createOrderSagaStep;
        }

        public async Task CompensateAsync(CreateOrderCommand request)
        {
            var order = _createOrderSagaStep.Orders;
            order.MarkAsCancelled();
            await _orderService.UpdateAsync(order);
        }

        public async Task<bool> ExecuteAsync(CreateOrderCommand request)
        {
            try
            {
                var order = _createOrderSagaStep.Orders;
                order.MarkAsCompleted();
                await _orderService.UpdateAsync(order);
                LoggingService.LogInformation($"Create Order process ending! Order:{order}");
                return true;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex,$"Create Order process ending! Order complate error!");
                return false;               
            }
        }
    }
}
