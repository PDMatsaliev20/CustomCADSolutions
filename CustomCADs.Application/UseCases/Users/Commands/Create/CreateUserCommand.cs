using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.Create;

public record CreateUserCommand(UserModel Model) : IRequest<string>;