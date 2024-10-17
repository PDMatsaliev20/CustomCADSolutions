using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsByUsername;

public class ExistsByUsernameHandler(IUserQueries queries) : IRequestHandler<ExistsByUsernameQuery, bool>
{
    public async Task<bool> Handle(ExistsByUsernameQuery req, CancellationToken ct)
    {
        bool userExists = await queries.ExistsByNameAsync(req.Username, ct: ct).ConfigureAwait(false);

        return userExists;
    }
}
