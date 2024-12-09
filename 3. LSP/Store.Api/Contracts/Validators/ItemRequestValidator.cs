using FluentValidation;
using Store.Api.Contracts.Requests;

namespace Store.Api.Contracts.Validators;

public class ItemRequestValidator : AbstractValidator<ItemRequest>
{
    public ItemRequestValidator()
    {
        RuleFor(x => x.Quantity).NotEmpty().GreaterThanOrEqualTo(1);
        RuleFor(x => x.ProductId).NotEmpty().GreaterThanOrEqualTo(1);
    }
}