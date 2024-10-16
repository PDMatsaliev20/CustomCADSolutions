using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteById;

public record DeleteUserByIdCommand(string Id) : IRequest;