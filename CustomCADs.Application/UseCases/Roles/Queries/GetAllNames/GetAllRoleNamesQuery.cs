using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAllNames;

public record GetAllRoleNamesQuery : IRequest<IEnumerable<string>>;