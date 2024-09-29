using AutoMapper;
using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.ValueObjects;

namespace CustomCADs.API.Mappings
{
    public class CadApiProfile : Profile
    {
        public CadApiProfile()
        {
            CoordinateToDTO();
            QueryResultToDTO();
            ModelToGet();
        }

        public void CoordinateToDTO() => CreateMap<Coordinates, CoordinatesDTO>();

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
                opt.MapFrom(model => model.Paths.ImagePath));

        public void QueryResultToDTO() => CreateMap<CadResult, CadQueryResultDTO>();
    }
}
