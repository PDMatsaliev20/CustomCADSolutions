using AutoMapper;
using CustomCADSolutions.API.Models.Cads;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.API.Mappings
{
    public class CadApiProfile : Profile
    {
        public CadApiProfile()
        {
            ModelToExport();
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
        /// Converts DTO to Service Model
        /// </summary>
        public void ImportToModel() => CreateMap<CadImportDTO, CadModel>();

    }
}
