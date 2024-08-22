using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    /// <summary>
    ///     Controller for CRUD operations on Order.
    /// </summary>
    /// <param name="orderService"></param>
    /// <param name="cadService"></param>
    /// <param name="env"></param>
    /// <param name="mapper"></param>
    [Authorize(Roles = Client)]
    [ApiController]
    [Route("API/[controller]")]
    public class OrdersController(IOrderService orderService, ICadService cadService, IWebHostEnvironment env, IMapper mapper) : ControllerBase
    {
        private readonly string createdAtReturnAction = nameof(GetOrderAsync).Replace("Async", "");

        /// <summary>
        ///     Queries the User's Orders with the specified parameters.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pagination"></param>
        /// <param name="sorting"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets the User's most recent Orders.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Recent")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderResultDTO>> GetRecentOrdersAsync(int limit = 4)
        {
            try
            {
                OrderQuery query = new() { Buyer = User.Identity!.Name };
                SearchModel search = new() { Sorting = nameof(Sorting.Newest) };
                PaginationModel pagination = new() { Limit = limit };

                OrderResult result = await orderService.GetAllAsync(query, search, pagination)
                    .ConfigureAwait(false);
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

        /// <summary>
        ///     Gets the counts of the User's Orders grouped by their status.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Counts")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        public ActionResult<dynamic> CountOrdersAsync()
        {
            try
            {
                bool predicate(OrderModel o, OrderStatus s)
                    => o.Status == s && o.Buyer.UserName == User.Identity!.Name;

                int pendingOrdersCount = orderService.Count(o => predicate(o, OrderStatus.Pending));
                int begunOrdersCount = orderService.Count(o => predicate(o, OrderStatus.Begun));
                int finishedOrdersCount = orderService.Count(o => predicate(o, OrderStatus.Finished));
                int reportedOrdersCount = orderService.Count(o => predicate(o, OrderStatus.Removed));
                int removedOrdersCount = orderService.Count(o => predicate(o, OrderStatus.Removed));

                return new { pending = pendingOrdersCount, begun = begunOrdersCount, finished = finishedOrdersCount, reported = reportedOrdersCount, removed = removedOrdersCount };
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }

        /// <summary>
        ///     Gets an Order by the specified Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderExportDTO>> GetOrderAsync(int id)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id).ConfigureAwait(false);

                if (order.Buyer.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

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

        /// <summary>
        ///     Creates an Order entity in the database.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
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

        /// <summary>
        ///     Creates an Order entity with a Relation to the Cad with the specified id in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        
        /// <summary>
        ///     Updates Name, Description and CategoryId for Orders have a Pending status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
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
                    return StatusCode(403, ForbiddenAccess);
                }

                if (order.Status != OrderStatus.Pending)
                {
                    return BadRequest(ModifiedOrderNotPending);
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

        /// <summary>
        ///     Updates Order in the traditional way - with an array of operations.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchOrder"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status403Forbidden)]
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

                if (model.Buyer.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

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

        /// <summary>
        ///     Deletes the Order with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status403Forbidden)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                OrderModel model = await orderService.GetByIdAsync(id).ConfigureAwait(false);

                if (model.Buyer.UserName != User.Identity!.Name)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

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

        /// <summary>
        ///     Downloads the Cad with the specified id, as a .glb or a .zip depending on the way it was uploaded.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/DownloadCad")]
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
                    return NotFound(string.Format(ApiMessages.NotFound, "Order"));
                }

                bool orderHasCad = await orderService.HasCadAsync(id).ConfigureAwait(false);
                if (!orderHasCad)
                {
                    return BadRequest(OrderHasNoCad);
                }

                bool userOwnsOrder = await orderService.CheckOwnership(id, User.Identity!.Name!);
                if (!userOwnsOrder)
                {
                    return StatusCode(403, ForbiddenAccess);
                }

                CadModel model = await orderService.GetCadAsync(id).ConfigureAwait(false);

                byte[] cad = await env.GetCadBytes(model.Name + model.Id, model.Paths.FileExtension).ConfigureAwait(false);
                return model.Paths.FileExtension == ".glb"
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
