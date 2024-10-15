using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.IsBuyer
{
    public record IsOrderBuyerQuery(int Id, string Username) : IRequest<bool> { }
}
