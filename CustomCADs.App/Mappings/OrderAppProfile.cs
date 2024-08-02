using AutoMapper;
using CustomCADs.App.Models.Orders;
using CustomCADs.Core.Models.Orders;

namespace CustomCADs.App.Mappings
{
    public class OrderAppProfile : Profile
    {
        public OrderAppProfile()
        {         
            ModelToView();

            AddToModel();
            ModelToAdd();
            
            EditToModel();
            ModelToEdit();
        }
        
        public void ModelToView() => CreateMap<OrderModel, OrderViewModel>()
            .ForMember(view => view.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(view => view.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(view => view.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(view => view.DesignerName, opt => opt.AllowNull())
            .ForMember(view => view.DesignerName, opt => opt.MapFrom(model => model.Designer != null ? model.Designer.UserName : null))
            .ForMember(view => view.Category, opt => opt.MapFrom(model => model.Category.Name));

        public void AddToModel() => CreateMap<OrderAddModel, OrderModel>();
        public void ModelToAdd() => CreateMap<OrderModel, OrderAddModel>();
        
        public void EditToModel() => CreateMap<OrderEditModel, OrderModel>();
        public void ModelToEdit() => CreateMap<OrderModel, OrderEditModel>();
    }
}
