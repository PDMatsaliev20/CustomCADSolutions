using CustomCADs.Application.Models.Users;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetByRefreshToken;

public record GetUserByRefreshTokenQuery(string RefreshToken) : IRequest<UserModel>;