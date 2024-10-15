using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.ExistsById;

public record OrderExistsByIdQuery(int Id) : IRequest<bool>;
