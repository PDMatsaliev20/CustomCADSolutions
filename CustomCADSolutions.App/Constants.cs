namespace CustomCADSolutions.App
{
    public static class Constants
    {
        public static class Paths
        {
            public const string RootPath = "https://localhost:7119/API";
            public const string OrderRootPath = RootPath + "/Orders";
            public const string CadRootPath = RootPath + "/Cads";

            public const string OrdersPath = OrderRootPath;
            public const string OrderByIdPath = OrderRootPath + "/{0}/{1}";
            public const string CreateOrderPath = OrderRootPath + "/Create";
            public const string EditOrderPath = OrderRootPath + "/Edit";
            public const string DeleteOrderPath = OrderRootPath + "/{0}/{1}";

            public const string CadsPath = CadRootPath;
            public const string CadByIdPath = CadRootPath + "/{0}";
            public const string CreateCadPath = CadRootPath + "/Create";
            public const string EditCadPath = CadRootPath + "/Edit";
            public const string DeleteCadPath = CadRootPath + "/{0}";
        }
    }
}
