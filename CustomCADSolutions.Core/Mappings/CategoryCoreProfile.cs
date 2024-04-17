using AutoMapper;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Mappings
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
