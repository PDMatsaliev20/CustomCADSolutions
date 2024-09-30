using FastEndpoints;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Endpoints.Roles
{
    using static StatusCodes;
    
    public class RolesGroup : Group
    {
        public RolesGroup()
        {
            Configure("API/Roles", ep =>
            {
                ep.Roles(Admin);
                ep.Options(opt =>
                {
                    opt.WithTags("Roles");
                    opt.ProducesProblem(Status401Unauthorized);
                    opt.ProducesProblem(Status403Forbidden);
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });
        }
    }
}
