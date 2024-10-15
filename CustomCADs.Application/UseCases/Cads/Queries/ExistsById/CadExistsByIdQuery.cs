using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.ExistsById
{
    public record CadExistsByIdQuery(int Id) : IRequest<bool>;
}