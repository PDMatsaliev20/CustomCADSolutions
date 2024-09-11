using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Domain.Entities;

namespace CustomCADs.API.Mappings
{
    public class IdentityApiProfile : Profile
    {
        public IdentityApiProfile()
        {
            RoleToDTO();
            UserToDTO();
        }

        private void RoleToDTO() => CreateMap<Role, RoleDTO>();

        private void UserToDTO() => CreateMap<User, UserDTO>()
            .ForMember(dto => dto.Username, opt => opt.MapFrom(user => user.UserName))
            .ForMember(dto => dto.Role, opt => opt.Ignore());
    }
}
