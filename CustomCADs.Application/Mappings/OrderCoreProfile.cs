﻿using AutoMapper;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Core.Mappings
{
    public class OrderCoreProfile : Profile
    {
        public OrderCoreProfile()
        {
            EntityToModel();
            ModelToEntity();
        }

        /// <summary>
        ///     Converts Entity to Service Model
        /// </summary>
        public void EntityToModel() => CreateMap<Order, OrderModel>()
            .ForMember(model => model.CadId, opt => opt.AllowNull())
            .ForMember(model => model.Cad, opt => opt.AllowNull())
            .ForMember(model => model.Category, opt => opt.MapFrom(entity => entity.Category));

        /// <summary>
        ///     Converts Service Model to Entity
        /// </summary>
        public void ModelToEntity() => CreateMap<OrderModel, Order>()
            .ForMember(entity => entity.CadId, opt => opt.AllowNull())
            .ForMember(entity => entity.Cad, opt => opt.AllowNull());
    }
}