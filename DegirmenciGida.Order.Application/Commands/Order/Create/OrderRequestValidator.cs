using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Commands.Order.Create
{
    public class OrderRequestValidator:AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(order => order.ProductId).NotEmpty().WithMessage("ProductId alanı boş olamaz.");

            RuleFor(order => order.Quantity).GreaterThan(0).WithMessage("Quantity en az 1 olmalıdır.");
        }
    }
}
