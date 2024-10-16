using CustomCADs.Application.Models.Roles;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditByName;

public record EditRoleByNameCommand(string Name, RoleModel Model) : IRequest;