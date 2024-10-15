using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetImagePath
{
    public record SetOrderImagePathCommand(int Id, string ImagePath) : IRequest;
}
