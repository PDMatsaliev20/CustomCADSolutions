using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.ApiControllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("All")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<OrderViewModel[]>> Get()
        {
            IEnumerable<OrderModel> orders = await orderService.GetAllAsync();
            OrderViewModel[] views = orders
                .Select(o => new OrderViewModel
                {
                    BuyerId = o.BuyerId,
                    BuyerName = o.Buyer.UserName,
                    CadId = o.CadId,
                    Category = o.Cad.Category.Name,
                    Name = o.Cad.Name,
                    Description = o.Description,
                    Status = o.Status.ToString(),
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                })
                .ToArray();

            return views;
        }

        [HttpGet("Single")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderViewModel>> Get(int cadId, string buyerId)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(cadId, buyerId);
                OrderViewModel view = new()
                {
                    BuyerId = order.BuyerId,
                    BuyerName = order.Buyer.UserName,
                    CadId = order.CadId,
                    Category = order.Cad.Category.Name,
                    Name = order.Cad.Name,
                    Description = order.Description,
                    Status = order.Status.ToString(),
                    OrderDate = order.OrderDate.ToString("dd/MM/yyyy HH:mm:ss"),
                };

                return view;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderModel>> Post(OrderInputModel input)
        {
            OrderModel order = new()
            {
                Description = input.Description,
                OrderDate = DateTime.Now,
                Status = input.Status,
                ShouldShow = true,
                BuyerId = User.GetId(),
                Cad = new CadModel()
                {
                    Name = input.Name,
                    CategoryId = input.CategoryId,
                }
            };
            (string, int) ids = await orderService.CreateAsync(order);

            var values = new { buyerId = ids.Item1, cadId = ids.Item2};
            return CreatedAtAction("Single", values, order);
        }

        [HttpPut("Edit")]
        [Consumes("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(OrderInputModel input)
        {
            OrderModel order;
            try
            {
                order = await orderService.GetByIdAsync(input.CadId, input.BuyerId!);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }

            if (order.Status != OrderStatus.Pending)
            {
                return Forbid();
            }

            order.Cad.Name = input.Name;
            order.Description = input.Description;
            order.Cad.CategoryId = input.CategoryId;
            await orderService.EditAsync(order);

            return NoContent();
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int cadId, string buyerId)
        {
            try
            {
                await orderService.DeleteAsync(cadId, buyerId);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
