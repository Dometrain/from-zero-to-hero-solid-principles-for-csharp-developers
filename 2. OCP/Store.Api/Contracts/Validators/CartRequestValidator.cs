using FluentValidation;
using Store.Api.Contracts.Requests;

namespace Store.Api.Contracts.Validators;

public class CartRequestValidator : AbstractValidator<CartRequest>
{
    public CartRequestValidator()
    {
        RuleFor(x => x.Items).NotEmpty();
        RuleFor(x => x.Items).ForEach(x => x.SetValidator(new ItemRequestValidator()));
    }
}