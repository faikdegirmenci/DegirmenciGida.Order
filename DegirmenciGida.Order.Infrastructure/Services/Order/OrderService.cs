
using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Domain;
using Infrastructure.EventBus.RabbitMQEventBus;
using Infrastructure.EventBus.Request;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;

namespace DegirmenciGida.Order.Infrastructure
{
    public class OrderService :EfRepositoryBase<Orders,Guid,OrderDbContext>, IOrderService
    {
        public OrderService(OrderDbContext orderDbContext) :base(orderDbContext) 
        {
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = Orders.OrderStatus.Cancelled;
                try
                {
                    await UpdateAsync(order);
                }
                catch (Exception ex)
                {
                    var message = ex.ToString();    
                }
               
            }
        }
    }
}
