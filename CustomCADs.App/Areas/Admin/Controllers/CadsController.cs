using AutoMapper;
using CustomCADs.App.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomCADs.App.Models.Cads.View;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using static CustomCADs.Infrastructure.Data.DataConstants;

namespace CustomCADs.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class CadsController(
        ICadService cadService,
        ICategoryService categoryService,
        ILogger<CadsController> logger
            ) : Controller
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg 
                => cfg.AddProfile<CadAppProfile>())
            .CreateMapper();

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CadQueryInputModel query)
        {
            if (query.CadsPerPage % query.Cols != 0)
            {
                query.CadsPerPage = query.Cols * (query.CadsPerPage / query.Cols);
            }

            CadQueryResult result = await cadService.GetAllAsync(new()
            {
                Category = query.Category,
                SearchName = query.SearchName,
                Sorting = query.Sorting,
                CurrentPage = query.CurrentPage,
                CadsPerPage = query.CadsPerPage,
                Validated = true,
                Unvalidated = true,
            });
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
