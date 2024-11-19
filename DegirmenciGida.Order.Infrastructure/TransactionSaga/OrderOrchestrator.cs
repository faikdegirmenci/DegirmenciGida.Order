using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application;
using Persistence.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegirmenciGida.Order.Application.Interfaces;

namespace DegirmenciGida.Order.Infrastructure.TransactionSaga
{
    public class OrderOrchestrator
    {
        private readonly SagaOrchestrator<CreateOrderCommand> _sagaOrchestrator;

        public OrderOrchestrator(SagaOrchestrator<CreateOrderCommand> sagaOrchestrator,
                                 IOrderService orderService,
                                 IProductServiceAccessService productService,
                                 IOrderDetailService orderDetailService,
                                 OrderDbContext orderDbContext,
                                 IServiceProvider serviceProvider)
        {
            _sagaOrchestrator = sagaOrchestrator;
            CreateOrderSagaStep createOrderSagaStep = new CreateOrderSagaStep(orderService, orderDetailService, orderDbContext, serviceProvider);

            // Adımları orchestrator'a ekle
            _sagaOrchestrator.AddStep(new ValidateProductStockStep(productService));
            _sagaOrchestrator.AddStep(createOrderSagaStep);
            _sagaOrchestrator.AddStep(new CreateOrderDetailSagaStep(orderDetailService, createOrderSagaStep, productService, serviceProvider, orderDbContext));
            _sagaOrchestrator.AddStep(new DecreaseProductStockStep(productService));
            _sagaOrchestrator.AddStep(new OrderCompletedStep(orderService, createOrderSagaStep));
        }

        public async Task<bool> ProcessOrderAsync(CreateOrderCommand orderRequest)
        {
            return await _sagaOrchestrator.ExecuteAsync(orderRequest);
        }
    }
}
