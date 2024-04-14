using CustomCADSolutions.Core.Mappings.DTOs;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using AutoMapper;

namespace CustomCADSolutions.Core.Mappings
{
    public class OrderCoreProfile : Profile
    {
        public OrderCoreProfile()
        {
            EntityToModel();
            ModelToExport();

            ImportToModel();
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
        ///     Converts Service Model to DTO
        /// </summary>
        public void ModelToExport() => CreateMap<OrderModel, OrderExportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.AllowNull())
            .ForMember(dto => dto.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(model => model.Category.Name))
            .ForMember(dto => dto.CadId, opt => opt.MapFrom(model => model.CadId))
            ;

        /// <summary>
        ///     Converts DTO to Service Model
        /// </summary>
        public void ImportToModel() => CreateMap<OrderImportDTO, OrderModel>()
            .ForMember(model => model.CadId, opt => opt.AllowNull())
            .ForMember(model => model.Cad, opt => opt.AllowNull());

        /// <summary>
        ///     Converts Service Model to Entity
        /// </summary>
        public void ModelToEntity() => CreateMap<OrderModel, Order>()
            .ForMember(entity => entity.CadId, opt => opt.AllowNull())
            .ForMember(entity => entity.Cad, opt => opt.AllowNull());
    }
}