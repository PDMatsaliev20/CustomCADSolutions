namespace CustomCADSolutions.App.Models
{
    public class StripeSettings
    {
        public string SecretKey { get; set; } = null!;
        public string PublishableKey { get; set; } = null!;
        public string TestPublishableKey { get; set; } = null!;
        public string TestSecretKey { get; set; } = null!;
    }
}
