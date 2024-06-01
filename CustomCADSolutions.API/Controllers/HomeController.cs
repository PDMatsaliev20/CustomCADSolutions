using AutoMapper;
using CustomCADSolutions.API.Mappings;
using CustomCADSolutions.API.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADSolutions.API.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class HomeController(ICadService cadService) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg 
                => cfg.AddProfile<CadApiProfile>())
            .CreateMapper();

        [HttpGet("Cad")]
        public async Task<ActionResult<CadExportDTO>> GetAsync()
        {
            CadModel model = await cadService.GetByIdAsync(253);
            return mapper.Map<CadExportDTO>(model);
        }
    }
}
