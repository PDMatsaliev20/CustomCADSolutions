using FastEndpoints;

namespace CustomCADs.API.Endpoints.Payment
{
    public class PaymentGroup : Group
    {
        public PaymentGroup()
        {
            Configure("API/Payment", ep =>
            {

            });
        }
    }
}
