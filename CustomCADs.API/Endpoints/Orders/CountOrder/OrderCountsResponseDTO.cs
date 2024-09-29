namespace CustomCADs.API.Endpoints.Orders.CountOrder
{
    public record OrderCountsResponseDTO(
        int Pending,
        int Begun,
        int Finished,
        int Reported,
        int Removed)
    {

    }
}
