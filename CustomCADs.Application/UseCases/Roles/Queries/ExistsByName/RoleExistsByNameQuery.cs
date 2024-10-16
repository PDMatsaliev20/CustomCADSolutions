using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;

public record RoleExistsByNameQuery(string Name) : IRequest<bool>;
