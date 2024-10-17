using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.Cads.CadConstants;
using static CustomCADs.Domain.Shared.SharedConstants;

namespace CustomCADs.API.Endpoints.Cads.PutCad;

public class PostCadRequestValidator : Validator<PutCadRequest>
{
    public PostCadRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);
        
        RuleFor(r => r.Description)
            .NotEmpty().WithMessage(RequiredErrorMessage)
            .Length(DescriptionMinLength, DescriptionMaxLength).WithMessage(LengthErrorMessage);

        RuleFor(r => r.CategoryId)
            .NotEmpty().WithMessage(RequiredErrorMessage);

        RuleFor(r => r.Price)
            .ExclusiveBetween(PriceMin, PriceMax).WithMessage(RangeErrorMessage);
    }
}
