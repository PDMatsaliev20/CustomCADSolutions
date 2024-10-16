using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteByName;

public record DeleteUserByNameCommand(string Name) : IRequest;