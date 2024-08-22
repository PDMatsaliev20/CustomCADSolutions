using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Domain.Identity;

namespace CustomCADs.API.Mappings
{
    public class IdentityApiProfile : Profile
    {
        public IdentityApiProfile()
        {
            RoleToDTO();
            UserToDTO();
        }

        private void RoleToDTO() => CreateMap<AppRole, RoleDTO>();

        private void UserToDTO() => CreateMap<AppUser, UserDTO>()
            .ForMember(dto => dto.Username, opt => opt.MapFrom(user => user.UserName))
            .ForMember(dto => dto.Role, opt => opt.Ignore());
    }
}
