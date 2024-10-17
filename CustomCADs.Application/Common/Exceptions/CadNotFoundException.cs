namespace CustomCADs.Application.Common.Exceptions;

[Serializable]
public class CadNotFoundException : Exception
{
    public CadNotFoundException() : base("The requested Cad does not exist.") { }
    public CadNotFoundException(int id) : base($"The Cad with id: {id} does not exist.") { }
    public CadNotFoundException(string message) : base(message) { }
    public CadNotFoundException(string message, Exception inner) : base(message, inner) { }
}
