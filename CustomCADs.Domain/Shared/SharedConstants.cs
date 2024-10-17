namespace CustomCADs.Domain.Shared;

public static class SharedConstants
{
    public const string DateFormatString = "dd.MM.yyyy HH:mm:ss";

    public const string RequiredErrorMessage = "{PropertyName} is required!";
    public const string LengthErrorMessage = "{PropertyName} length must be between {MinLength} and {MaxLength} characters";
    public const string RangeErrorMessage = "{PropertyName} must be between {From} and {To}";
}
