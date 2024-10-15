using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Cads;

using static StatusCodes;

public class CadsGroup : Group
{
    public CadsGroup()
    {
        Configure("Cads", ep =>
        {
            ep.Roles(RoleConstants.Contributor, RoleConstants.Designer);
            ep.Description(d => d
                .WithTags("Cads")
                .ProducesProblem(Status401Unauthorized)
                .ProducesProblem(Status403Forbidden)
                .ProducesProblem(Status500InternalServerError));
        });   
    }
}
