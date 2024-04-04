using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using CustomCADSolutions.App.Extensions;
using Microsoft.Extensions.Options;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using System.Text.Json;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Mappings.CadDTOs;

namespace CustomCADSolutions.App.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "Client")]
    public class OrdersController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly StripeSettings stripeSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrdersController(
            ILogger<OrdersController> logger,
            ICategoryService categoryService,
            IOptions<StripeSettings> stripeSettings,
            HttpClient httpClient)
        {
            this.categoryService = categoryService;
            this.logger = logger;
            this.httpClient = httpClient;
            this.stripeSettings = stripeSettings.Value;
            MapperConfiguration config = new(cfg =>
            {
                cfg.AddProfile<OrderDTOProfile>();
                cfg.AddProfile<CadDTOProfile>();
            });
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dtos = await httpClient.TryGetFromJsonAsync<IEnumerable<OrderExportDTO>>(OrdersAPIPath);
                if (dtos != null)
                {
                    dtos = dtos.Where(v => v.BuyerName == User.Identity!.Name);

                    ViewBag.Area = "Client";
                    return View(mapper.Map<OrderViewModel[]>(dtos));
                }
                else throw new JsonException("Json parsing error");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string path = $"{OrdersAPIPath}/{User.GetId()}/{id}";
            OrderExportDTO? dto = await httpClient.TryGetFromJsonAsync<OrderExportDTO>(path);

            if (dto != null)
            {
                return View(mapper.Map<OrderViewModel>(dto));
            }
            else return BadRequest("Json parsing error");
        }

        [HttpGet]
        public async Task<IActionResult> Order(int id)
        {
            var dto = await httpClient.TryGetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
            if (dto != null && id != 1)
            {
                CadViewModel view = mapper.Map<CadViewModel>(dto);
                ViewBag.StripeKey = stripeSettings.TestPublishableKey;
                return View(view);
            }
            else return BadRequest("Model not found");
        }

        [HttpPost]
        public async Task<IActionResult> Order(int id, string stripeToken)
        {
            string cadPath = $"{CadsAPIPath}/{id}";
            var cadDto = await httpClient.TryGetFromJsonAsync<CadExportDTO>(cadPath);

            if (cadDto != null)
            {
                string orderPath = $"{OrdersAPIPath}/{User.GetId()}/{id}";
                try
                {
                    await httpClient.TryGetFromJsonAsync<OrderExportDTO>(orderPath);
                }
                catch
                {
                    bool paymentSuccessful = stripeSettings.ProcessPayment(stripeToken);
                    if (!paymentSuccessful)
                    {
                        TempData["ErrorMessage"] = "Payment failed. Please try again.";
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    OrderImportDTO dto = new()
                    {
                        CadId = id,
                        BuyerId = User.GetId(),
                        Description = $"3D Model from the gallery with id: {id}",
                        Status = OrderStatus.Finished.ToString(),
                    };
                    await httpClient.PostAsJsonAsync($"{OrdersAPIPath}/Create", dto);

                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            else return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new OrderInputModel()
            {
                Categories = await categoryService.GetAllAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            try
            {
                input.BuyerId = User.GetId();
                var dto = mapper.Map<OrderImportDTO>(input);
                var response = await httpClient.PostAsJsonAsync($"{OrdersAPIPath}/Create", dto);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int cadId)
        {
            string path = $"{OrdersAPIPath}/{User.GetId()}/{cadId}";
            var dto = await httpClient.TryGetFromJsonAsync<OrderExportDTO>(path);
            if (dto != null)
            {
                if (dto.Status != OrderStatus.Pending.ToString())
                {
                    return RedirectToAction(nameof(Index));
                }

                OrderInputModel input = new()
                {
                    CadId = dto.CadId,
                    BuyerId = dto.BuyerId,
                    Name = dto.Cad.Name,
                    Description = dto.Description,
                    CategoryId = (await categoryService.GetByNameAsync(dto.Cad.CategoryName)).Id,
                    Categories = await categoryService.GetAllAsync(),
                };

                return View(input);
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int cadId, string buyerId, OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                logger.LogError($"Invalid Order - {string.Join(", ", ModelState.GetErrors())}");
                input.Categories = await categoryService.GetAllAsync();
                return View(input);
            }

            if (User.GetId() != buyerId)
            {
                return Forbid();
            }

            if (input.Status != OrderStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = mapper.Map<OrderImportDTO>(input);
            var response = await httpClient.PutAsJsonAsync($"{OrdersAPIPath}/Edit", dto);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string buyerId, int id)
        {
            string path = string.Format($"{OrdersAPIPath}/{buyerId}/{id}");
            var response = await httpClient.DeleteAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else return StatusCode((int)response.StatusCode);
        }
    }
}
