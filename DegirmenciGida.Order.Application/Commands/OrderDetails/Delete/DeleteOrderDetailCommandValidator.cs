using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Commands.OrderDetails.Delete
{
    public class DeleteOrderDetailCommandValidator:AbstractValidator<DeleteOrderDetailCommand>
    {
        public DeleteOrderDetailCommandValidator()
        {
            RuleFor(o=>o.Id).NotEmpty();
        }
    }
}
