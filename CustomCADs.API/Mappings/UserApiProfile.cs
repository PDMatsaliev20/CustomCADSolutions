using AutoMapper;
using CustomCADs.API.Models.Users;
using CustomCADs.Application.Models.Users;

namespace CustomCADs.API.Mappings
{
    public class UserApiProfile : Profile
    {
        public UserApiProfile()
        {
            UserToGet();
            PostToUser();
        }

        public void UserToGet() => CreateMap<UserModel, UserGetDTO>()
            .ForMember(get => get.Role, opt => opt.MapFrom(model => model.RoleName));

        public void PostToUser() => CreateMap<UserPostDTO, UserModel>();
    }
}
