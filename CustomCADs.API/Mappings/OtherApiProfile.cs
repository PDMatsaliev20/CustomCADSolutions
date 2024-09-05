using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Application.Models.Categories;
namespace CustomCADs.API.Mappings
{
    public class OtherApiProfile : Profile
    {
        public OtherApiProfile()
        {
            CategoryToDTO();
            DTOToCategory();
        }

        private void CategoryToDTO() => CreateMap<CategoryModel, CategoryDTO>();
        private void DTOToCategory() => CreateMap<CategoryDTO, CategoryModel>();
    }
}
