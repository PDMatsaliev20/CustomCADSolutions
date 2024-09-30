using FastEndpoints;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Endpoints.Payment
{
    using static StatusCodes;

    public class PaymentGroup : Group
    {
        public PaymentGroup()
        {
            Configure("API/Payment", ep =>
            {
                ep.Roles(Client);
                ep.Options(opt =>
                {
                    opt.WithTags("Payment");
                    opt.ProducesProblem(Status401Unauthorized);
                    opt.ProducesProblem(Status403Forbidden);
                    opt.ProducesProblem(Status500InternalServerError);
                });
            });
        }
    }
}
