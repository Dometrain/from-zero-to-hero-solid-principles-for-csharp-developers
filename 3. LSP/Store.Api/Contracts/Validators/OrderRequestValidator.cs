using FluentValidation;
using Store.Api.Contracts.Requests;

namespace Store.Api.Contracts.Validators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.CartId).NotEmpty().GreaterThanOrEqualTo(1);
    }
}