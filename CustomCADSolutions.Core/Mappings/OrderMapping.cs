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

        public IMappingExpression<Order, OrderModel> EntityToModel() => CreateMap<Order, OrderModel>()
            .ForMember(model => model.CadId, opt => opt.AllowNull())
            .ForMember(model => model.Cad, opt => opt.AllowNull())
            .ForMember(model => model.Category, opt => opt.MapFrom(entity => entity.Category));

        public IMappingExpression<OrderModel, Order> ModelToEntity() => CreateMap<OrderModel, Order>()
            .ForMember(entity => entity.CadId, opt => opt.AllowNull())
            .ForMember(entity => entity.Cad, opt => opt.AllowNull());
    }
}