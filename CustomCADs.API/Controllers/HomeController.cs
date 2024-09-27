using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    /// <summary>
    ///     Controller for public data.
    /// </summary>
    /// <param name="cadService"></param>
    /// <param name="mapper"></param>
    [ApiController]
    [Route("API/[controller]")]
    public class HomeController(ICadService cadService, IMapper mapper) : ControllerBase
    {
        /// <summary>
        ///     Gets the path and coordinates to the 3D Model for the Home Page.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Cad")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<CadGetDTO> GetHomeCadAsync()
            => new CadGetDTO()
            {
                CadPath = "/files/HomeCAD.glb",
                CamCoordinates = new() { X = 2, Y = 16, Z = 33 },
                PanCoordinates = new() { X = 0, Y = 6, Z = -3 },
            };

        /// <summary>
        ///     Queries all Validated 3D Models with the specified parameters.
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <param name="creator"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet("Gallery")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public ActionResult<CadQueryResultDTO> GetGalleryAsync(string? sorting, string? category, string? name, string? creator, int page = 1, int limit = 20)
        {
            try
            {
                CadResult result = cadService.GetAllAsync(
                    status: nameof(CadStatus.Validated),
                    category: category,
                    name:  name,
                    owner: creator,
                    sorting: sorting ?? string.Empty,
                    page: page,
                    limit: limit
                    );

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

        /// <summary>
        ///     Get info about a 3D Model from the Gallery.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Gallery/{id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status500InternalServerError)]
        [ProducesResponseType(Status502BadGateway)]
        public async Task<ActionResult<CadGetDTO>> GetGalleryCadAsync(int id)
        {
            try
            {
                CadModel model = await cadService.GetByIdAsync(id).ConfigureAwait(false);
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

        /// <summary>
        ///     Gets all existing Sortings.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Sortings")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<string[]> GetSortingsAsync()
            => Enum.GetNames<Sorting>();
    }
}
