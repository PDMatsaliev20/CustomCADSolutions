using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.HasCadById
{
    public class OrderHasCadByIdHandler(IOrderQueries queries) : IRequestHandler<OrderHasCadByIdQuery, bool>
    {
        public async Task<bool> Handle(OrderHasCadByIdQuery request, CancellationToken cancellationToken)
        {
            Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(request.Id);

            var response = order.CadId != null && order.Cad != null;
            return response;
        }
    }
}
