namespace CustomCADs.Application.Common.Exceptions;

[Serializable]
public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException() : base("The requested Category does not exist.") { }
    public CategoryNotFoundException(int id) : base($"The Category with id: {id} does not exist.") { }
    public CategoryNotFoundException(string message) : base(message) { }
    public CategoryNotFoundException(string message, Exception inner) : base(message, inner) { }
}
