namespace CustomCADs.Application.Exceptions
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("The requested User does not exist.") { }
        public UserNotFoundException(int id) : base($"The User with id: {id} does not exist.") { }
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
