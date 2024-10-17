namespace CustomCADs.Domain.Roles;

public static class RoleConstants
{
    public const string Admin = "Administrator";
    public const string AdminDescription = "Has full access to Users, Roles, Orders, Cads, Categories and all other endpoints - can do anyhting.";
    public const string Designer = "Designer";
    public const string DesignerDescription = "Has access to Cads and Designer endpoints - can upload his 3D Models straight to the Gallery, validate contributors' cads and finish clients' orders.";
    public const string Contributor = "Contributor";
    public const string ContributorDescription = "Has access to Cads endpoints - can apply to upload his 3D Models to the Gallery, set their prices and track their status";
    public const string Client = "Client";
    public const string ClientDescription = "Has access to Orders endpoints - can buy 3D Models from the Gallery and make and track Orders.";

    public const int NameMinLength = 2;
    public const int NameMaxLength = 20;

    public const int DescriptionMinLength = 2;
    public const int DescriptionMaxLength = 200;
}
