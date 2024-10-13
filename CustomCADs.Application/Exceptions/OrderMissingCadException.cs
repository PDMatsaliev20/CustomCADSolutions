namespace CustomCADs.Application.Exceptions
{
    [Serializable]
    public class OrderMissingCadException : Exception
    {
        public OrderMissingCadException() : base("The requested Order has no associated Cad with it.") { }
        public OrderMissingCadException(int id) : base($"The Order with id: {id} has no associated Cad with it.") { }
        public OrderMissingCadException(string message) : base(message) { }
        public OrderMissingCadException(string message, Exception inner) : base(message, inner) { }
    }
}
