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
            => mapper.Map<CadExportDTO>(await cadService.GetByIdAsync(253));
        

        [HttpGet("Gallery")]
        public async Task<ActionResult<CadQueryResult>> GetAllAsync([FromQuery] CadQueryModel query)
            => await cadService.GetAllAsync(query);        
    }
}
