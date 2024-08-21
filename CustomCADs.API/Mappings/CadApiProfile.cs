using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.API.Mappings
{
    public class CadApiProfile : Profile
    {
        public CadApiProfile()
        {
            QueryResultToDTO();
            ModelToGet();
            PostToModel();
            PutToModel();
        }

        public void ModelToGet() => CreateMap<CadModel, CadGetDTO>()
            .ForMember(export => export.CreationDate, opt =>
                opt.MapFrom(model => model.CreationDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(export => export.CreatorName, opt =>
                opt.MapFrom(model => model.Creator.UserName))
            .ForMember(export => export.Status, opt => 
                opt.MapFrom(model => model.Status.ToString()))
            .ForMember(export => export.OrdersCount, opt => 
                opt.MapFrom(model => model.Orders.Count));

        public void QueryResultToDTO() => CreateMap<CadResult, CadQueryResultDTO>();
        
        public void PostToModel() => CreateMap<CadPostDTO, CadModel>()
            .ForMember(model => model.ImageExtension, opt => opt.MapFrom(post => post.Image.GetFileExtension()))
            .ForMember(model => model.CadExtension, opt => opt.MapFrom(post => post.File.GetFileExtension()));

        public void PutToModel() => CreateMap<CadPutDTO, CadModel>();
    }
}
