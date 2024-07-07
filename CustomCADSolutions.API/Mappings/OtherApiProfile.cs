using AutoMapper;
using CustomCADSolutions.API.Models.Others;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.OpenApi.Extensions;

namespace CustomCADSolutions.API.Mappings
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
