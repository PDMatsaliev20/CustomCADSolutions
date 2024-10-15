using CustomCADs.Application.Models.Cads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Edit
{
    public record EditCadCommand(int Id, CadModel Model) : IRequest;
}