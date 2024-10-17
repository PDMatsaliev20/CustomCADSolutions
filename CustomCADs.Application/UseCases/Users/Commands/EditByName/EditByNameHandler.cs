using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditByName;

public class EditUserByNameHandler(IUserReads reads, IUnitOfWork uow) : IRequestHandler<EditUserByNameCommand>
{
    public async Task Handle(EditUserByNameCommand req, CancellationToken ct)
    {
        User user = await reads.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with name: {req.Name} does not exist.");

        user.UserName = req.Model.UserName;
        user.FirstName = req.Model.FirstName;
        user.LastName = req.Model.LastName;
        user.Email = req.Model.Email;
        user.RoleName = req.Model.RoleName;
        user.RefreshToken = req.Model.RefreshToken;
        user.RefreshTokenEndDate = req.Model.RefreshTokenEndDate;

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
