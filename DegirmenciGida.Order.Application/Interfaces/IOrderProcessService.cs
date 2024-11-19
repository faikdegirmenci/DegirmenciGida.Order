using DegirmenciGida.Order.Application.Commands.Order.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Interfaces
{
    public interface IOrderProcessService
    {
        Task<bool> ProcessOrder(CreateOrderCommand command);
    }
}
