namespace CustomCADs.API.Endpoints.Orders.CountOrder;

public record OrderCountsResponse(
    int Pending,
    int Begun,
    int Finished,
    int Reported,
    int Removed)
{

}
