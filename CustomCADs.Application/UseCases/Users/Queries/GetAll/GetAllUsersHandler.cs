using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetAll;

public class GetAllUsersHandler(IUserQueries queries) : IRequestHandler<GetAllUsersQuery, UserResult>
{
    public Task<UserResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> queryable = queries.GetAll(true)
            .Filter(request.HasRT)
            .Search(request.Username, request.Email, request.FirstName, request.LastName, request.RtEndDateBefore, request.RtEndDateAfter)
            .Sort(request.Sorting);

        IEnumerable<User> users =
        [
            .. queryable
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
        ];

        UserResult response = new()
        {
            Count = queryable.Count(),
            Users = users.Adapt<ICollection<UserModel>>(),
        };
        return Task.FromResult(response);
    }
}
