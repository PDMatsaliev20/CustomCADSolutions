namespace CustomCADSolutions.Infrastructure.Constants
{
    public static class DataConstants
    {
        public const string RequiredErrorMessage = "{0} is required!";
        public const string LengthErrorMessage = "{0} length must be between {2} and {1} characters";

        public class Cad
        {
            public const string NameDisplay = "Name of 3D Model";
            public const int NameMaxLength = 50;
            public const int NameMinLength = 3;

            public const string CategoryDisplay = "Category of 3D Model";

            public const string FileDisplay = "3D Model";
        }

        public class Order
        {
            public const string NameDisplay = "Name of 3D Model";

            public const string CategoryDisplay = "Category of 3D Model";

            public const string DescriptionDisplay = "Full Description of 3D Model";
            public const int DescriptionMaxLength = 1500;
            public const int DescriptionMinLength = 5;
        }
    }
}
