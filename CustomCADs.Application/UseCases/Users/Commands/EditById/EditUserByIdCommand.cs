using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditById;

public record EditUserByIdCommand(string Id, UserModel Model) : IRequest;