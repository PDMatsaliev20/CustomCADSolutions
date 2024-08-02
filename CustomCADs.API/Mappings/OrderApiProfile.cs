using AutoMapper;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Core.Models.Orders;

namespace CustomCADs.API.Mappings
{
    public class OrderApiProfile : Profile
    {
        public OrderApiProfile()
        {
            ResultToDTO();
            ModelToExport();
            ImportToModel();
        }
        
        private void ResultToDTO() => CreateMap<OrderResult, OrderResultDTO>();

        public void ModelToExport() => CreateMap<OrderModel, OrderExportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.AllowNull())
            .ForMember(dto => dto.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(view => view.DesignerName, opt => opt.AllowNull())
            .ForMember(view => view.DesignerName, opt => opt.MapFrom(model => model.Designer != null ? model.Designer.UserName : null))
            .ForMember(dto => dto.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(dto => dto.Category, opt => opt.MapFrom(model => model.Category.Name));

        public void ImportToModel() => CreateMap<OrderImportDTO, OrderModel>();
    }
}
