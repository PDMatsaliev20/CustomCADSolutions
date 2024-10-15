using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetById;

public record GetCadByIdQuery(int Id) : IRequest<CadModel>;