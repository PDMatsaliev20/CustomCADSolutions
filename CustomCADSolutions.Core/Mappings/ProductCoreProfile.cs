using AutoMapper;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Mappings
{
    public class ProductCoreProfile : Profile
    {
        public ProductCoreProfile()
        {
            EntityToModel();
            ModelToEntity();
        }

        /// <summary>
        ///     Converts Entity to Service Model
        /// </summary>
        public void EntityToModel() => CreateMap<Product, ProductModel>();

        /// <summary>
        ///     Converts Service to Entity Model
        /// </summary>
        public void ModelToEntity() => CreateMap<ProductModel, Product>();
    }
}
