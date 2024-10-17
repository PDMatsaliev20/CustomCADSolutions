using CustomCADs.Domain.Users.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsByUsername;

public class ExistsByUsernameHandler(IUserReads reads) : IRequestHandler<ExistsByUsernameQuery, bool>
{
    public async Task<bool> Handle(ExistsByUsernameQuery req, CancellationToken ct)
    {
        bool userExists = await reads.ExistsByNameAsync(req.Username, ct: ct).ConfigureAwait(false);

        return userExists;
    }
}
