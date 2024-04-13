using AutoMapper;
using CustomCADSolutions.Core.Mappings.DTOs;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            DTOToModel();
            ModelToDTO();
            ModelToEntity();
            EntityToModel();
        }

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

        public IMappingExpression<Order, OrderModel> EntityToModel() => CreateMap<Order, OrderModel>()
            .ForMember(model => model.CadId, opt => opt.AllowNull())
            .ForMember(model => model.Cad, opt => opt.AllowNull())
            .ForMember(model => model.Category, opt => opt.MapFrom(entity => entity.Category));

        public IMappingExpression<OrderModel, Order> ModelToEntity() => CreateMap<OrderModel, Order>()
            .ForMember(entity => entity.CadId, opt => opt.AllowNull())
            .ForMember(entity => entity.Cad, opt => opt.AllowNull());
    }
}