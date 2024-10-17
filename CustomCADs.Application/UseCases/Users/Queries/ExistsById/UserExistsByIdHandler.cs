using CustomCADs.Domain.Users.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsById;

public class UserExistsByIdHandler(IUserQueries queries) : IRequestHandler<UserExistsByIdQuery, bool>
{
    public async Task<bool> Handle(UserExistsByIdQuery req, CancellationToken ct)
    {
        bool userExists = await queries.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return userExists;
    }
}
