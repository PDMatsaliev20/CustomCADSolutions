using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsById;

public class UserExistsByIdHandler(IUserQueries queries) : IRequestHandler<UserExistsByIdQuery, bool>
{
    public async Task<bool> Handle(UserExistsByIdQuery request, CancellationToken cancellationToken)
    {
        bool userExists = await queries.ExistsByIdAsync(request.Id).ConfigureAwait(false);

        return userExists;
    }
}
