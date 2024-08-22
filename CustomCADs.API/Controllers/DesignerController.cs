using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;
    using static ApiMessages;

    /// <summary>
    ///     Controller for Updating Status of Cad and Order.
    /// </summary>
    /// <param name="designerService"></param>
    /// <param name="mapper"></param>
    [ApiController]
    [Route("API/[controller]")]
    [Authorize(Designer)]
    public class DesignerController(IDesignerService designerService, IMapper mapper) : ControllerBase
    {
        /// <summary>
        ///     Gets all Cads with Unchecked status.
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="creator"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("Cads")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetUncheckedCadsAsync(string? sorting, string? category, string? name, string? creator, int page = 1, int limit = 6)
        {
            SearchModel search = new() { Category = category, Owner = creator, Name = name, Sorting = sorting ?? ""};
            PaginationModel pagination = new() { Page = page, Limit = limit };

            try
            {
                CadResult result = await designerService.GetCadsAsync(search, pagination).ConfigureAwait(false);
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

        /// <summary>
        ///     Updates the specified Cad with the specified Status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPatch("Cads/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> UpdateCadStatusAsync(int id, string status)
        {
            try
            {
                await designerService.EditCadStatusAsync(id, Enum.Parse<CadStatus>(status)).ConfigureAwait(false);
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
        ///     Gets all Orders with specified status.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="sorting"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="buyer"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("Orders")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderResultDTO>> GetOrdersByStatusAsync(string status, string? sorting, string? category, string? name, string? buyer, int page = 1, int limit = 20)
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
                PaginationModel pagination = new() { Page = page, Limit = limit };

                OrderResult result = await designerService.GetOrdersAsync(status, null, search, pagination).ConfigureAwait(false);

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
        ///     Gets the User's most recent finished Orders.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("Orders/Recent")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderResultDTO>> GetRecentOrdersAsync(int limit = 4)
        {
            try
            {
                OrderQuery query = new() { Status = OrderStatus.Finished };
                SearchModel search = new() { Sorting = "newest" };
                PaginationModel pagination = new() { Limit = limit };

                OrderResult result = await designerService.GetOrdersAsync("finished", User.GetId(), search, pagination);

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
        ///     Updates the specified Cad with the specified Status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">Must be Begin, Report or Cancel</param>
        /// <returns></returns>
        [HttpPatch("Orders/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> UpdateOrderStatusAsync(int id, string status)
        {
            try
            {
                switch (status.ToLower())
                {
                    case "begin": await designerService.BeginAsync(id, User.GetId()); break;
                    case "report": await designerService.ReportAsync(id); break;
                    case "cancel": await designerService.CancelAsync(id, User.GetId()); break;
                    default: return BadRequest(string.Format(InvalidStatus, string.Join(", ", Enum.GetNames<CadStatus>())));
                }
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
        ///     Updates the specified Order's Status as Finished and sets its CadId to the specified.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cadId"></param>
        /// <returns></returns>
        [HttpPatch("Orders/{id}/Finish")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> CompleteOrderAsync(int id, int cadId)
        {
            try
            {
                await designerService.CompleteAsync(id, cadId, User.GetId()).ConfigureAwait(false);
                return NoContent();
            }
            catch (KeyNotFoundException ex) 
            {
                return NotFound(ex.GetMessage());
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(ForbiddenAccess);
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
