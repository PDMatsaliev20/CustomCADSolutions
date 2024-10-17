using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.GetAll;

public class GetAllUsersHandler(IUserReads reads) : IRequestHandler<GetAllUsersQuery, UserResult>
{
    public Task<UserResult> Handle(GetAllUsersQuery req, CancellationToken ct)
    {
        IQueryable<User> queryable = reads.GetAll(asNoTracking: true)
            .Filter(req.HasRT)
            .Search(req.Username, req.Email, req.FirstName, req.LastName, req.RtEndDateBefore, req.RtEndDateAfter)
            .Sort(req.Sorting);

        IEnumerable<User> users =
        [
            .. queryable
            .Skip((req.Page - 1) * req.Limit)
            .Take(req.Limit)
        ];

        UserResult response = new()
        {
            Count = queryable.Count(),
            Users = users.Adapt<ICollection<UserModel>>(),
        };
        return Task.FromResult(response);
    }
}
