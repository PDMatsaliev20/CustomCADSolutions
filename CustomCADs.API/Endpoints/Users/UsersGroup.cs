using FastEndpoints;
using static CustomCADs.Domain.Roles.RoleConstants;

namespace CustomCADs.API.Endpoints.Users;

using static StatusCodes;

public class UsersGroup : Group
{
    public UsersGroup()
    {
        Configure("Users", ep => 
        {
            ep.Roles(Admin);
            ep.Description(opt =>
            {
                opt.WithTags("Users");
                opt.ProducesProblem(Status401Unauthorized);
                opt.ProducesProblem(Status403Forbidden);
                opt.ProducesProblem(Status500InternalServerError);
            });
        });
    }
}
