using FluentValidation;
using Store.Api.Contracts.Requests;

namespace Store.Api.Contracts.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.DeliveryAddress).NotEmpty();
        RuleFor(x => x.CountryCode).NotEmpty().Length(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}