using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.DTOs.Payment;
using CustomCADs.Application.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    /// <summary>
    ///     Controller for managing payments on the server.
    /// </summary>
    /// <param name="paymentService"></param>
    /// <param name="cadService"></param>
    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class PaymentController(IPaymentService paymentService, ICadService cadService) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(OrdersController.GetOrderAsync).Replace("Async", "");

        /// <summary>
        ///     Gets the Public Key.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPublicKey")]
        public ActionResult<string> GetPublicKey() => paymentService.GetPublicKey();

        /// <summary>
        ///     Initializes the Payment Intent and returns a Client Secret if an error occurs.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        [HttpPost("{id}/Purchase")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> Purchase(int id, string paymentMethodId)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id).ConfigureAwait(false);
                PaymentResult paymentIntent = await paymentService.InitializePayment(paymentMethodId, new()
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
                        (await paymentService.CapturePaymentAsync(paymentIntent.Id)).Status == "succeeded"
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
