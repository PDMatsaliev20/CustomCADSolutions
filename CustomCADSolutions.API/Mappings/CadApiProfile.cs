using AutoMapper;
using CustomCADSolutions.API.Models.Cads;
using CustomCADSolutions.API.Models.Queries;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.API.Mappings
{
    public class CadApiProfile : Profile
    {
        public CadApiProfile()
        {
            QueryResultToDTO();
            DTOToQuery();
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

        public void DTOToQuery() => CreateMap<CadQueryDTO, CadQueryModel>()
            .ForMember(model => model.Sorting, opt => opt.MapFrom(dto => (CadSorting)dto.Sorting.Value));

        public void QueryResultToDTO() => CreateMap<CadQueryResult, CadQueryResultDTO>();

        /// <summary>
        /// Converts DTO to Service Model
        /// </summary>
        public void ImportToModel() => CreateMap<CadImportDTO, CadModel>();

    }
}
