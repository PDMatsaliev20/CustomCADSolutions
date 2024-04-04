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

namespace CustomCADSolutions.App.Areas.Contributer.Controllers
{
    [Area("Contributer")]
    [Authorize(Roles = "Contributer")]
    public class OrdersController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrdersController(
            ICategoryService categoryService,
            ILogger<OrdersController> logger,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.categoryService = categoryService;
            this.httpClient = httpClient;
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<OrderDTOProfile>()).CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var dtos = await httpClient.TryGetFromJsonAsync<OrderExportDTO[]>(OrdersAPIPath);
                if (dtos != null)
                {
                    dtos = dtos.Where(v => v.BuyerName == User.Identity!.Name).ToArray();

                    ViewBag.Area = "Contributer";
                    return View(mapper.Map<OrderViewModel[]>(dtos));
                }
                else throw new JsonException("Json parsing error");

            }
            catch (HttpRequestException ex)
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

            string path = $"{OrdersAPIPath}/{buyerId}/{cadId}";
            var dto = mapper.Map<OrderImportDTO>(input);
            if (dto != null)
            {
                if (dto.Status != OrderStatus.Pending.ToString())
                {
                    return RedirectToAction(nameof(Index));
                }

                var response = await httpClient.PutAsJsonAsync($"{OrdersAPIPath}/Edit", dto);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string buyerId)
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
