namespace CustomCADs.Application.Exceptions
{
    [Serializable]
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException() : base("The requested Order does not exist.") { }
        public OrderNotFoundException(int id) : base($"The Order with id: {id} does not exist.") { }
        public OrderNotFoundException(string message) : base(message) { }
        public OrderNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
