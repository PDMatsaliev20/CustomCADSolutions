using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetStatus;

public record SetOrderStatusCommand(
    int Id,
    string Action,
    string? DesignerId = null,
    int? CadId = null) : IRequest;
