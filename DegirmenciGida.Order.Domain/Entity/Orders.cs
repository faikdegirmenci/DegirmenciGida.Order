using Persistence.Repositories;

namespace DegirmenciGida.Order.Domain
{
    public class Orders:BaseEntity<Guid>
    {
        public List<OrderDetail> OrderDetails { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; } =0;
        public Guid CustomerId { get; set; }


        public Orders(Guid customerId)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
        }

        public void AddOrderDetail(Guid productId, string productName, decimal productPrice, int quantity)
        {
            OrderDetails.Add(new OrderDetail(productId, productName,productPrice,quantity));
        }

        public void MarkAsCompleted()
        {
            Status = OrderStatus.Completed;
        }

        public void MarkAsCancelled()
        {
            Status = OrderStatus.Cancelled;
        }

        public enum OrderStatus
        {
            Pending,
            Completed,
            Cancelled
        }
    }
}
