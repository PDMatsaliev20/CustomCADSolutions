﻿using CustomCADSolutions.App.Models.Orders;
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
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.App.Mappings.CadDTOs;

namespace CustomCADSolutions.App.Areas.Designer.Controllers
{
    [Area("Designer")]
    [Authorize(Roles = "Designer")]
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
        public async Task<IActionResult> All()
        {
            ViewBag.Statuses = typeof(OrderStatus).GetEnumNames();

            try
            {
                var dtos = (await httpClient.GetFromJsonAsync<OrderExportDTO[]>(OrdersAPIPath))!;
                ViewBag.HiddenOrders = dtos.Count(m => !m.ShouldShow);
                var wantedOrders = dtos.Where(v => v.ShouldShow);

                var orders = mapper.Map<OrderViewModel[]>(wantedOrders);
                return View(orders);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Begin(int id)
        {
            string _;
            try
            {
                _ = $"{OrdersAPIPath}/{id}";
                var exportDTO = (await httpClient.GetFromJsonAsync<OrderExportDTO>(_))!;

                OrderImportDTO importDTO = new()
                {
                    Id = exportDTO.Id,
                    Status = OrderStatus.Begun.ToString(),
                    Cad = new()
                    {
                        Name = exportDTO.Cad.Name,
                        CategoryId = exportDTO.Cad.CategoryId,
                    },
                };
                
                _ = $"{OrdersAPIPath}/{id}";
                var response = await httpClient.PutAsJsonAsync(_, importDTO);

                return RedirectToAction(nameof(All));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Finish(int id)
        {
            string _;
            try
            {
                _ = $"{OrdersAPIPath}/{id}";
                var dto = (await httpClient.GetFromJsonAsync<OrderExportDTO>(_))!;

                CadFinishModel input = new()
                {
                    OrderId = id,
                    Description = dto.Description,
                    Categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath)
                };
                return View(input);
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id, CadFinishModel cad)
        {
            string _;
            try
            {
                CadImportDTO dto = new()
                {
                    Name = cad.Name,
                    CategoryId = cad.CategoryId,
                    Bytes = await cad.CadFile.GetBytesAsync(),
                    CreatorId = User.GetId(),
                    IsValidated = true,
                };

                _ = $"{OrdersAPIPath}/Finish/{id}";
                var response = await httpClient.PutAsJsonAsync(_, dto);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(All));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Hide(int id)
        {
            string _;
            try
            {
                _ = $"{OrdersAPIPath}/{id}";
                var exportDTO = (await httpClient.GetFromJsonAsync<OrderExportDTO>(_))!;

                OrderImportDTO importDTO = new()
                {
                    Id = exportDTO.Id,
                    Status = exportDTO.Status,
                    Cad = new()
                    {
                        Name = exportDTO.Cad.Name,
                        CategoryId = exportDTO.Cad.CategoryId,
                    },
                    ShouldShow = exportDTO.ShouldShow,
                };

                _ = $"{OrdersAPIPath}/{id}";
                var response = await httpClient.PutAsJsonAsync(_, importDTO);

                return RedirectToAction(nameof(All));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
