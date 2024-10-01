using FastEndpoints;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Endpoints.Orders
{
    using static StatusCodes;

    public class OrdersGroup : Group
    {
        public OrdersGroup()
        {
            Configure("API/Orders", ep =>
            {
                ep.Roles(Client);
                ep.Description(opt =>
                {
                    opt.WithTags("Orders");
                    opt.ProducesProblem(Status400BadRequest);
                    opt.ProducesProblem(Status401Unauthorized);
                    opt.ProducesProblem(Status403Forbidden);
                    opt.ProducesProblem(Status404NotFound);
                    opt.ProducesProblem(Status409Conflict);
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });
        }
    }
}
