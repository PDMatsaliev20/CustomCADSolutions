using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Create;

public class CreateCadHandler(ICommands<Cad> commands, IUnitOfWork unitOfWork) : IRequestHandler<CreateCadCommand, int>
{
    public async Task<int> Handle(CreateCadCommand request, CancellationToken cancellationToken)
    {
        Cad cad = request.Model.Adapt<Cad>();
        await commands.AddAsync(cad).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return cad.Id;
    }
}
