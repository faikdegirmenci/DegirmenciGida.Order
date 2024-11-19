using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Interfaces;
using Infrastructure.LoggingServices;
using Persistence.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Infrastructure.TransactionSaga
{
    public class ValidateProductStockStep : ISagaStep<CreateOrderCommand, bool>
    {
        private readonly IProductServiceAccessService _productService;

        public ValidateProductStockStep(IProductServiceAccessService productService)
        {
            _productService = productService;
        }
        public async Task CompensateAsync(CreateOrderCommand request)
        {
            var message = "buraya geldi";
        }

        public async Task<bool> ExecuteAsync(CreateOrderCommand request)
        {
            if (request.OrderRequest.Count > 0)
            {
                foreach (var orderRequest in request.OrderRequest)
                {
                    LoggingService.LogInformation($"Create Order process Started! Step 1 : Product stock controller. Product Id: {orderRequest.ProductId} - Quantity: {orderRequest.Quantity}");
                    var isResult = await _productService.CheckProductStockAsync(orderRequest.ProductId, orderRequest.Quantity);
                    return isResult;
                }
            }

            return false;
        }
    }
}
