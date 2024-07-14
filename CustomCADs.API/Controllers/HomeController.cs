using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CustomCADs.API.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class HomeController(ICadService cadService) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OtherApiProfile>();
            cfg.AddProfile<CadApiProfile>();
        }).CreateMapper();

        [HttpGet("Cad")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<CadExportDTO>> GetAsync()
            => mapper.Map<CadExportDTO>(await cadService.GetByIdAsync(253));


        [HttpGet("Gallery")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<CadQueryResultDTO>> GetAllAsync([FromQuery] CadQueryDTO dto)
        {
            try
            {
                CadQueryModel query = mapper.Map<CadQueryModel>(dto);
                CadQueryResult result = await cadService.GetAllAsync(query);
                return mapper.Map<CadQueryResultDTO>(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
