using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home
{
    public class HomeGroup : Group
    {
        public HomeGroup()
        {
            Configure("API/Home", ep =>
            {
                ep.Options(opt => opt.WithTags("Home"));
                ep.AllowAnonymous();
            });
        }
    }
}
