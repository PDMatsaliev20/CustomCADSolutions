using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class OrdersController(IOrderService orderService, ICadService cadService, IWebHostEnvironment env) : ControllerBase
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
        public async Task<ActionResult<OrderResultDTO>> GetOrdersAsync(string status, [FromQuery] PaginationModel pagination, string? sorting, string? category, string? name)
        {
            try
            {
                if (!Enum.TryParse(status.Capitalize(), out OrderStatus enumStatus))
                {
                    string allowedStatuses = string.Join(", ", Enum.GetNames<OrderStatus>());
                    return BadRequest(string.Format(InvalidStatus, allowedStatuses));
                }

                OrderQuery query = new() { Buyer = User.Identity!.Name, Status = enumStatus };
                SearchModel search = new() { Category = category, Name = name, Sorting = sorting ?? string.Empty };

                OrderResult result = await orderService.GetAllAsync(query, search, pagination).ConfigureAwait(false);
                return mapper.Map<OrderResultDTO>(result);
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
                OrderModel order = await orderService.GetByIdAsync(id).ConfigureAwait(false);
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
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult> PostOrderAsync([FromForm] OrderImportDTO dto)
        {
            try
            {
                OrderModel model = mapper.Map<OrderModel>(dto);
                model.OrderDate = DateTime.Now;
                model.BuyerId = User.GetId();

                int id = await orderService.CreateAsync(model).ConfigureAwait(false);
                if (dto.Image != null)
                {
                    await env.UploadOrderAsync(dto.Image, dto.Name + id + dto.Image.GetFileExtension());
                }

                model = await orderService.GetByIdAsync(id).ConfigureAwait(false);
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

        [HttpPost("{id}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> OrderExistingAsync(int id)
        {
            try
            {
                CadModel cad = await cadService.GetByIdAsync(id).ConfigureAwait(false);
                OrderModel order = new()
                {
                    Name = cad.Name,
                    Description = string.Format(CadPurchasedMessage, id),
                    Status = OrderStatus.Finished,
                    CategoryId = cad.CategoryId,
                    OrderDate = DateTime.Now,
                    CadId = id,
                    BuyerId = User.GetId(),
                    DesignerId = cad.CreatorId,
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

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]   
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PutOrderAsync(int id, [FromForm] OrderImportDTO dto)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id).ConfigureAwait(false);

                if (User.GetId() != order.BuyerId)
                {
                    return Forbid(ForbiddenAccess);
                }

                order.Name = dto.Name;
                order.Description = dto.Description;
                order.CategoryId = dto.CategoryId;
                await orderService.EditAsync(id, order).ConfigureAwait(false);

                return NoContent();
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

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status409Conflict)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> PatchOrderAsync(int id, [FromBody] JsonPatchDocument<OrderModel> patchOrder)
        {
            string? modifiedForbiddenField = patchOrder.CheckForBadChanges("/id", "/orderDate", "/status", "/cadId", "/buyerId", "/category", "/cad", "/buyer");
            if (modifiedForbiddenField != null)
            {
                return BadRequest(string.Format(ForbiddenPatch, modifiedForbiddenField));
            }

            try
            {
                OrderModel model = await orderService.GetByIdAsync(id).ConfigureAwait(false);

                string? error = null;
                patchOrder.ApplyTo(model, p => error = p.ErrorMessage);
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                bool isValid = model.Validate(out IList<string> errors);
                if (!isValid)
                {
                    return BadRequest(string.Join("; ", errors));
                }

                await orderService.EditAsync(id, model).ConfigureAwait(false);
                return NoContent();
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

        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                OrderModel model = await orderService.GetByIdAsync(id).ConfigureAwait(false);
                if (string.IsNullOrEmpty(model.ImagePath))
                {
                    env.DeleteFile("orders", model.Name + model.Id, model.ImageExtension!);
                }

                await orderService.DeleteAsync(id).ConfigureAwait(false);
                return NoContent();
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

        [HttpGet("DownloadCad/{id}")]
        [Produces("model/gltf-binary", "application/zip")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DownloadCad(int id)
        {
            try
            {
                bool orderExists = await orderService.ExistsByIdAsync(id).ConfigureAwait(false);
                if (!orderExists)
                {
                    return NotFound();
                }

                bool orderHasCad = await orderService.HasCadAsync(id).ConfigureAwait(false);
                if (!orderHasCad)
                {
                    return BadRequest();
                }

                bool userOwnsOrder = await orderService.CheckOwnership(id, User.Identity!.Name!);
                if (!userOwnsOrder)
                {
                    return StatusCode(403);
                }

                CadModel model = await orderService.GetCadAsync(id).ConfigureAwait(false);
                
                byte[] cad = await env.GetCadBytes(model.Name + model.Id, model.CadExtension).ConfigureAwait(false);
                return model.CadExtension == ".glb"
                    ? File(cad, "model/gltf-binary", $"{model.Name}.glb")
                    : File(cad, "application/zip", $"{model.Name}.zip");
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
    }
}
