
using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace DegirmenciGida.Order.Infrastructure
{
    public class OrderDetailService :EfRepositoryBase<OrderDetail,Guid,OrderDbContext>, IOrderDetailService
    {

        public OrderDetailService(OrderDbContext dbContext):base(dbContext)
        {
        }
    }
}
