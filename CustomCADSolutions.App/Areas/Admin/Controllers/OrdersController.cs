using AutoMapper;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Mappings;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Models.Orders;
using Microsoft.AspNetCore.Authorization;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Administrator")]
    public class OrdersController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public OrdersController(ILogger<OrdersController> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<OrderDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string _;
            try
            {
                _ = CategoriesAPIPath;
                ViewBag.Categories = (await httpClient.GetFromJsonAsync<Category[]>(_))!;

                _ = OrdersAPIPath;
                var orders = (await httpClient.GetFromJsonAsync<OrderExportDTO[]>(_))!;

                return View(mapper.Map<OrderViewModel[]>(orders));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string buyerId)
        {
            string _;
            try
            {
                _ = $"{OrdersAPIPath}/{buyerId}/{id}";
                var response = await httpClient.DeleteAsync(_);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }
    }
}
