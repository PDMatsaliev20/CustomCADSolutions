using CustomCADs.Application.Models.Orders;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Edit;

public record EditOrderCommand(int Id, OrderModel Model) : IRequest;
