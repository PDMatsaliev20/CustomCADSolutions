﻿using CustomCADs.Application.Contracts;
using CustomCADs.Application.Services;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using CustomCADs.Infrastructure.Identity.Managers;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Users.DeleteUser
{
    using static StatusCodes;

    public class DeleteUserEndpoint(IAppUserManager manager, IUserService service) : Endpoint<DeleteUserRequest>
    {
        public override void Configure()
        {
            Delete("{username}");
            Group<UsersGroup>();
            Description(d => d.WithSummary("Deletes the User with the specified username."));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status204NoContent);
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
        {
            AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            if (user == null)
            {
                string message = string.Format(ApiMessages.NotFound, "User");
                await SendResultAsync(Results.NotFound(message)).ConfigureAwait(false);
                return; 
            }

            await manager.DeleteAsync(user).ConfigureAwait(false);
            await service.DeleteAsync(req.Username).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
