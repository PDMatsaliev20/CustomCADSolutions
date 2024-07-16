using AutoMapper;
using CustomCADs.API.Mappings;
using CustomCADs.API.Models.Others;
using CustomCADs.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.API.Controllers
{
    using static StatusCodes;

    [ApiController]
    [Route("API/[controller]")]
    [Authorize(Roles = Admin)]
    public class RolesController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        private readonly IMapper mapper = new MapperConfiguration(opt 
                => opt.AddProfile<OtherApiProfile>())
            .CreateMapper();

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(Status200OK)]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAsync() 
            => mapper.Map<RoleDTO[]>(await roleManager.Roles.ToArrayAsync());

        [HttpGet("{name}")]
        public async Task<ActionResult<RoleDTO>> GetSingleAsync(string name)
        {
            AppRole? role = await roleManager.FindByNameAsync(name);

            return role == null ? NotFound() : mapper.Map<RoleDTO>(role);
        }
    }
}
