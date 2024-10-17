using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetCadById;

public class GetOrderCadByIdHandler(IOrderQueries queries) : IRequestHandler<GetOrderCadByIdQuery, CadModel>
{
    public async Task<CadModel> Handle(GetOrderCadByIdQuery req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        if (order.CadId == null || order.Cad == null)
        {
            throw new OrderMissingCadException(req.Id);
        }

        var response = order.Cad.Adapt<CadModel>();
        return response;
    }
}
