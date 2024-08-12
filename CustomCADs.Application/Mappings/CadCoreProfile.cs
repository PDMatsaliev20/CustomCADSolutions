using AutoMapper;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Core.Mappings
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
                    opt.MapFrom(entity => new double[] { entity.X, entity.Y, entity.Z }))
            .ForMember(model => model.PanCoords, opt =>
                    opt.MapFrom(entity => new double[] { entity.PanX, entity.PanY, entity.PanZ }));
        
        /// <summary>
        ///     Converts Service Model to Entity
        /// </summary>
        public void ModelToEntity() => CreateMap<CadModel, Cad>()
                .ForMember(entity => entity.X, opt => opt.MapFrom(model => model.Coords[0]))
                .ForMember(entity => entity.Y, opt => opt.MapFrom(model => model.Coords[1]))
                .ForMember(entity => entity.Z, opt => opt.MapFrom(model => model.Coords[2]));
    }
}
