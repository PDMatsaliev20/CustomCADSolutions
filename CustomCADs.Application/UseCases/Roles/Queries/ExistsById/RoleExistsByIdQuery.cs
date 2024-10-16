using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsById;

public record RoleExistsByIdQuery(string Id) : IRequest<bool>;
