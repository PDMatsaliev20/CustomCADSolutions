namespace CustomCADs.Application.Exceptions;

[Serializable]
public class RoleNotFoundException : Exception
{
    public RoleNotFoundException() : base("The requested Role does not exist.") { }
    public RoleNotFoundException(string message) : base(message) { }
    public RoleNotFoundException(string message, Exception inner) : base(message, inner) { }
}
