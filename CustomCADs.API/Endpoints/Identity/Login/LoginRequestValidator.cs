using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.Shared.SharedConstants;
using static CustomCADs.Domain.Users.UserConstants;

namespace CustomCADs.API.Endpoints.Identity.Login;

public class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(PasswordMinLength, PasswordMaxLength).WithMessage(LengthErrorMessage);
    }
}
