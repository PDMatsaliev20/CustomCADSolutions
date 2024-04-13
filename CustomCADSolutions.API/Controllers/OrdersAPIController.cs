using AutoMapper;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.Core.Mappings.DTOs;
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
            MapperConfiguration config = new(cfg =>
            {
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<CadProfile>();
            });
            this.mapper = config.CreateMapper();
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
        public async Task<ActionResult<OrderExportDTO>> GetSingleAsync(int id)
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
        public async Task<ActionResult<OrderExportDTO>> PostAsync(OrderImportDTO dto)
        {
            OrderModel model = mapper.Map<OrderModel>(dto);
            model.OrderDate = DateTime.Now;
            try
            {
                int id = await orderService.CreateAsync(model);

                model = await orderService.GetByIdAsync(id);
                OrderExportDTO export = mapper.Map<OrderExportDTO>(model);

                return CreatedAtAction(null, new { id }, export);
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

                order.Name = dto.Name;
                order.Description = dto.Description;
                order.Status = Enum.Parse<OrderStatus>(dto.Status);
                order.ShouldShow = dto.ShouldShow;
                order.Name = dto.Name;
                order.CategoryId = dto.CategoryId;
                await orderService.EditAsync(id, order);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
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
        [ProducesResponseType(Status204NoContent)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult> FinishAsync(int id, CadImportDTO dto)
        {
            if (!await orderService.ExistsByIdAsync(id))
            {
                return BadRequest();
            }
            OrderModel order = (await orderService.GetByIdAsync(id))!;

            order.Cad = new()
            {
                Name = dto.Name,
                IsValidated = dto.IsValidated,
                Price = dto.Price,
                CreationDate = DateTime.Now,
                CreatorId = dto.CreatorId,
                CategoryId = dto.CategoryId,
                Color = Color.FromArgb(dto.RGB[0], dto.RGB[1], dto.RGB[2]),
            };
            await orderService.FinishOrderAsync(id, order);

            return NoContent();
        }
    }
}
