using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Commands.Order.Create
{
    public class OrderRequest
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
