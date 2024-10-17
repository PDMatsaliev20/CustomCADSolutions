using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetByRefreshToken;

public class GetUserByRefreshTokenHandler(IUserQueries queries) : IRequestHandler<GetUserByRefreshTokenQuery, UserModel>
{
    public async Task<UserModel> Handle(GetUserByRefreshTokenQuery req, CancellationToken cancellationToken
        )
    {
        User user = await queries.GetByRefreshTokenAsync(req.RefreshToken, ct: cancellationToken).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with refresh token: {req.RefreshToken} doesn't exist.");

        var response = user.Adapt<UserModel>();
        return response;
    }
}
