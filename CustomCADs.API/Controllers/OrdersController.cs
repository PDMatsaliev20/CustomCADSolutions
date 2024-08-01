using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Orders;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetSingleAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderApiProfile>();
            cfg.AddProfile<CadApiProfile>();
            cfg.AddProfile<OtherApiProfile>();
        }).CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<OrderExportDTO[]>> GetOrdersAsync()
        {
            try
            {
                IEnumerable<OrderModel> orders = (await orderService.GetAllAsync())
                    .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Begun)
                    .OrderByDescending(o => (int)o.Status)
                    .ThenBy(o => o.OrderDate);

                return mapper.Map<OrderExportDTO[]>(orders);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet("Completed")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<OrderExportDTO[]>> GetCompletedOrdersAsync()
        {
            try
            {
                IEnumerable<OrderModel> orders = (await orderService.GetAllAsync())
                    .Where(o => !(o.Status == OrderStatus.Pending || o.Status == OrderStatus.Begun))
                    .OrderByDescending(o => (int)o.Status).ThenBy(o => o.OrderDate);

                return mapper.Map<OrderExportDTO[]>(orders);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<OrderExportDTO>> GetSingleAsync(int id)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id);
                return mapper.Map<OrderExportDTO>(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<OrderExportDTO>> PostAsync(OrderImportDTO dto)
        {
            OrderModel model = mapper.Map<OrderModel>(dto);
            model.OrderDate = DateTime.Now;
            model.BuyerId = User.GetId();
            
            try
            {
                int id = await orderService.CreateAsync(model);
                
                model = await orderService.GetByIdAsync(id);
                OrderExportDTO export = mapper.Map<OrderExportDTO>(model);

                return CreatedAtAction(createdAtReturnAction, new { id }, export);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> PutAsync(int id, OrderImportDTO dto)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id);

                if (User.Identity!.Name != order.Buyer.UserName)
                {
                    return Forbid();
                }

                order.Name = dto.Name;
                order.Description = dto.Description;
                order.CategoryId = dto.CategoryId;
                await orderService.EditAsync(id, order);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<OrderModel> patchOrder)
        {
            try
            {
                OrderModel model = await orderService.GetByIdAsync(id);
                patchOrder.ApplyTo(model);

                IList<string> errors = orderService.ValidateEntity(model);
                if (errors.Any())
                {
                    return BadRequest(string.Join("; ", errors));
                }

                await orderService.EditAsync(id, model);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<OrderModel>> DeleteAsync(int id)
        {
            try
            {
                if (await orderService.ExistsByIdAsync(id))
                {
                    await orderService.DeleteAsync(id);
                    return NoContent();
                }
                else return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetCad/{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<CadGetDTO>> GetCadAsync(int id)
        {
            OrderModel order = await orderService.GetByIdAsync(id);
            if (order.CadId != null)
            {
                return mapper.Map<CadGetDTO>(order.Cad);
            }
            else return NotFound();
        }
    }
}
