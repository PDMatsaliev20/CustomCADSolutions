using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetByUsername;

public record GetUserByUsernameQuery(string Username) : IRequest<UserModel>;