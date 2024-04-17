using AutoMapper;
using CustomCADSolutions.Core.Mappings.DTOs;
using CustomCADSolutions.App.Models.Orders;
using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.App.Mappings
{
    public class OrderAppProfile : Profile
    {
        public OrderAppProfile()
        {         
            InputToModel();
            ModelToView();
            ModelToInput();
        }
        
        public void ModelToView() => CreateMap<OrderModel, OrderViewModel>()
            .ForMember(view => view.Status, opt => opt.MapFrom(model => model.Status.ToString()))
            .ForMember(view => view.OrderDate, opt => opt.MapFrom(model => model.OrderDate.ToString("dd/MM/yyyy HH:mm:ss")))
            .ForMember(view => view.BuyerName, opt => opt.MapFrom(model => model.Buyer.UserName))
            .ForMember(view => view.Category, opt => opt.MapFrom(model => model.Category.Name));

        public void InputToModel() => CreateMap<OrderInputModel, OrderModel>();
        
        public void ModelToInput() => CreateMap<OrderModel, OrderInputModel>();
    }
}
