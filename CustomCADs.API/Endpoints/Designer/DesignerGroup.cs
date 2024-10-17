using CustomCADs.Domain.Roles;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer;

using static StatusCodes;

public class DesignerGroup : Group
{
    public DesignerGroup()
    {
        Configure("Designer", ep =>
        {
            ep.Roles(RoleConstants.Designer);
            ep.Description(opt =>
            {
                opt.WithTags("Designer");
                opt.ProducesProblem(Status401Unauthorized);
                opt.ProducesProblem(Status403Forbidden);
                opt.ProducesProblem(Status500InternalServerError);
            });
        });
    }
}
