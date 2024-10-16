using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Designer.OngoingOrder;
using CustomCADs.API.Endpoints.Designer.OngoingOrders;
using CustomCADs.API.Endpoints.Designer.RecentOngoingOrders;
using CustomCADs.API.Endpoints.Orders.GalleryOrder;
using CustomCADs.API.Endpoints.Orders.GetOrder;
using CustomCADs.API.Endpoints.Orders.GetOrders;
using CustomCADs.API.Endpoints.Orders.PostOrder;
using CustomCADs.API.Endpoints.Orders.RecentOrders;
using CustomCADs.Application.Models.Orders;
using Mapster;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Mappers;

public class OrdersMapper : IRegister
{
    private static readonly Func<OrderModel, string> mapOrderDate = m => m.OrderDate.ToString(DateFormatString);
    private static readonly Func<OrderModel, string> mapBuyerName = m => m.Buyer.UserName;
    private static readonly Func<OrderModel, string> mapStatus = m => m.Status.ToString();
    private static readonly Func<OrderModel, CategoryDto> mapCategory = m => new(m.CategoryId, m.Category.Name);
    private static readonly Func<OrderModel, string?> mapDesignerEmail = m => m.Designer?.Email;
    private static readonly Func<OrderModel, string?> mapDesignerName = m => m.Designer?.UserName;

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderModel, GalleryOrderResponse>()
            .Map(r => r.DesignerName, m => mapDesignerName(m))
            .Map(r => r.DesignerEmail, m => mapDesignerEmail(m))
            .Map(r => r.Category, m => mapCategory(m));

        config.NewConfig<OrderModel, GetOrderResponse>()
            .Map(r => r.BuyerName, m => mapBuyerName(m))
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.Category, m => mapCategory(m));

        config.NewConfig<OrderModel, GetOrdersResponse>()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.DesignerEmail, m => mapDesignerEmail(m))
            .Map(r => r.DesignerName, m => mapDesignerName(m))
            .Map(r => r.Category, m => mapCategory(m));

        config.NewConfig<OrderModel, PostOrderResponse>()
            .Map(r => r.BuyerName, m => mapBuyerName(m))
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.DesignerEmail, m => mapDesignerEmail(m))
            .Map(r => r.DesignerName, m => mapDesignerName(m))
            .Map(r => r.Category, m => mapCategory(m));

        config.NewConfig<OrderModel, RecentOrdersResponse>()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.Category, m => mapCategory(m));

        config.NewConfig<OrderModel, OngoingOrdersResponse>()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Category, m => mapCategory(m));

        config.NewConfig<OrderModel, OngoingOrderResponse>()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Category, m => mapCategory(m))
            .Map(r => r.BuyerName, m => mapBuyerName(m))
            .Map(r => r.Status, m => mapStatus(m));

        config.NewConfig<OrderModel, RecentOngoingOrdersResponse>()
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Category, m => mapCategory(m));
    }
}
