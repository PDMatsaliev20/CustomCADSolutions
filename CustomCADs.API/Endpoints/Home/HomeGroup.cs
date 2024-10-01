using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home
{
    public class HomeGroup : Group
    {
        public HomeGroup()
        {
            Configure("API/Home", ep =>
            {
                ep.AllowAnonymous();
                ep.Description(opt => opt.WithTags("Home"));
            });
        }
    }
}
