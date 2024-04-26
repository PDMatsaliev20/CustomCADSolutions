using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using AutoMapper;
using System.Drawing;

namespace CustomCADSolutions.Core.Mappings
{
    public class CadCoreProfile : Profile
    {
        public CadCoreProfile()
        {
            EntityToModel();
            ModelToExport();

            QueryToDTO();
            DTOToQuery();

            ImportToModel();
            ModelToEntity();
        }

        /// <summary>
        ///     Converts Entity to Service Model
        /// </summary>

        public void EntityToModel() => CreateMap<Cad, CadModel>()
                .ForMember(model => model.Coords, opt =>
                    opt.MapFrom(entity => new int[] { entity.X, entity.Y, entity.Z }))
                .ForMember(model => model.Color, opt =>
                    opt.MapFrom(entity => Color.FromArgb(entity.R, entity.G, entity.B)));

        /// <summary>
        ///     Converts Service Model to DTO
        /// </summary>
        public void ModelToExport() => CreateMap<CadModel, CadExportDTO>()
            .ForMember(export => export.RGB, opt => 
                opt.MapFrom(model => new[] { model.Color.R, model.Color.G, model.Color.B }))
            .ForMember(export => export.CreationDate, opt => 
                opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(export => export.CreatorName, opt => 
                opt.MapFrom(model => model.Creator.UserName))
            .ForMember(export => export.CategoryName, opt => 
                opt.MapFrom(model => model.Category.Name));

        /// <summary>
        ///     Converts Query to DTO
        /// </summary>
        public void QueryToDTO() => CreateMap<CadQueryModel, CadQueryDTO>();

        /// <summary>
        ///     Converts DTO to Query
        /// </summary>
        public void DTOToQuery() => CreateMap<CadQueryDTO, CadQueryModel>();

        /// <summary>
        /// Converts DTO to Service Model
        /// </summary>
        public void ImportToModel() => CreateMap<CadImportDTO, CadModel>()
            .ForMember(entity => entity.Color, opt => 
                opt.MapFrom(import => Color.FromArgb(import.RGB[0], import.RGB[1], import.RGB[2])));

        /// <summary>
        ///     Converts Service Model to Entity
        /// </summary>
        public void ModelToEntity() => CreateMap<CadModel, Cad>()
                .ForMember(entity => entity.X, opt => opt.MapFrom(model => model.Coords[0]))
                .ForMember(entity => entity.Y, opt => opt.MapFrom(model => model.Coords[1]))
                .ForMember(entity => entity.Z, opt => opt.MapFrom(model => model.Coords[2]))
                .ForMember(entity => entity.R, opt => opt.MapFrom(model => model.Color.R))
                .ForMember(entity => entity.G, opt => opt.MapFrom(model => model.Color.G))
                .ForMember(entity => entity.B, opt => opt.MapFrom(model => model.Color.B));
    }
}
