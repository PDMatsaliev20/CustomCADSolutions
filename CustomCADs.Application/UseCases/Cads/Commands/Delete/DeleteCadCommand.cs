using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Delete
{
    public record DeleteCadCommand(int Id) : IRequest { }
}