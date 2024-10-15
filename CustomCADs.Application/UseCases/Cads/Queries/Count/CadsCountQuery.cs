using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.Count
{
    public record CadsCountQuery(Func<CadModel, bool> Predicate) : IRequest<int>;
}