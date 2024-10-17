using CustomCADs.Domain.Users.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Queries.ExistsById;

public class UserExistsByIdHandler(IUserReads reads) : IRequestHandler<UserExistsByIdQuery, bool>
{
    public async Task<bool> Handle(UserExistsByIdQuery req, CancellationToken ct)
    {
        bool userExists = await reads.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return userExists;
    }
}
