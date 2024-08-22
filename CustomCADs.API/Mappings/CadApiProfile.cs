using AutoMapper;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Models.Cads;

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
                opt.MapFrom(model => model.Orders.Count))
            .ForMember(export => export.CadPath, opt => 
                opt.MapFrom(model => model.Paths.FilePath))
            .ForMember(export => export.ImagePath, opt => 
                opt.MapFrom(model => model.Paths.ImagePath))
            .ForMember(export => export.Coords, opt => 
                opt.MapFrom(model => new double[3] { model.CamCoordinates.X, model.CamCoordinates.Y, model.CamCoordinates.Z  }))
            .ForMember(export => export.PanCoords, opt => 
                opt.MapFrom(model => new double[3] { model.PanCoordinates.X, model.PanCoordinates.Y, model.PanCoordinates.Z  }))
            ;

        public void QueryResultToDTO() => CreateMap<CadResult, CadQueryResultDTO>();
        
        public void PostToModel() => CreateMap<CadPostDTO, CadModel>();

        public void PutToModel() => CreateMap<CadPutDTO, CadModel>();
    }
}
