using CustomCADs.Application.Models.Roles;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.Create;

public record CreateRoleCommand(RoleModel Model) : IRequest<string>;