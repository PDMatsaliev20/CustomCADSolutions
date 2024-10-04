using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace CustomCADs.API.Endpoints.Roles.PatchRole
{
    using static ApiMessages;
    using static StatusCodes;

    public class PatchRoleEndpoint(IAppRoleManager manager, IRoleService service) : Endpoint<PatchRoleRequest>
    {
        public override void Configure()
        {
            Patch("{name}");
            Group<RolesGroup>();
            Description(d => d
                .WithSummary("Updates a Role in the traditional way - with an array of operations.")
                .Produces<EmptyResponse>(Status204NoContent)
                .ProducesProblem(Status400BadRequest)
                .ProducesProblem(Status404NotFound));
        }

        public override async Task HandleAsync(PatchRoleRequest req, CancellationToken ct)
        {
            if (!ValidatePatchPaths(req.PatchDocument.Operations, out string[] allowedPaths))
            {
                string message = string.Format(ForbiddenPatch, string.Join(", ", allowedPaths));
                await SendResultAsync(Results.BadRequest(message)).ConfigureAwait(false);
                return;
            }

            RoleModel model = await service.GetByNameAsync(req.Name).ConfigureAwait(false);
            AppRole? role = await manager.FindByNameAsync(req.Name).ConfigureAwait(false);
            if (role == null || model == null)
            {
                string message = string.Format(NotFound, "Role");
                await SendResultAsync(Results.NotFound(message)).ConfigureAwait(false);
                return;
            }

            string? error = null;
            req.PatchDocument.ApplyTo(model, a => error = a.ErrorMessage);
            if (string.IsNullOrEmpty(error))
            {
                await SendResultAsync(Results.BadRequest(error)).ConfigureAwait(false);
                return;
            }

            await service.EditAsync(model.Id, model).ConfigureAwait(false);
            await manager.UpdateAsync(role).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }

        private bool ValidatePatchPaths(List<Operation<RoleModel>> operations, out string[] allowedPaths)
        {
            string[] validPaths = [nameof(RoleModel.Name), nameof(RoleModel.Description)];
            allowedPaths = validPaths;

            bool validateOperationPath(Operation<RoleModel> op) => !validPaths.Any(p => op.path != '/' + p.ToLower());
            return operations.All(validateOperationPath);
        }
    }
}
