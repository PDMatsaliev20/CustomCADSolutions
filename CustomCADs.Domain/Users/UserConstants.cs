namespace CustomCADs.Domain.Users;

public static class UserConstants
{
    public const int NameMaxLength = 62;
    public const int NameMinLength = 2;

    public const int PasswordMaxLength = 100;
    public const int PasswordMinLength = 6;

    public const int RefreshTokenDaysLimit = 7;
}
