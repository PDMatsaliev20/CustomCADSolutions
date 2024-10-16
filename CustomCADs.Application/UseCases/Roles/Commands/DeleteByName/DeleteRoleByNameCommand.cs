using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteByName;

public record DeleteRoleByNameCommand(string Name) : IRequest;
