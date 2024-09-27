using FastEndpoints;

namespace CustomCADs.API.Endpoints.Users
{
    public class UsersGroup : Group
    {
        public UsersGroup()
        {
            Configure("API/Users", ep => 
            {

            });
        }
    }
}
