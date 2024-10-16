namespace CustomCADs.Application.Exceptions;

public class OrderStatusException : Exception
{
    public OrderStatusException() : base("The provided Order cannot perform the requested action.") { }
    public OrderStatusException(string message) : base(message) { }
    public OrderStatusException(int orderId, string action) : base($"The Order with id: {orderId} cannot perform the action: {action}.") { }
    public OrderStatusException(string message, Exception inner) : base(message, inner) { }
}
