using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditById;

public class EditUserByIdHandler(IUserReads reads, IUnitOfWork uow) : IRequestHandler<EditUserByIdCommand>
{
    public async Task Handle(EditUserByIdCommand req, CancellationToken ct)
    {
        User user = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {req.Id} does not exist.");

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
