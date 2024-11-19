using Persistence.Repositories;

namespace DegirmenciGida.Order.Domain
{
    public class OrderDetail:BaseEntity<Guid>
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } // API'den gelen ürün adı
        public decimal ProductPrice { get; set; } // API'den gelen ürün fiyatı
        public int Quantity { get; set; }
        public Guid OrdersId { get; set; }

        public OrderDetail()
        {
            
        }
        public OrderDetail(Guid productId, string productName, decimal productPrice, int quantity, Guid ordersId)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            Quantity = quantity;
            OrdersId = ordersId;

        }

        public OrderDetail(Guid productId, string productName, decimal productPrice, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            Quantity = quantity;
        }
    }
}
