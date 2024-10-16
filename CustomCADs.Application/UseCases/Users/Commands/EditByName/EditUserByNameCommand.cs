using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditByName;

public record EditUserByNameCommand(string Name, UserModel Model) : IRequest;