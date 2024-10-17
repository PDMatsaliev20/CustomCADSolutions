namespace CustomCADs.Domain.Cads;

public static class CadConstants
{
    public const int NameMaxLength = 18;
    public const int NameMinLength = 2;

    public const int DescriptionMaxLength = 750;
    public const int DescriptionMinLength = 10;

    public const decimal PriceMin = 0.01m;
    public const decimal PriceMax = 1000m;

    public const string PriceMinString = "0.01";
    public const string PriceMaxString = "1000";

    public const int CoordMin = -1000;
    public const int CoordMax = 1000;
}
