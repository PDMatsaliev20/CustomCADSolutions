using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("API")]
    public class ApiController : ControllerBase
    {
        private readonly ICadService cadService;
        private readonly IOrderService orderService;
        public ApiController(ICadService cadService, IOrderService orderService)
        {
            this.cadService = cadService;
            this.orderService = orderService;
        }

        [HttpGet("Cads")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CadViewModel>>> GetCads()
        {
            CadQueryModel query = await cadService.GetAllAsync();
            return query.CadModels
                .Select(c => new CadViewModel() { Id = c.Id, Name = c.Name, Category = c.Category.Name })
                .ToArray();
        }

        [HttpGet("Orders")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrders()
        {
            IEnumerable<OrderModel> orders = await orderService.GetAllAsync();
            return orders.ToArray();
        }
    }
}
