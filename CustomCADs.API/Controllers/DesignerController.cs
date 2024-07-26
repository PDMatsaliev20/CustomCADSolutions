using AutoMapper;
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
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<CadQueryResultDTO>> CadsAsync([FromQuery] CadQueryDTO dto)
        {
            CadQueryModel query = mapper.Map<CadQueryModel>(dto);
            query.Status = CadStatus.Unchecked;

            CadQueryResult result = await cadService.GetAllAsync(query);
            return mapper.Map<CadQueryResultDTO>(result);
        }

        [HttpPatch("Cads/Status/{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status204NoContent)]
        public async Task<ActionResult> UpdateCadStatusCadAsync(int id, CadStatus status)
        {
            await cadService.EditStatusAsync(id, status);
            return NoContent();
        }

        [HttpGet("Orders")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<OrderExportDTO[]>> OrdersAsync(string status)
        {
            IEnumerable<OrderModel> models = await orderService.GetAllAsync();
            
            IEnumerable<OrderModel> unfinished = models
                .Where(m => status.Equals(m.Status.ToString(), 
                    StringComparison.CurrentCultureIgnoreCase));

            return mapper.Map<OrderExportDTO[]>(unfinished);
        }

        [HttpPatch("Orders/Status/{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(Status204NoContent)]
        public async Task<ActionResult> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            await orderService.EditStatusAsync(id, status);
            return NoContent();
        }
    }
}
