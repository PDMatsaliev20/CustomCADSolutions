using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Designer
{
    using static StatusCodes;

    public class DesignerGroup : Group
    {
        public DesignerGroup()
        {
            Configure("API/Designer", ep =>
            {
                ep.Roles(RoleConstants.Designer);
                ep.Options(opt =>
                {
                    opt.WithTags("Desigenr");
                    opt.ProducesProblem(Status401Unauthorized);
                    opt.ProducesProblem(Status403Forbidden);
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });
        }
    }
}
