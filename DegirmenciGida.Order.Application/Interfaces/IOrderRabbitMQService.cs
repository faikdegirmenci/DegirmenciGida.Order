using DegirmenciGida.Order.Domain.Response;

namespace DegirmenciGida.Order.Application
{
    public interface IOrderRabbitMQService
    {
        Task<ProductResponse> CreateOrderAsync(Guid productId);
    }
}
