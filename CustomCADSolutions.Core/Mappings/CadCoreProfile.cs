using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using AutoMapper;

namespace CustomCADSolutions.Core.Mappings
{
    public class CadCoreProfile : Profile
    {
        public CadCoreProfile()
        {
            EntityToModel();
            ModelToEntity();
        }

        /// <summary>
        ///     Converts Entity to Service Model
        /// </summary>

        public void EntityToModel() => CreateMap<Cad, CadModel>()
                .ForMember(model => model.Coords, opt =>
                    opt.MapFrom(entity => new int[] { entity.X, entity.Y, entity.Z }));
        
        /// <summary>
        ///     Converts Service Model to Entity
        /// </summary>
        public void ModelToEntity() => CreateMap<CadModel, Cad>()
                .ForMember(entity => entity.X, opt => opt.MapFrom(model => model.Coords[0]))
                .ForMember(entity => entity.Y, opt => opt.MapFrom(model => model.Coords[1]))
                .ForMember(entity => entity.Z, opt => opt.MapFrom(model => model.Coords[2]));
    }
}
