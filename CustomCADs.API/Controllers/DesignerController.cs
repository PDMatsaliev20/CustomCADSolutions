using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [ApiController]
    [Route("API/[controller]")]
    [Authorize(Designer)]
    public class DesignerController(ICadService cadService, IOrderService orderService) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderApiProfile>();
            cfg.AddProfile<CadApiProfile>();
            cfg.AddProfile<OtherApiProfile>();
        }).CreateMapper();

        [HttpGet("Cads")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetUncheckedCadsAsync([FromQuery] CadQueryDTO dto)
        {
            try
            {
                CadQueryModel query = mapper.Map<CadQueryModel>(dto);
                query.Status = CadStatus.Unchecked;

                CadQueryResult result = await cadService.GetAllAsync(query);
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
                await cadService.EditStatusAsync(id, Enum.Parse<CadStatus>(status));
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

        [HttpGet("Orders")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<OrderExportDTO[]>> GetOrdersByStatusAsync(string status)
        {
            try
            {
                IEnumerable<OrderModel> models = await orderService.GetAllAsync();
                string capitalizedStatus = status.Capitalize();

                IEnumerable<OrderModel> filteredModels;
                if (Enum.Parse<OrderStatus>(capitalizedStatus) == OrderStatus.Finished)
                {
                    filteredModels = models
                        .Where(m => m.Status == OrderStatus.Finished && m.ShouldBeDelivered);
                }
                else
                {
                    filteredModels = models
                        .Where(m => capitalizedStatus.Equals(m.Status.ToString()));
                }

                return mapper.Map<OrderExportDTO[]>(filteredModels);
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

        [HttpPatch("Orders/Status/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> UpdateOrderStatusAsync(int id, string status)
        {
            try
            {
                await orderService.EditStatusAsync(id, Enum.Parse<OrderStatus>(status));
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

        [HttpPatch("Orders/Complete/{id}")]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult> CompleteOrderAsync(int id, int cadId)
        {
            try
            {
                await orderService.CompleteAsync(id, cadId);
                await orderService.EditStatusAsync(id, OrderStatus.Finished);
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
    }
}
