using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Application.Commands.Order.Create;
using DegirmenciGida.Order.Application.Interfaces;
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
    public class DecreaseProductStockStep : ISagaStep<CreateOrderCommand, bool>
    {
        private readonly IProductServiceAccessService _productServiceAccessService;
        public DecreaseProductStockStep(IProductServiceAccessService productServiceAccessService)
        {
            _productServiceAccessService = productServiceAccessService;
        }
        public async Task CompensateAsync(CreateOrderCommand request)
        {
            foreach (var orderRequest in request.OrderRequest)
            {
                await _productServiceAccessService.RestoreStockAsync(orderRequest.ProductId, orderRequest.Quantity);
            }
        }

        public async Task<bool> ExecuteAsync(CreateOrderCommand request)
        {
            try
            {
                foreach (var orderRequest in request.OrderRequest)
                {
                    LoggingService.LogInformation($"Create Order process Started! Step 4 : decrease product stock || ProductId:{orderRequest.ProductId} - Quantity: {orderRequest.Quantity}");
                    
                    var isDecreaseProduct = await _productServiceAccessService.DecreaseStockAsync(orderRequest.ProductId,orderRequest.Quantity);
                    if (!isDecreaseProduct)
                    {
                        LoggingService.LogInformation($"Create Order process Started! Step 4 : decrease product stock error! (product service returned false) || ProductId:{orderRequest.ProductId} - Quantity: {orderRequest.Quantity}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex,$"Create Order process Started! Step 4 : decrease product stock error!");
                return false;                
            }
        }
    }
}
