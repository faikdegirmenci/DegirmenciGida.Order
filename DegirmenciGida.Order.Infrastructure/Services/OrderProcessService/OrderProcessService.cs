using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Interfaces;
using DegirmenciGida.Order.Infrastructure.TransactionSaga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Infrastructure.Services.OrderProcessService
{
    public class OrderProcessService: IOrderProcessService
    {
        private readonly OrderOrchestrator _orderOrchestrator;

        public OrderProcessService(OrderOrchestrator orderOrchestrator)
        {
            _orderOrchestrator = orderOrchestrator;
        }

        public async Task<bool> ProcessOrder(CreateOrderCommand command)
        {
            // OrderOrchestrator'u kullanarak işlemi başlat
            return await _orderOrchestrator.ProcessOrderAsync(command);
        }
    }
}
