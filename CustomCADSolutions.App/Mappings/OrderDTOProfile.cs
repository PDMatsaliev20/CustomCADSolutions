using AutoMapper;
using CustomCADSolutions.App.Mappings.CadDTOs;
using CustomCADSolutions.App.Mappings.DTOs;
using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Mappings
{
    public class OrderDTOProfile : Profile
    {
        public OrderDTOProfile()
        {
            InputToDTO();
            DTOToModel();
            ModelToDTO();
            DTOToView();
        }

        public IMappingExpression<OrderInputModel, OrderImportDTO> InputToDTO() => CreateMap<OrderInputModel, OrderImportDTO>()
            .ForMember(dto => dto.Status, opt => opt.MapFrom(input => input.Status.ToString()))
            .ForMember(dto => dto.CadId, opt => opt.AllowNull())
            .ForMember(dto => dto.Cad, opt => opt.AllowNull());

        public IMappingExpression<OrderImportDTO, OrderModel> DTOToModel() => CreateMap<OrderImportDTO, OrderModel>()
            .ForMember(model => model.CadId, opt => opt.AllowNull())
            .ForMember(model => model.Cad, opt => opt.AllowNull());

        public IMappingExpression<OrderModel, OrderExportDTO> ModelToDTO() => CreateMap<OrderModel, OrderExportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.AllowNull())
            .ForMember(dto => dto.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(model => model.Category.Name))
            .ForMember(dto => dto.CadId, opt => opt.MapFrom(model => model.CadId))
            ;

        public IMappingExpression<OrderExportDTO, OrderViewModel> DTOToView() => CreateMap<OrderExportDTO, OrderViewModel>()
            .ForMember(view => view.CadId, opt => opt.AllowNull())
            .ForMember(view => view.Name, opt => opt.MapFrom(dto => dto.Name))
            .ForMember(view => view.Category, opt => opt.MapFrom(dto => dto.CategoryName))
            ;
    }
}
