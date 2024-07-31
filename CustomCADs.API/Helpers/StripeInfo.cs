namespace CustomCADs.API.Helpers
{
    public class StripeInfo
    {
        public string LiveSecretKey { get; set; } = null!;
        public string LivePublishableKey { get; set; } = null!;
        public string TestPublishableKey { get; set; } = null!;
        public string TestSecretKey { get; set; } = null!;
    }
}
