using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CustomCADs.API.Controllers
{
    [ApiController]
    [Route("/API/Sortings")]
    public class CadSortingsController : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(opt 
                => opt.AddProfile<OtherApiProfile>())
            .CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<CadSortingDTO[]> GetAsync() => mapper.Map<CadSortingDTO[]>(Enum.GetValues<CadSorting>());

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<CadSortingDTO> GetAsync(int id)
        {
            try
            {
                return mapper.Map<CadSortingDTO>((CadSorting)id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
