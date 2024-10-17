using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.Shared.SharedConstants;

namespace CustomCADs.API.Endpoints.Categories.PutCategory;

public class PutCategoryRequestValidator : Validator<PutCategoryRequest>
{
    public PutCategoryRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(RequiredErrorMessage);
    }
}
