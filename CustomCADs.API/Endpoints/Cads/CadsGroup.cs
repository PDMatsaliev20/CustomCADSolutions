using FastEndpoints;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Endpoints.Cads
{
    using static StatusCodes;

    public class CadsGroup : Group
    {
        public CadsGroup()
        {
            Configure("API/Cads", ep =>
            {
                ep.Roles(Contributor, Designer);
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
