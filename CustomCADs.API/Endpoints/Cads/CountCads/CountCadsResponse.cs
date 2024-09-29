namespace CustomCADs.API.Endpoints.Cads.CountCads
{
    public record CountCadsResponse(
            int Unchecked,
            int Validated,
            int Reported,
            int Banned
        )
    {
    }
}
