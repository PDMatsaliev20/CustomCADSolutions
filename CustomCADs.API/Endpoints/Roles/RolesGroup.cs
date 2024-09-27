using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles
{
    public class RolesGroup : Group
    {
        public RolesGroup()
        {
            Configure("API/Roles", ep =>
            {

            });
        }
    }
}
