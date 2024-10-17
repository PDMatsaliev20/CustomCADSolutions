using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetByUsername;

public class GetUserByUsernameHandler(IUserReads reads) : IRequestHandler<GetUserByUsernameQuery, UserModel>
{
    public async Task<UserModel> Handle(GetUserByUsernameQuery req, CancellationToken ct)
    {
        User user = await reads.GetByNameAsync(req.Username, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with username: {req.Username} doesn't exist.");

        var response = user.Adapt<UserModel>();
        return response;
    }
}
