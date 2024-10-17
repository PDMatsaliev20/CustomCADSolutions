using CustomCADs.Application.Common.Contracts;
using CustomCADs.Infrastructure.Email;
using CustomCADs.Infrastructure.Payment;
using Stripe;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection;

public static class InfrastructureProgramExtensions
{
    public static void AddStripeServices(this IServiceCollection services)
    {
        services.AddScoped<PaymentIntentService>();
        services.AddScoped<IPaymentService, StripeService>();
    }
    
    public static void AddEmailServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, MailKitService>();
    }
}
