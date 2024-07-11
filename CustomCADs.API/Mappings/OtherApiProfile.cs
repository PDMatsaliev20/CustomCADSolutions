using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Core.Models;
using CustomCADs.Infrastructure.Data.Models.Enums;

namespace CustomCADs.API.Mappings
{
    public class OtherApiProfile : Profile
    {
        public OtherApiProfile()
        {
            CategoryToDTO();
            SortingToDTO();
        }

        private void CategoryToDTO() => CreateMap<CategoryModel, CategoryDTO>();
        private void SortingToDTO() => CreateMap<CadSorting, CadSortingDTO>()
            .ForMember(dto => dto.Name, opt => opt.MapFrom(sort => sort.ToString()))
            .ForMember(dto => dto.Value, opt => opt.MapFrom(sort => sort));
    }
}
