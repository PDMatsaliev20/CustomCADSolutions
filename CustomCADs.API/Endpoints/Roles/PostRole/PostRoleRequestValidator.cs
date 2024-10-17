using FastEndpoints;
using FluentValidation;
using static CustomCADs.Domain.Shared.SharedConstants;
using static CustomCADs.Domain.Roles.RoleConstants;

namespace CustomCADs.API.Endpoints.Roles.PostRole;

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
