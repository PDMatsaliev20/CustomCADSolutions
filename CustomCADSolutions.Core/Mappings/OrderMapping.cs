using AutoMapper;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using System.Drawing;

namespace CustomCADSolutions.Core.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            ModelToEntity();
            EntityToModel();
        }

        public IMappingExpression<Order, OrderModel> ModelToEntity() => CreateMap<Order, OrderModel>()
                .ForMember(m => m.CadId, opt => opt.MapFrom(o => o.CadId))
                .ForMember(m => m.BuyerId, opt => opt.MapFrom(o => o.BuyerId))
                .ForMember(m => m.Description, opt => opt.MapFrom(o => o.Description))
                .ForMember(m => m.OrderDate, opt => opt.MapFrom(o => o.OrderDate))
                .ForMember(m => m.Status, opt => opt.MapFrom(o => o.Status))
                .ForMember(m => m.ShouldShow, opt => opt.MapFrom(o => o.ShouldShow))
                .ForMember(m => m.Buyer, opt => opt.MapFrom(o => o.Buyer))
                .ForMember(m => m.Cad, opt => opt.MapFrom(o => o.Cad));

        public IMappingExpression<OrderModel, Order> EntityToModel() => CreateMap<OrderModel, Order>()
                .ForMember(o => o.CadId, opt => opt.MapFrom(m => m.CadId))
                .ForMember(o => o.BuyerId, opt => opt.MapFrom(m => m.BuyerId))
                .ForMember(o => o.Description, opt => opt.MapFrom(m => m.Description))
                .ForMember(o => o.OrderDate, opt => opt.MapFrom(m => m.OrderDate))
                .ForMember(o => o.Status, opt => opt.MapFrom(m => m.Status))
                .ForMember(o => o.ShouldShow, opt => opt.MapFrom(m => m.ShouldShow))
                .ForMember(o => o.Buyer, opt => opt.MapFrom(m => m.Buyer))
                .ForMember(o => o.Cad, opt => opt.MapFrom(m => m.Cad));
    }
}