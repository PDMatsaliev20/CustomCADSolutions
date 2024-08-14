using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    [ApiController]
    [Route("API/[controller]")]
    [Authorize(Designer)]
    public class DesignerController(ICadService cadService, IOrderService orderService, IMapper mapper) : ControllerBase
    {
        [HttpGet("Cads")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetUncheckedCadsAsync([FromQuery] PaginationModel pagination, string? sorting, string? category, string? name, string? creator)
        {
            CadQuery query = new() { Status = CadStatus.Unchecked };
            SearchModel search = new() { Category = category, Owner = creator, Name = name, Sorting = sorting ?? ""};

            try
            {
                CadResult result = await cadService.GetAllAsync(query, search, pagination).ConfigureAwait(false);
                return mapper.Map<CadQueryResultDTO>(result);
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

        [HttpPatch("Cads/Status/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> UpdateCadStatusAsync(int id, string status)
        {
            try
            {
                await cadService.EditStatusAsync(id, Enum.Parse<CadStatus>(status)).ConfigureAwait(false);
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

        [HttpGet("Orders")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderResultDTO>> GetOrdersByStatusAsync(string status, [FromQuery] PaginationModel pagination, string? sorting, string? category, string? name, string? buyer)
        {
            try
            {
                if (!Enum.TryParse(status.Capitalize(), out OrderStatus enumStatus))
                {
                    string allowedStatuses = string.Join(", ", Enum.GetNames<OrderStatus>());
                    return BadRequest(string.Format(InvalidStatus, allowedStatuses));
                }

                OrderQuery query = new() { Status = Enum.Parse<OrderStatus>(status.Capitalize()) };
                SearchModel search = new() { Category = category, Name = name, Owner = buyer, Sorting = sorting ?? string.Empty };
                
                OrderResult result = await orderService.GetAllAsync(query, search, pagination, 
                    order => order.Status != OrderStatus.Finished || order.ShouldBeDelivered).ConfigureAwait(false);
                
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
        
        [HttpGet("Orders/Recent")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderResultDTO>> GetRecentOrdersAsync()
        {
            try
            {
                OrderQuery query = new() { Status = OrderStatus.Finished };
                SearchModel search = new() { Sorting = "newest" };
                PaginationModel pagination = new() { Limit = 4 };

                OrderResult result = await orderService.GetAllAsync(query, search, pagination,
                    o => o.DesignerId == User.GetId()).ConfigureAwait(false);
                
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

        [HttpPatch("Orders/Begin/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> BeginOrderAsync(int id)
        {
            try
            {
                await orderService.BeginAsync(id, User.GetId()).ConfigureAwait(false);
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
        
        [HttpPatch("Orders/Report/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> ReportOrderAsync(int id)
        {
            try
            {
                await orderService.ReportAsync(id).ConfigureAwait(false);
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
        
        [HttpPatch("Orders/Cancel/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> CancelOrderAsync(int id)
        {
            try
            {
                await orderService.CancelAsync(id).ConfigureAwait(false);
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

        [HttpPatch("Orders/Finish/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> CompleteOrderAsync(int id, int cadId)
        {
            try
            {
                if (!await cadService.ExistsByIdAsync(cadId).ConfigureAwait(false))
                {
                    throw new KeyNotFoundException();
                }

                await orderService.CompleteAsync(id, cadId).ConfigureAwait(false);
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
    }
}
