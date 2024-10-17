﻿using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.Orders.OrderConstants;
using static CustomCADs.Domain.Shared.SharedConstants;

namespace CustomCADs.API.Endpoints.Orders.PutOrder;

public class PutOrderRequestValidator : Validator<PutOrderRequest>
{
    public PutOrderRequestValidator()
    {
        RuleFor(r => r.Name)
          .NotEmpty().WithMessage(RequiredErrorMessage)
          .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);

        RuleFor(r => r.Description)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(DescriptionMinLength, DescriptionMaxLength).WithMessage(LengthErrorMessage);

        RuleFor(r => r.CategoryId)
            .NotEmpty().WithMessage(RequiredErrorMessage);
    }
}
