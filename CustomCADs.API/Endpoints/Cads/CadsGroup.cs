using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Cads
{
    using static StatusCodes;

    public class CadsGroup : Group
    {
        public CadsGroup()
        {
            Configure("API/Cads", ep =>
            {
                ep.Roles(RoleConstants.Contributor, RoleConstants.Designer);
                ep.Options(opt =>
                {
                    opt.WithTags("Cads");
                    opt.ProducesProblem(Status400BadRequest);
                    opt.ProducesProblem(Status401Unauthorized);
                    opt.ProducesProblem(Status403Forbidden);
                    opt.ProducesProblem(Status404NotFound);
                    opt.ProducesProblem(Status409Conflict);
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });   
        }
    }
}
