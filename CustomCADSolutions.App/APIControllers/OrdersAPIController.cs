using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CustomCADSolutions.App.APIControllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersAPIController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersAPIController(IOrderService orderService)
        {
            this.orderService = orderService;
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<OrderDTOProfile>()).CreateMapper();
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<OrderExportDTO[]>> GetAsync()
        {
            try
            {
                IEnumerable<OrderModel> orders = (await orderService.GetAllAsync())
                    .OrderBy(o => (int)o.Status).ThenBy(o => o.OrderDate);

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
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<OrderExportDTO>> GetAsync(int id)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(id);
                return mapper.Map<OrderExportDTO>(order);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<OrderExportDTO>> PostAsync(OrderImportDTO dto)
        {
            OrderModel model = mapper.Map<OrderModel>(dto);
            model.OrderDate = DateTime.Now;

            try
            {
                int id = await orderService.CreateAsync(model);
                OrderExportDTO export = mapper.Map<OrderExportDTO>(model);
                return CreatedAtAction(nameof(GetAsync), new { id }, export);
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
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> PutAsync(int id, OrderImportDTO dto)
        {
            try
            {
                OrderModel order = await orderService.GetByIdAsync(dto.Id);
                
                order.Description = dto.Description;
                order.Status = Enum.Parse<OrderStatus>(dto.Status);
                order.ShouldShow = dto.ShouldShow;
                order.Cad.Name = dto.Cad.Name;
                order.Cad.CategoryId = dto.Cad.CategoryId;
                await orderService.EditAsync(order);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<OrderModel>> DeleteAsync(int id)
        {
            if (await orderService.ExistsByIdAsync(id))
            {
                await orderService.DeleteAsync(id);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("Finish/{id}")]
        [Consumes("application/json")]
        [IgnoreAntiforgeryToken]
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> FinishAsync(int id, CadImportDTO dto)
        {
            if (!await orderService.ExistsByIdAsync(id))
            {
                return BadRequest();
            }

            CadModel model = (await orderService.GetByIdAsync(id))!.Cad;

            model.Bytes = dto.Bytes;
            model.IsValidated = dto.IsValidated;
            model.CreatorId = dto.CreatorId;
            model.CategoryId = dto.CategoryId;
            model.CreationDate = DateTime.Now;
            model.Name = dto.Name;
            model.Color = Color.FromArgb(dto.RGB[0], dto.RGB[1], dto.RGB[2]);
            await orderService.FinishOrderAsync(id, model);

            return NoContent();
        }
    }
}
