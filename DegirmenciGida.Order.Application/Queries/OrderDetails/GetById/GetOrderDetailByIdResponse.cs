using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Queries.OrderDetails.GetById
{
    public class GetOrderDetailByIdResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } // API'den gelen ürün adı
        public decimal ProductPrice { get; set; } // API'den gelen ürün fiyatı
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
    }
}
