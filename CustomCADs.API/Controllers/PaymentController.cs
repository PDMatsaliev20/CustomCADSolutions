using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Infrastructure.Payment.Contracts;
using CustomCADs.Infrastructure.Payment.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class PaymentController(IStripeService stripeService, ICadService cadService) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(OrdersController.GetOrderAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(opt =>
            opt.AddProfile<OrderApiProfile>()
        ).CreateMapper();

        [HttpGet("GetPublicKey")]
        public ActionResult<string> GetPublicKey() => stripeService.GetPublicKey();

        [HttpPost("Purchase/{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> Purchase(int id, string paymentMethodId)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id).ConfigureAwait(false);
                PaymentResult paymentIntent = await stripeService.ProcessPayment(paymentMethodId, new()
                {
                    Product = cad.Name,
                    Price = cad.Price,
                    Seller = cad.Creator!.UserName!,
                    Buyer = User.Identity!.Name!,
                }).ConfigureAwait(false);

                return paymentIntent.Status switch
                {
                    "succeeded" => Ok(SuccessfulPayment),
                    "processing" => Ok(ProcessingPayment),
                    "canceled" => BadRequest(CanceledPayment),
                    "requires_payment_method" => BadRequest(FailedPaymentMethod),
                    "requires_action" => BadRequest(new { Message = FailedPayment, paymentIntent.ClientSecret }),
                    "requires_capture" =>
                        (await stripeService.CapturePaymentAsync(paymentIntent.Id)).Status == "succeeded"
                            ? Ok(SuccessfulPayment)
                            : BadRequest(FailedPaymentCapture),
                    _ => BadRequest(string.Format(UnhandledPayment, paymentIntent.Status))
                };
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
