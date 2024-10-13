using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home
{
    using static StatusCodes;

    public class HomeGroup : Group
    {
        public HomeGroup()
        {
            Configure("Home", ep =>
            {
                ep.AllowAnonymous();
                ep.Description(opt =>
                {
                    opt.WithTags("Home");
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });
        }
    }
}
