using AutoMapper;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Models.Orders;

namespace CustomCADs.API.Mappings
{
    public class OrderApiProfile : Profile
    {
        public OrderApiProfile()
        {
            ResultToDTO();
            ModelToExport();
        }

        private void ResultToDTO() => CreateMap<OrderResult, OrderResultDTO>();

        public void ModelToExport() => CreateMap<OrderModel, OrderExportDTO>()
            .ForMember(dto => dto.CadId, opt => opt.AllowNull())
            .ForMember(dto => dto.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(view => view.DesignerName, opt =>
                {
                    opt.AllowNull();
                    opt.MapFrom(model => model.Designer != null ? model.Designer.UserName : null);
                })
            .ForMember(view => view.DesignerEmail, opt =>
                {
                    opt.AllowNull();
                    opt.MapFrom(model => model.Designer != null ? model.Designer.Email : null);
                })
            .ForMember(dto => dto.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(dto => dto.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")));
    }
}
