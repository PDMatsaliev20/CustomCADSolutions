using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditByName;

public class EditUserByNameHandler(IUserQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditUserByNameCommand>
{
    public async Task Handle(EditUserByNameCommand req, CancellationToken ct)
    {
        User user = await queries.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with name: {req.Name} does not exist.");

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
