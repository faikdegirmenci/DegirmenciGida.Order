using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegirmenciGida.Order.Application.Commands.Order.Create
{
    public class CreateOrderCommandValidator:AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.OrderRequest).NotEmpty().WithMessage("OrderRequest listesi boş olamaz.");

            RuleForEach(command => command.OrderRequest).SetValidator(new OrderRequestValidator());

        }
    }
}
