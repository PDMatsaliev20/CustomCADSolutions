using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditByName;

public class EditUserByNameHandler(IUserQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditUserByNameCommand>
{
    public async Task Handle(EditUserByNameCommand request, CancellationToken cancellationToken)
    {
        User user = await queries.GetByNameAsync(request.Name).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with name: {request.Name} does not exist.");

        user.UserName = request.Model.UserName;
        user.FirstName = request.Model.FirstName;
        user.LastName = request.Model.LastName;
        user.Email = request.Model.Email;
        user.RoleName = request.Model.RoleName;
        user.RefreshToken = request.Model.RefreshToken;
        user.RefreshTokenEndDate = request.Model.RefreshTokenEndDate;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
