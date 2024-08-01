using AutoMapper;
using CustomCADs.API.Models.Orders;
using CustomCADs.Core.Models.Orders;

namespace CustomCADs.API.Mappings
{
    public class OrderApiProfile : Profile
    {
        public OrderApiProfile()
        {
            ModelToExport();
            ImportToModel();
        }

        /// <summary>
        ///     Converts Service Model to Export DTO
        /// </summary>
        public void ModelToExport() => CreateMap<OrderModel, OrderExportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.AllowNull())
            .ForMember(dto => dto.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(dto => dto.Category, opt => opt.MapFrom(model => model.Category.Name))
            ;

        /// <summary>
        ///     Converts Import DTO to Service Model
        /// </summary>
        public void ImportToModel() => CreateMap<OrderImportDTO, OrderModel>();
    }
}
