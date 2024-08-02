using AutoMapper;
using CustomCADs.App.Mappings;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Core.Models.Cads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class CadsController(
        ICadService cadService,
        ILogger<CadsController> logger
            ) : Controller
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg 
                => cfg.AddProfile<CadAppProfile>())
            .CreateMapper();

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel query)
        {
            SearchModel search = new()
            {
                Category = query.Category,
                Name = query.SearchName,
                Sorting = query.Sorting.ToString(),
            };
            PaginationModel pagination = new()
            {
                Page = query.CurrentPage,
                Limit = query.CadsPerPage,
            };
            CadResult result = await cadService.GetAllAsync(new(), search, pagination);
            return View(mapper.Map<CadViewModel[]>(result.Cads));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await cadService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
