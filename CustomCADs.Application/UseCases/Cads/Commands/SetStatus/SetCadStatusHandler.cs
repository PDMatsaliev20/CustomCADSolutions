using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.SetStatus;

public class SetCadStatusHandler(ICadQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetCadStatusCommand>
{
    public async Task Handle(SetCadStatusCommand req, CancellationToken ct)
    {
        Cad cad = await queries.GetByIdAsync(req.Id, ct: ct)
            ?? throw new CadNotFoundException(req.Id);

        switch (req.Action)
        {
            case "validate":
                ValidateCad(cad);
                break;

            case "report":
                ReportCad(cad);
                break;

            default: throw new CadStatusException(req.Id, req.Action);
        }

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    private static void ValidateCad(Cad cad)
    {
        if (cad.Status != CadStatus.Unchecked)
        {
            throw new CadStatusException();
        }
        cad.Status = CadStatus.Validated;
    }
    
    private static void ReportCad(Cad cad)
    {
        if (cad.Status != CadStatus.Unchecked)
        {
            throw new CadStatusException();
        }
        cad.Status = CadStatus.Reported;
    }
}
