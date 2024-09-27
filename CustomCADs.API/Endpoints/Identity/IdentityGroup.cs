using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity
{
    public class IdentityGroup : Group
    {
        public IdentityGroup()
        {
            Configure("API/Identity", ep =>
            {

            });
        }
    }
}
