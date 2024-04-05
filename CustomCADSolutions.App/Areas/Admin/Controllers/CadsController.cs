using AutoMapper;
using CustomCADSolutions.App.Extensions;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.App.Mappings;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADSolutions.App.Extensions.UtilityExtensions;
using static CustomCADSolutions.App.Constants.Paths;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CadsController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public CadsController(ILogger<CadModel> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            MapperConfiguration config = new(cfg => cfg.AddProfile<CadDTOProfile>());
            this.mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel inputQuery)
        {
            Dictionary<string, string> parameters = new()
            {
                ["validated"] = "true",
                ["unvalidated"] = "true",
            };
            string path = CadsAPIPath + HttpContext.SecureQuery(parameters.ToArray());
            var query = await httpClient.GetFromJsonAsync<CadQueryDTO>(path);
            if (query != null)
            {
                var categories = await httpClient.GetFromJsonAsync<Category[]>(CategoriesAPIPath);
                if (categories != null)
                {
                    inputQuery.Categories = categories.Select(c => c.Name);
                    inputQuery.TotalCount = query.TotalCount;
                    inputQuery.Cads = mapper.Map<CadViewModel[]>(query.Cads);

                    ViewBag.Sortings = typeof(CadSorting).GetEnumNames();
                    return View(inputQuery);
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await httpClient.DeleteAsync($"{CadsAPIPath}/{id}");
            response.EnsureSuccessStatusCode();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
