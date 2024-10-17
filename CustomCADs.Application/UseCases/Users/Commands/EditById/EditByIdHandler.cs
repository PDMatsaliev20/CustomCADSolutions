using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditById;

public class EditUserByIdHandler(IUserQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditUserByIdCommand>
{
    public async Task Handle(EditUserByIdCommand req, CancellationToken ct)
    {
        User user = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {req.Id} does not exist.");

        user.UserName = req.Model.UserName;
        user.FirstName = req.Model.FirstName;
        user.LastName = req.Model.LastName;
        user.Email = req.Model.Email;
        user.RoleName = req.Model.RoleName;
        user.RefreshToken = req.Model.RefreshToken;
        user.RefreshTokenEndDate = req.Model.RefreshTokenEndDate;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
