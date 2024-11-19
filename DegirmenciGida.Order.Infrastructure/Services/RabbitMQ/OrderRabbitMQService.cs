using System.Text;
using System.Text.Json;
using DegirmenciGida.Order.Application;
using DegirmenciGida.Order.Domain.Response;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DegirmenciGida.Order.Infrastructure
{
    public class OrderRabbitMQService: IOrderRabbitMQService
    {
        private readonly OrderDbContext _context;

        public OrderRabbitMQService(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ProductResponse> CreateOrderAsync(Guid productId)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var replyQueue = channel.QueueDeclare().QueueName;
            var correlationId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueue;
            props.CorrelationId = correlationId;

            var messageBytes = Encoding.UTF8.GetBytes(productId.ToString());
            channel.BasicPublish(exchange: "", routingKey: "product_request_queue", basicProperties: props, body: messageBytes);

            var tcs = new TaskCompletionSource<string>();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                    tcs.SetResult(response);
                }
            };

            channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: consumer);

            var productResponse = await tcs.Task;
            var product = JsonSerializer.Deserialize<ProductResponse>(productResponse);

            return product;
        }
    }
}
