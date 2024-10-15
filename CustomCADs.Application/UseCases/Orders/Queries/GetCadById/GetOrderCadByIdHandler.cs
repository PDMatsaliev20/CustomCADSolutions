using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetCadById;

public class GetOrderCadByIdHandler(IOrderQueries queries) : IRequestHandler<GetOrderCadByIdQuery, CadModel>
{
    public async Task<CadModel> Handle(GetOrderCadByIdQuery request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id, asNoTracking: true).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(request.Id);

        if (order.CadId == null || order.Cad == null)
        {
            throw new OrderMissingCadException(request.Id);
        }

        var response = order.Cad.Adapt<CadModel>();
        return response;
    }
}
