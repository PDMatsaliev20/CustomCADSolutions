using AutoMapper;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Core.Mappings
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
