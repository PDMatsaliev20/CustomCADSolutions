using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetAll
{
    public record GetAllCadsQuery(
        string? Creator = null,
        string? Status = null,
        string? Category = null,
        string? Name = null,
        string Sorting = "",
        int Page = 1,
        int Limit = 20) : IRequest<CadResult>
    { }
}