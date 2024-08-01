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
        private readonly string createdAtReturnAction = nameof(GetOrderAsync).Replace("Async", "");
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderApiProfile>();
            cfg.AddProfile<CadApiProfile>();
            cfg.AddProfile<OtherApiProfile>();
        }).CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
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
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("Completed")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderExportDTO[]>> GetCompletedOrdersAsync()
        {
            try
            {
                IEnumerable<OrderModel> orders = (await orderService.GetAllAsync())
                    .Where(o => !(o.Status == OrderStatus.Pending || o.Status == OrderStatus.Begun))
                    .OrderByDescending(o => (int)o.Status).ThenBy(o => o.OrderDate);

                return mapper.Map<OrderExportDTO[]>(orders);
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderExportDTO>> GetOrderAsync(int id)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id);
                return mapper.Map<OrderExportDTO>(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostOrderAsync(OrderImportDTO dto)
        {
            try
            {
                OrderModel model = mapper.Map<OrderModel>(dto);
                model.OrderDate = DateTime.Now;
                model.BuyerId = User.GetId();

                int id = await orderService.CreateAsync(model);
                model = await orderService.GetByIdAsync(id);

                OrderExportDTO export = mapper.Map<OrderExportDTO>(model);
                return CreatedAtAction(createdAtReturnAction, new { id }, export);
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PutOrderAsync(int id, OrderImportDTO dto)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id);

                if (User.Identity!.Name != order.Buyer.UserName)
                {
                    return Forbid("Not allowed to access another Client's order!");
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
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status409Conflict)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchOrderAsync(int id, [FromBody] JsonPatchDocument<OrderModel> patchOrder)
        {
            // Forbid modifying certain Order properties
            string? modifiedForbiddenField = patchOrder.CheckForBadChanges("/id", "/orderDate", "/status", "/cadId", "/buyerId", "/category", "/cad", "/buyer");
            if (modifiedForbiddenField != null)
            {
                return BadRequest(modifiedForbiddenField);
            }

            try
            {
                OrderModel model = await orderService.GetByIdAsync(id);

                string? error = null;
                patchOrder.ApplyTo(model, p => error = p.ErrorMessage);
                if (error != null)
                {
                    return BadRequest(error);
                }

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
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                await orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        [HttpGet("GetCad/{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<CadGetDTO>> GetCadAsync(int id)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id);
                if (order.CadId == null)
                {
                    return NotFound();
                }

                return mapper.Map<CadGetDTO>(order.Cad);
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
