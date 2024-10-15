using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetCadById
{
    public record GetOrderCadByIdQuery(int Id) : IRequest<CadModel> {}
}
