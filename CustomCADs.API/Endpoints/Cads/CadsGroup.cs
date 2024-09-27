using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads
{
    public class CadsGroup : Group
    {
        public CadsGroup()
        {
            Configure("API/Cads", ep =>
            {

            });   
        }
    }
}
