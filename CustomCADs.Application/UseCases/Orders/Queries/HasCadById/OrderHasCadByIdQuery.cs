using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.HasCadById;

public record OrderHasCadByIdQuery(int Id) : IRequest<bool>;
