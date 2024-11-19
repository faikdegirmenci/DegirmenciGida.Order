using DegirmenciGida.Order.Domain;

namespace DegirmenciGida.Order.Application.Commands.Order.Create
{
    public class CreateOrderCommandResponse
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid CustomerId { get; set; }
    }
}