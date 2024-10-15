using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Create
{
    public record CreateCadCommand(CadModel Model) : IRequest<int> { }
}
