using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders
{
    public class OrdersGroup : Group
    {
        public OrdersGroup()
        {
            Configure("API/Orders", ep =>
            {

            });
        }
    }
}
