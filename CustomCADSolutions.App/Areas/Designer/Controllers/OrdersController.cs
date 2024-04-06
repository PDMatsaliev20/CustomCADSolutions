using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings.DTOs;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using System.Text.Json;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrdersController(
            IOrderService orderService,
            ILogger<OrdersController> logger,
            HttpClient httpClient)
        {
            this.orderService = orderService;
            this.logger = logger;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<OrderDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ViewBag.Statuses = typeof(OrderStatus).GetEnumNames();

            var dtos = await httpClient.GetFromJsonAsync<OrderExportDTO[]>(OrdersAPIPath)
                ?? throw new JsonException("parsing error");

            ViewBag.HiddenOrders = dtos.Count(m => !m.ShouldShow);
            return View(mapper.Map<OrderViewModel[]>(dtos.Where(v => v.ShouldShow)));
        }

        [HttpPost]
        public async Task<IActionResult> Begin(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);
            if (model == null)
            {
                return BadRequest();
            }

            model.Status = OrderStatus.Begun;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Finish(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);

            if (model == null)
            {
                return BadRequest();
            }

            byte[] bytes = await input.CadFile.GetBytesAsync();

            model.Cad.Bytes = bytes;
            model.Cad.CreatorId = User.GetId();
            model.Cad.CreationDate = DateTime.Now;
            model.Cad.IsValidated = true;
            model.Status = OrderStatus.Finished;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Hide(CadInputModel input)
        {
            OrderModel? model = await orderService.GetByIdAsync(input.Id, input.BuyerId!);

            if (model == null)
            {
                return BadRequest();
            }

            model.ShouldShow = false;
            await orderService.EditAsync(model);

            return RedirectToAction(nameof(All));
        }
    }
}
