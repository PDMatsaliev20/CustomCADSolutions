using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.OrderConstants;

namespace CustomCADs.API.Endpoints.Orders.PostOrder
{
    public class PostOrderRequestValidator : Validator<PostOrderRequest>
    {
        public PostOrderRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage(RequiredErrorMessage)
                .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);
            
            RuleFor(r => r.Description)
                .NotEmpty().WithMessage(RequiredErrorMessage)
                .Length(DescriptionMinLength, DescriptionMaxLength).WithMessage(LengthErrorMessage);

            RuleFor(r => r.CategoryId)
                .NotEmpty().WithMessage(RequiredErrorMessage);

            RuleFor(r => r.Image)
                .NotNull().WithMessage(RequiredErrorMessage);
        }
    }
}
