using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsByUsername;

public class ExistsByUsernameHandler(IUserQueries queries) : IRequestHandler<ExistsByUsernameQuery, bool>
{
    public async Task<bool> Handle(ExistsByUsernameQuery request, CancellationToken cancellationToken)
    {
        bool userExists = await queries.ExistsByNameAsync(request.Username).ConfigureAwait(false);

        return userExists;
    }
}
