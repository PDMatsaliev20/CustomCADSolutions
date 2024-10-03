using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Endpoints.Roles.PostRole
{
    public class PostRoleRequestValidator : Validator<PostRoleRequest>
    {
        public PostRoleRequestValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage(RequiredErrorMessage)
                .Length(NameMinLength, NameMaxLength).WithMessage(LengthErrorMessage);

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage(RequiredErrorMessage)
                .Length(DescriptionMinLength, DescriptionMaxLength).WithMessage(LengthErrorMessage);
        }
    }
}
