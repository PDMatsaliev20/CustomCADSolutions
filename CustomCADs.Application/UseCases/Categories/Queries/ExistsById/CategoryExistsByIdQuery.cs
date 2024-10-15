using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.ExistsById;

public record CategoryExistsByIdQuery(int Id) : IRequest<bool>;
