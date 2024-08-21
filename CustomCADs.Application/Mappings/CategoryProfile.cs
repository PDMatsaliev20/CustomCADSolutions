using AutoMapper;
using CustomCADs.Application.Models;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            EntityToModel();
            ModelToEntity();
        }

        public void EntityToModel() => CreateMap<Category, CategoryModel>();
        
        public void ModelToEntity() => CreateMap<CategoryModel, Category>();
    }
}
