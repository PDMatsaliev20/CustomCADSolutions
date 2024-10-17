namespace CustomCADs.Application.Common.Exceptions;

public class CadStatusException : Exception
{
    public CadStatusException() : base("The provided Cad cannot perform the requested action.") { }
    public CadStatusException(string message) : base(message) { }
    public CadStatusException(int cadId, string action) : base($"The Cad with id: {cadId} cannot perform the action: {action}.") { }
    public CadStatusException(string message, Exception inner) : base(message, inner) { }
}
