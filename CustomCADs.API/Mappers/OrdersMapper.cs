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

public class OrdersMapper
{
    private static readonly Func<OrderModel, string> mapOrderDate = m => m.OrderDate.ToString(DateFormatString);
    private static readonly Func<OrderModel, string> mapBuyerName = m => m.Buyer.UserName;
    private static readonly Func<OrderModel, string> mapStatus = m => m.Status.ToString();
    private static readonly Func<OrderModel, CategoryDto> mapCategory = m => new(m.CategoryId, m.Category.Name);
    private static readonly Func<OrderModel, string?> mapDesignerEmail = m => m.Designer?.Email;
    private static readonly Func<OrderModel, string?> mapDesignerName = m => m.Designer?.UserName;

    public static void Map()
    {
        TypeAdapterConfig<OrderModel, GalleryOrderResponse>.NewConfig()
            .Map(r => r.DesignerName, m => mapDesignerName(m))
            .Map(r => r.DesignerEmail, m => mapDesignerEmail(m))
            .Map(r => r.Category, m => mapCategory(m));

        TypeAdapterConfig<OrderModel, GetOrderResponse>.NewConfig()
            .Map(r => r.BuyerName, m => mapBuyerName(m))
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.Category, m => mapCategory(m));

        TypeAdapterConfig<OrderModel, GetOrdersResponse>.NewConfig()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.DesignerEmail, m => mapDesignerEmail(m))
            .Map(r => r.DesignerName, m => mapDesignerName(m))
            .Map(r => r.Category, m => mapCategory(m));

        TypeAdapterConfig<OrderModel, PostOrderResponse>.NewConfig()
            .Map(r => r.BuyerName, m => mapBuyerName(m))
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.DesignerEmail, m => mapDesignerEmail(m))
            .Map(r => r.DesignerName, m => mapDesignerName(m))
            .Map(r => r.Category, m => mapCategory(m));

        TypeAdapterConfig<OrderModel, RecentOrdersResponse>.NewConfig()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.Category, m => mapCategory(m));

        TypeAdapterConfig<OrderModel, OngoingOrdersResponse>.NewConfig()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Category, m => mapCategory(m));

        TypeAdapterConfig<OrderModel, OngoingOrderResponse>.NewConfig()
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Category, m => mapCategory(m))
            .Map(r => r.BuyerName, m => mapBuyerName(m))
            .Map(r => r.Status, m => mapStatus(m));

        TypeAdapterConfig<OrderModel, RecentOngoingOrdersResponse>.NewConfig()
            .Map(r => r.Status, m => mapStatus(m))
            .Map(r => r.OrderDate, m => mapOrderDate(m))
            .Map(r => r.Category, m => mapCategory(m));
    }
}
