using DegirmenciGida.Order.Domain;
using Persistence.Repositories;

namespace DegirmenciGida.Order.Application
{
    public interface IOrderDetailService:IAsyncRepository<OrderDetail,Guid>
    {
    }
}
