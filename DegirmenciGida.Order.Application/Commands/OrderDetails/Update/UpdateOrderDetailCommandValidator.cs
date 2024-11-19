using FluentValidation;

namespace DegirmenciGida.Order.Application.Commands.OrderDetails.Update
{
    public class UpdateOrderDetailCommandValidator:AbstractValidator<UpdatedOrderDetailCommand>
    {
        public UpdateOrderDetailCommandValidator()
        {
            RuleFor(o=>o.OrderId).NotEmpty();
            RuleFor(o=>o.ProductId).NotEmpty();
            RuleFor(o=>o.ProductPrice).NotEmpty().GreaterThan(0);
            RuleFor(o=>o.ProductName).NotEmpty();
            RuleFor(o=>o.Quantity).NotEmpty().GreaterThan(0);
            RuleFor(o => o.Id).NotEmpty();
        }
    }
}
