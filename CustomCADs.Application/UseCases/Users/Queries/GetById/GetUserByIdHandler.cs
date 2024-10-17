using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetById;

public class GetUserByIdHandler(IUserQueries queries) : IRequestHandler<GetUserByIdQuery, UserModel>
{
    public async Task<UserModel> Handle(GetUserByIdQuery req, CancellationToken ct)
    {
        User user = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {req.Id} doesn't exist.");

        var response = user.Adapt<UserModel>();
        return response;
    }
}
