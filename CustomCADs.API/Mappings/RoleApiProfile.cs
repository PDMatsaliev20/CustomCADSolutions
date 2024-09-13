using AutoMapper;
using CustomCADs.API.Models.Roles;
using CustomCADs.Application.Models.Roles;

namespace CustomCADs.API.Mappings
{
    public class RoleApiProfile : Profile
    {
        public RoleApiProfile()
        {
            UserToGet();
            PostToUser();
        }

        public void UserToGet() => CreateMap<RoleModel, RoleGetDTO>();
        public void PostToUser() => CreateMap<RolePostDTO, RoleModel>();
    }
}
