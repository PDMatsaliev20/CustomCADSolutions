using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity
{
    using static StatusCodes;

    public class IdentityGroup : Group
    {
        public IdentityGroup()
        {
            Configure("API/Identity", ep =>
            {
                ep.AllowAnonymous();
                ep.Description(opt =>
                {
                    opt.WithTags("Identity");
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });
        }
    }
}
