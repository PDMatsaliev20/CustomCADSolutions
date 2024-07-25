using AutoMapper;
using CustomCADs.API.Helpers;
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
            ModelToGet();
            PostToModel();
            PutToModel();
        }

        /// <summary>
        ///     Converts Service Model to Get DTO
        /// </summary>
        public void ModelToGet() => CreateMap<CadModel, CadGetDTO>()
            .ForMember(export => export.CreationDate, opt =>
                opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(export => export.CreatorName, opt =>
                opt.MapFrom(model => model.Creator.UserName));

        public void DTOToQuery() => CreateMap<CadQueryDTO, CadQueryModel>()
            .ForMember(model => model.Sorting, opt => opt.MapFrom(dto => Enum.Parse<CadSorting>(dto.Sorting ?? ((CadSorting)1).ToString())));

        public void QueryResultToDTO() => CreateMap<CadQueryResult, CadQueryResultDTO>();
        
        /// <summary>
        /// Converts Post DTO to Service Model
        /// </summary>
        public void PostToModel() => CreateMap<CadPostDTO, CadModel>()
            .ForMember(model => model.ImageExtension, opt => opt.MapFrom(post => post.Image.GetFileExtension()))
            .ForMember(model => model.CadExtension, opt => opt.MapFrom(post => post.File.GetFileExtension()));


        /// <summary>
        /// Converts Put DTO to Service Model
        /// </summary>
        public void PutToModel() => CreateMap<CadPutDTO, CadModel>();
    }
}
