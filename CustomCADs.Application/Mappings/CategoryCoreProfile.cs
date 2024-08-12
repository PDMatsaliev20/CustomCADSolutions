using AutoMapper;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Core.Mappings
{
    public class CategoryCoreProfile : Profile
    {
        public CategoryCoreProfile()
        {
            EntityToModel();
            ModelToEntity();
        }

        public void EntityToModel() => CreateMap<Category, CategoryModel>();
        
        public void ModelToEntity() => CreateMap<CategoryModel, Category>();
    }
}
