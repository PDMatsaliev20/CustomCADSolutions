using AutoMapper;
using CustomCADSolutions.API.Models.CadDTOs;
using CustomCADSolutions.Core.Mappings.CadDTOs;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.API.Mappings
{
    public class CadApiProfile : Profile
    {
        public CadApiProfile()
        {
            ModelToExport();
            QueryToDTO();
            DTOToQuery();
            ImportToModel();
        }

        /// <summary>
        ///     Converts Service Model to DTO
        /// </summary>
        public void ModelToExport() => CreateMap<CadModel, CadExportDTO>()
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
        public void ImportToModel() => CreateMap<CadImportDTO, CadModel>();

    }
}
