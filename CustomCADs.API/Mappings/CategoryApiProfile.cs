using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Application.Models.Categories;
namespace CustomCADs.API.Mappings
{
    public class CategoryApiProfile : Profile
    {
        public CategoryApiProfile()
        {
            CategoryToDTO();
            DTOToCategory();
        }

        private void CategoryToDTO() => CreateMap<CategoryModel, CategoryDTO>();
        private void DTOToCategory() => CreateMap<CategoryDTO, CategoryModel>();
    }
}
