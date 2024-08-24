namespace CustomCADs.Domain
{
    public static class DataConstants
    {
        public const string RequiredErrorMessage = "{0} is required!";
        public const string LengthErrorMessage = "{0} length must be between {2} and {1} characters";
        public const string RangeErrorMessage = "{0} must be between {2} and {1}";

        public class CadConstants
        {

            public const string NameDisplay = "Name";
            public const int NameMaxLength = 18;
            public const int NameMinLength = 2;
            
            public const string DescriptionDisplay = "Description";
            public const int DescriptionMaxLength = 750;
            public const int DescriptionMinLength = 10;

            public const string CategoryDisplay = "Category";

            public const string FileDisplay = "3D Model";

            public const double PriceMin = 0.01;
            public const double PriceMax = 1000;

            public const int CoordMin = -1000;
            public const int CoordMax = 1000;
            
            public const string SpinAxisErrorMessage = "{0} must be either x, y or z";
            public const string SpinAxisRegEx = "[xyz]?";
        }

        public class OrderConstants
        {
            public const string NameDisplay = "Name of 3D Model";
            
            public const int NameMaxLength = 18;
            public const int NameMinLength = 2;

            public const string CategoryDisplay = "Category of 3D Model";

            public const string DescriptionDisplay = "Full Description of 3D Model";
            public const int DescriptionMaxLength = 750;
            public const int DescriptionMinLength = 5;
        }

        public class UserConstants
        {
            public const int NameMaxLength = 62;
            public const int NameMinLength = 2;

            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;
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
