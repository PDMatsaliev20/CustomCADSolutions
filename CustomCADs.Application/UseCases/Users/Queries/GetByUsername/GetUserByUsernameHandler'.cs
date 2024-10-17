using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetByUsername;

public class GetUserByUsernameHandler(IUserQueries queries) : IRequestHandler<GetUserByUsernameQuery, UserModel>
{
    public async Task<UserModel> Handle(GetUserByUsernameQuery req, CancellationToken ct)
    {
        User user = await queries.GetByNameAsync(req.Username, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with username: {req.Username} doesn't exist.");

        var response = user.Adapt<UserModel>();
        return response;
    }
}
