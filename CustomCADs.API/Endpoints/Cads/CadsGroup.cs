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
                ep.Description(d => d
                    .WithTags("Cads")
                    .ProducesProblem(Status400BadRequest)
                    .ProducesProblem(Status401Unauthorized)
                    .ProducesProblem(Status403Forbidden)
                    .ProducesProblem(Status404NotFound)
                    .ProducesProblem(Status409Conflict)
                    .ProducesProblem(Status500InternalServerError));
            });   
        }
    }
}
