using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.SetPaths
{
    public record SetCadPathsCommand(int Id, string CadPath, string ImagePath) : IRequest { }
}
