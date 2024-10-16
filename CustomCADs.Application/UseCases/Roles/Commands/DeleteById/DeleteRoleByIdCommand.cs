using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteById;

public record DeleteRoleByIdCommand(string Id) : IRequest;
