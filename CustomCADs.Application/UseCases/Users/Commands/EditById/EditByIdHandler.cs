using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.EditById;

public class EditUserByIdHandler(IUserQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditUserByIdCommand>
{
    public async Task Handle(EditUserByIdCommand request, CancellationToken cancellationToken)
    {
        User user = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {request.Id} does not exist.");

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
