namespace CustomCADs.Domain
{
    public static class DataConstants
    {
        public const string DateFormatString = "dd.MM.yyyy HH:mm:ss";

        public const string RequiredErrorMessage = "{PropertyName} is required!";
        public const string LengthErrorMessage = "{PropertyName} length must be between {MinLength} and {MaxLength} characters";
        public const string RangeErrorMessage = "{PropertyName} must be between {From} and {To}";

        public class CadConstants
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

        public class OrderConstants
        {
            public const int NameMaxLength = 18;
            public const int NameMinLength = 2;

            public const int DescriptionMaxLength = 750;
            public const int DescriptionMinLength = 5;
        }

        public class UserConstants
        {
            public const int NameMaxLength = 62;
            public const int NameMinLength = 2;

            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;

            public const int RefreshTokenDaysLimit = 7;
        }

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
    }
}
