using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Infrastructure.Payment.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class PaymentController(IStripeService stripeService, ICadService cadService, IOrderService orderService) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(OrdersController.GetOrderAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(opt =>
            opt.AddProfile<OrderApiProfile>()
        ).CreateMapper();

        [HttpGet("GetPublicKey")]
        public ActionResult<string> GetPublicKey() => stripeService.GetPublicKey();

        [HttpPost("Purchase/{id}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> Purchase(int id, string paymentMethodId)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id).ConfigureAwait(false);
                PaymentIntent paymentIntent = await stripeService.ProcessPayment(paymentMethodId, new()
                {
                    Product = cad.Name,
                    Price = cad.Price,
                    Seller = cad.Creator!.UserName!,
                    Buyer = User.Identity!.Name!,
                }).ConfigureAwait(false);

                switch (paymentIntent.Status)
                {
                    case "succeeded": return Ok(SuccessfulPayment);
                    case "processing": return Ok(ProcessingPayment);
                    case "canceled": return BadRequest(CanceledPayment);
                    case "requires_payment_method": return BadRequest(FailedPaymentMethod);

                    case "requires_action":
                        return BadRequest(new
                        {
                            Message = FailedPayment,
                            paymentIntent.ClientSecret
                        });

                    case "requires_capture":
                        PaymentIntent capturedIntent = await stripeService.CapturePaymentAsync(paymentIntent.Id);
                        return capturedIntent.Status != "succeeded"
                            ? BadRequest(FailedPaymentCapture)
                            : Ok(SuccessfulPayment);

                    default: return BadRequest(string.Format(UnhandledPayment, paymentIntent.Status));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPost("CapturePayment/{id}")]
        public async Task<IActionResult> CapturePayment(string id)
        {
            PaymentIntent paymentIntent = await stripeService.CapturePaymentAsync(id);
            return paymentIntent.Status == "succeeded"
                ? Ok(SuccessfulPaymentCapture)
                : BadRequest(FailedPaymentCapture);
        }
    }
}
