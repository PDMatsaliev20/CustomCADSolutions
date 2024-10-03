using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Categories.PostCategory
{
    public class PostCategoryRequestValidator : Validator<PostCategoryRequest>
    {
        public PostCategoryRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage(RequiredErrorMessage);
        }
    }
}
