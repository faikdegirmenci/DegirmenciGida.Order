using DegirmenciGida.Order.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Interfaces
{
    public interface IProductServiceAccessService
    {
        Task<ProductResponse> GetProductByIdAsync(Guid productId);
        Task<bool> CheckProductStockAsync(Guid productId, int quantity);
        Task<bool> DecreaseStockAsync(Guid productId, int quantity);
        Task<bool> RestoreStockAsync(Guid productId, int quantity);
    }
}
