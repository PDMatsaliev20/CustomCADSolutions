namespace CustomCADSolutions.App
{
    public static class Constants
    {
        public static class Paths
        {
            public const string RootPath = "https://localhost:7119";
            public const string OrdersAPIPath = $"{RootPath}/OrdersAPI";
            public const string CadsAPIPath = $"{RootPath}/CadsAPI";
            public const string CategoriesAPIPath = $"{RootPath}/CategoriesAPI";
        }

        public static class Roles
        {
            public const int NameMaxLength = 20;
            public const int NameMinLength = 2;
        }
    }
}
