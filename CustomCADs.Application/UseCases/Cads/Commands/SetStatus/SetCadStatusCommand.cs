using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.SetStatus;

public record SetCadStatusCommand(int Id, string Action) : IRequest;
