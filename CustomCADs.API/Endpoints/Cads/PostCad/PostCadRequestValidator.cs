using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.Cads.CadConstants;
using static CustomCADs.Domain.Shared.SharedConstants;

namespace CustomCADs.API.Endpoints.Cads.PostCad;

public class PostCadRequestValidator : Validator<PostCadRequest>
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

        RuleFor(r => r.Image)
            .NotNull().WithMessage(RequiredErrorMessage);

        RuleFor(r => r.File)
            .NotNull().WithMessage(RequiredErrorMessage);
    }
}
