﻿using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.UserConstants;

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
