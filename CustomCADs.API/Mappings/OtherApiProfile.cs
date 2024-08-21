using AutoMapper;
using CustomCADs.API.Models.Others;
using CustomCADs.Application.Models;
namespace CustomCADs.API.Mappings
{
    public class OtherApiProfile : Profile
    {
        public OtherApiProfile()
        {
            CategoryToDTO();
        }

        private void CategoryToDTO() => CreateMap<CategoryModel, CategoryDTO>();
    }
}
