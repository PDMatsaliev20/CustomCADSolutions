using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
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

        [HttpGet("/AccessDenied")]
        [ProducesResponseType(Status403Forbidden)]
        public ActionResult AccessDenied() => StatusCode(Status403Forbidden, "Access Denied");

        [HttpGet("Cad")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<CadGetDTO> GetAsync()
            => new CadGetDTO()
            {
                CadPath = "/others/cads/HomeCAD.glb",
                Coords = [2, 16, 33],
                PanCoords = [0, 6, -3]
            };

        [HttpGet("Gallery")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        public async Task<ActionResult<CadQueryResultDTO>> GetAllAsync([FromQuery] CadQueryDTO dto)
        {
            try
            {
                CadQueryModel query = mapper.Map<CadQueryModel>(dto);
                query.Status = CadStatus.Validated;

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
