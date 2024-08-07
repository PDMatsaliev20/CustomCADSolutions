using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Orders;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class PaymentController(IOptions<StripeInfo> stripeOptions, ICadService cadService, IOrderService orderService) : ControllerBase
    {
        private readonly StripeInfo stripe = stripeOptions.Value;
        private readonly string createdAtReturnAction = nameof(OrdersController.GetOrderAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(opt =>
            opt.AddProfile<OrderApiProfile>()
        ).CreateMapper();

        [HttpPost("Purchase/{id}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> Purchase(int id, string? stripeToken)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(stripeToken))
                {
                    var charge = stripe.ProcessPayment(stripeToken, User.Identity!.Name!, cad);
                    if (charge.Status != "succeeded")
                    {
                        return BadRequest(charge.FailureMessage);
                    }
                }

                OrderModel order = new()
                {
                    Name = cad.Name,
                    Description = string.Format(CadPurchasedMessage, id),
                    Status = OrderStatus.Finished,
                    CategoryId = cad.CategoryId,
                    OrderDate = DateTime.Now,
                    CadId = id,
                    BuyerId = User.GetId(),
                };
                int newOrderId = await orderService.CreateAsync(order).ConfigureAwait(false);
                
                order = await orderService.GetByIdAsync(newOrderId).ConfigureAwait(false);
                OrderExportDTO export = mapper.Map<OrderExportDTO>(order);

                return CreatedAtAction(createdAtReturnAction, "Orders", new { id = newOrderId }, export);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex.GetMessage());
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

    }
}
