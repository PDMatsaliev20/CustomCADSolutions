using System.Resources;

namespace CustomCADs.App.Resources.Shared
{
    public class DisplayResources
    {
        public static string Name { get => GetString(nameof(Name)); }
        public static string Category { get => GetString(nameof(Category)); }
        public static string Description { get => GetString(nameof(Description)); }
        public static string File { get => GetString(nameof(File)); } 
        public static string Folder { get => GetString(nameof(Folder)); } 
        public static string Axis { get => GetString(nameof(Axis)); }
        public static string Username { get => GetString(nameof(Username)); }
        public static string Email { get => GetString(nameof(Email)); }
        public static string Password { get => GetString(nameof(Password)); }
        public static string Confirm { get => GetString(nameof(Confirm)); }
        public static string Remember { get => GetString(nameof(Remember)); }
        public static string ByName { get => GetString(nameof(ByName)); }
        public static string ByCreator { get => GetString(nameof(ByCreator)); }
        public static string Sorting { get => GetString(nameof(Sorting)); }
        public static string RoleName { get => GetString(nameof(RoleName)); }
        public static string RoleDesc { get => GetString(nameof(RoleDesc)); }
        public static string Price { get => GetString(nameof(Price)); }

        private static string GetString(string key) => new ResourceManager(typeof(DisplayResources)).GetString(key)!;
    }
}
