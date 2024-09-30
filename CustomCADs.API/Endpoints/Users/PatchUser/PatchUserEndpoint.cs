using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.Services;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using CustomCADs.Infrastructure.Identity.Managers;
using FastEndpoints;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace CustomCADs.API.Endpoints.Users.PatchUser
{
    using static ApiMessages;
    using static StatusCodes;

    public class PatchUserEndpoint(IAppUserManager manager, IUserService service, IRoleService roleService) : Endpoint<PatchUserRequest>
    {
        public override void Configure()
        {
            Patch("{username}");
            Group<UsersGroup>();
            Description(d => d.WithSummary("Updates a User in the traditional way - with an array of operations."));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status204NoContent);
                opt.ProducesProblem(Status400BadRequest);
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(PatchUserRequest req, CancellationToken ct)
        {
            if (!ValidatePatchPaths(req.PatchDocument.Operations, out string[] allowedPaths))
            {
                string message = string.Format(ForbiddenPatch, string.Join(", ", allowedPaths));
                await SendResultAsync(Results.BadRequest(message)).ConfigureAwait(false);
                return;
            }

            UserModel model = await service.GetByNameAsync(req.Username).ConfigureAwait(false);
            AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            if (user == null || model == null)
            {
                string message = string.Format(NotFound, "User");
                await SendResultAsync(Results.NotFound(message)).ConfigureAwait(false);
                return;
            }

            string oldRole = model.RoleName;
            string? error = null;
            req.PatchDocument.ApplyTo(model, a => error = a.ErrorMessage);
            if (string.IsNullOrEmpty(error))
            {
                await SendResultAsync(Results.BadRequest(error)).ConfigureAwait(false);
                return;
            }

            await service.EditAsync(model.Id, model).ConfigureAwait(false);
            string newRole = model.RoleName;
            
            if (oldRole != newRole)
            {
                if (!await roleService.ExistsByNameAsync(newRole).ConfigureAwait(false))
                {
                    string message = string.Format(InvalidRole, string.Join(", ", roleService.GetAllNames()));
                    await SendResultAsync(Results.BadRequest(message)).ConfigureAwait(false);
                    return;
                }

                await manager.RemoveFromRoleAsync(user, oldRole).ConfigureAwait(false);
                await manager.AddToRoleAsync(user, newRole).ConfigureAwait(false);
            }            
            await manager.UpdateAsync(user).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }

        private bool ValidatePatchPaths(List<Operation<UserModel>> operations, out string[] allowedPaths)
        {
            string[] validPaths = [nameof(UserModel.UserName), nameof(UserModel.FirstName), nameof(UserModel.LastName)];
            allowedPaths = validPaths;

            bool validateOperationPath(Operation<UserModel> op) => !validPaths.Any(p => op.path != '/' + p.ToLower());
            return operations.All(validateOperationPath);
        }
    }
}
