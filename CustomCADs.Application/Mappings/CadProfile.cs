using AutoMapper;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Mappings
{
    public class CadProfile : Profile
    {
        public CadProfile()
        {
            EntityToModel();
            ModelToEntity();
        }

        /// <summary>
        ///     Converts Entity to Service Model
        /// </summary>

        public void EntityToModel() => CreateMap<Cad, CadModel>();
        
        /// <summary>
        ///     Converts Service Model to Entity
        /// </summary>
        public void ModelToEntity() => CreateMap<CadModel, Cad>();
    }
}
