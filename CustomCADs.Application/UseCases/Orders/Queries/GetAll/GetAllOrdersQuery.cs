using CustomCADs.Application.Models.Orders;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetAll
{
    public record GetAllOrdersQuery(
        string? Buyer = null,
        string? Status = null,
        string? Category = null,
        string? Name = null,
        string Sorting = "",
        int Page = 1,
        int Limit = 20) : IRequest<OrderResult>
    { }
}
