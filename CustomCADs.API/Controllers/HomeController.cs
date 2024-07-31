using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

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
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadQueryResultDTO>> GetAllAsync([FromQuery] CadQueryDTO dto)
        {
            try
            {
                CadQueryModel query = mapper.Map<CadQueryModel>(dto);
                query.Status = CadStatus.Validated;

                CadQueryResult result = await cadService.GetAllAsync(query);
                return mapper.Map<CadQueryResultDTO>(result);
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.GetMessage());
            }
        }

        [HttpGet("Gallery/{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadGetDTO>> GetCadAsync(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id);
                return mapper.Map<CadGetDTO>(model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.GetMessage());
            }
            catch (Exception ex) when (
                ex is AutoMapperConfigurationException
                || ex is AutoMapperMappingException
            )
            {
                return StatusCode(Status502BadGateway, ex.GetMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(Status500InternalServerError, ex.GetMessage());
            }
        }
    }
}
