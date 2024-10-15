using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.UserConstants;

namespace CustomCADs.API.Endpoints.Identity.Register;

public class RegisterRequestValidator : Validator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Username)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .EmailAddress();

        RuleFor(r => r.FirstName)
            .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);
        
        RuleFor(r => r.LastName)
            .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(PasswordMinLength, PasswordMaxLength).WithMessage(LengthErrorMessage);
        
        RuleFor(r => r.ConfirmPassword)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Equal(r => r.Password);
    }
}
