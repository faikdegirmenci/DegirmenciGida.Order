using DegirmenciGida.Order.Domain;
using Persistence.Repositories;

namespace DegirmenciGida.Order.Application
{
    public interface IOrderService:IAsyncRepository<Orders,Guid>
    {
        Task CancelOrderAsync(Guid orderId);
    }
}
