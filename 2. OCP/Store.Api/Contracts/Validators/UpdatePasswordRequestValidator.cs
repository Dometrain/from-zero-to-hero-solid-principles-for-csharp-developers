using FluentValidation;
using Store.Api.Contracts.Requests;

namespace Store.Api.Contracts.Validators;

public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
{
    public UpdatePasswordRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
    }
}