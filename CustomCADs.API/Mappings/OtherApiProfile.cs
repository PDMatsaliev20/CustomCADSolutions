using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Core.Models;
using CustomCADs.Infrastructure.Data.Models.Enums;
using CustomCADs.Infrastructure.Data.Models.Identity;

namespace CustomCADs.API.Mappings
{
    public class OtherApiProfile : Profile
    {
        public OtherApiProfile()
        {
            CategoryToDTO();
            RoleToDTO();
            UserToDTO();
            SortingToDTO();
        }
        private void CategoryToDTO() => CreateMap<CategoryModel, CategoryDTO>();

        private void RoleToDTO() => CreateMap<AppRole, RoleDTO>();
        
        private void UserToDTO() => CreateMap<AppUser, UserDTO>()
            .ForMember(dto => dto.Username, opt => opt.MapFrom(user => user.UserName))
            .ForMember(dto => dto.Role, opt => opt.Ignore());
        
        private void SortingToDTO() => CreateMap<CadSorting, CadSortingDTO>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(sort => sort.ToString()))
            .ForMember(dto => dto.Value, opt => opt.MapFrom(sort => sort));
    }
}
