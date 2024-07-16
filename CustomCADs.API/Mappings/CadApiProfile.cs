using AutoMapper;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.API.Mappings
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
                opt.MapFrom(model => model.Creator.UserName));

        public void DTOToQuery() => CreateMap<CadQueryDTO, CadQueryModel>()
            .ForMember(model => model.Sorting, opt => opt.MapFrom(dto => Enum.Parse<CadSorting>(dto.Sorting ?? ((CadSorting)1).ToString())));

        public void QueryResultToDTO() => CreateMap<CadQueryResult, CadQueryResultDTO>();

        /// <summary>
        /// Converts DTO to Service Model
        /// </summary>
        public void ImportToModel() => CreateMap<CadImportDTO, CadModel>();

    }
}
