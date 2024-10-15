using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Categories.PutCategory;

public class PutCategoryRequestValidator : Validator<PutCategoryRequest>
{
    public PutCategoryRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(RequiredErrorMessage);
    }
}
