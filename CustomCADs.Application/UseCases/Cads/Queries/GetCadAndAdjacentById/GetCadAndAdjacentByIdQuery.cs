using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetCadAndAdjacentById;

public record GetCadAndAdjacentByIdQuery(int Id) : IRequest<(int? PrevId, CadModel Current, int? NextId)>;
