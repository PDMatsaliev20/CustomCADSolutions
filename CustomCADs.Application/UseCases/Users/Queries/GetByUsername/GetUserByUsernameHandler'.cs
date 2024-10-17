using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetByUsername;

public class GetUserByUsernameHandler(IUserQueries queries) : IRequestHandler<GetUserByUsernameQuery, UserModel>
{
    public async Task<UserModel> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        User user = await queries.GetByNameAsync(request.Username).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with username: {request.Username} doesn't exist.");

        var response = user.Adapt<UserModel>();
        return response;
    }
}
