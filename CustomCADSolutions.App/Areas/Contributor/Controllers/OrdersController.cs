using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Extensions;
using System.Text.Json;
using AutoMapper;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.App.Mappings.DTOs;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Models.Cads;
using Microsoft.Extensions.Options;

namespace CustomCADSolutions.App.Areas.Contributor.Controllers
{
    [Area("Contributor")]
    [Authorize(Roles = "Contributor")]
    public class OrdersController : Controller
    {
        private readonly StripeSettings stripeSettings;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrdersController(
            IOptions<StripeSettings> stripeSettings,
            ILogger<OrdersController> logger,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.stripeSettings = stripeSettings.Value;
            MapperConfiguration config = new(cfg => cfg.AddProfile<OrderDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dtos = (await httpClient.GetFromJsonAsync<OrderExportDTO[]>(OrdersAPIPath))!
                    .Where(v => v.BuyerName == User.Identity!.Name).ToArray();

                ViewBag.Area = "Contributor";
                return View(mapper.Map<OrderViewModel[]>(dtos));
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string _ = $"{OrdersAPIPath}/{User.GetId()}/{id}";
            try
            {
                var dto = await httpClient.GetFromJsonAsync<OrderExportDTO>(_);
                return View(mapper.Map<OrderViewModel>(dto));
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Order(int id)
        {
            var dto = await httpClient.GetFromJsonAsync<CadExportDTO>($"{CadsAPIPath}/{id}");
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
            try
            {
                string _ = $"{CadsAPIPath}/{id}";
                var cadDto = await httpClient.GetFromJsonAsync<CadExportDTO>(_);
                
                try 
                {
                    _ = $"{OrdersAPIPath}/{User.GetId()}/{id}";
                    await httpClient.GetFromJsonAsync<OrderExportDTO>(_);
                    return RedirectToAction(nameof(Details), new { id });
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
                    await httpClient.PostAsJsonAsync(OrdersAPIPath, dto);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new OrderInputModel()
            {
                Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderInputModel input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    input.Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                    return View(input);
                }

                input.BuyerId = User.GetId();
                var dto = mapper.Map<OrderImportDTO>(input);
                var response = await httpClient.PostAsJsonAsync(OrdersAPIPath, dto);
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
            string _;
            try
            {
                _ = $"{OrdersAPIPath}/{User.GetId()}/{cadId}";
                var dto = (await httpClient.GetFromJsonAsync<OrderExportDTO>(_))!;

                if (dto.Status != OrderStatus.Pending.ToString())
                {
                    return RedirectToAction(nameof(Index));
                }

                _ = $"{CategoriesAPIPath}/{dto.Cad.CategoryId}";
                var category = (await httpClient.GetFromJsonAsync<Category>(_))!;

                OrderInputModel input = new()
                {
                    CadId = dto.CadId,
                    BuyerId = dto.BuyerId,
                    Name = dto.Cad.Name,
                    Description = dto.Description,
                    CategoryId = category.Id,
                    Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath),
                };

                return View(input);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int cadId, OrderInputModel input)
        {
            if (!ModelState.IsValid)
            {
                input.Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                return View(input);
            }

            if (input.Status != OrderStatus.Pending)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = mapper.Map<OrderImportDTO>(input);
                var response = await httpClient.PutAsJsonAsync(OrdersAPIPath, dto);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string buyerId)
        {
            string _ = $"{OrdersAPIPath}/{buyerId}/{id}";
            try
            {
                var response = await httpClient.DeleteAsync(_);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
