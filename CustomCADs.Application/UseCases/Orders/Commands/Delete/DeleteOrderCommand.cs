using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Delete;

public record DeleteOrderCommand(int Id) : IRequest;
