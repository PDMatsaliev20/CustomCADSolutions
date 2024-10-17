using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetById;

public class GetUserByIdHandler(IUserQueries queries) : IRequestHandler<GetUserByIdQuery, UserModel>
{
    public async Task<UserModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User user = await queries.GetByIdAsync(request.Id, asNoTracking: true).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {request.Id} doesn't exist.");

        var response = user.Adapt<UserModel>();
        return response;
    }
}
