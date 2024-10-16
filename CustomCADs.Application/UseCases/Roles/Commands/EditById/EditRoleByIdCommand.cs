using CustomCADs.Application.Models.Roles;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditById;

public record EditRoleByIdCommand(string Id, RoleModel Model) : IRequest;