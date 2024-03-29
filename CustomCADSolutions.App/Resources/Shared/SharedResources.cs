using System.Resources;

namespace CustomCADSolutions.App.Resources.Shared
{
    public class SharedResources
    {
        // Error messages
        public static string Required { get => GetString(nameof(Required)); }
        public static string Length { get => GetString(nameof(Length)); }
        public static string Range { get => GetString(nameof(Range)); }
        public static string InvalidEmail { get => GetString(nameof(InvalidEmail)); }
        public static string InvalidAxis { get => GetString(nameof(InvalidAxis)); }

        // Displays
        public static string Name { get => GetString(nameof(Name)); }
        public static string Category { get => GetString(nameof(Category)); }
        public static string Description { get => GetString(nameof(Description)); }
        public static string File { get => GetString(nameof(File)); } 
        public static string Axis { get => GetString(nameof(Axis)); }
        public static string Username { get => GetString(nameof(Username)); }
        public static string Email { get => GetString(nameof(Email)); }
        public static string Password { get => GetString(nameof(Password)); }
        public static string Confirm { get => GetString(nameof(Confirm)); }
        public static string Remember { get => GetString(nameof(Remember)); }
        public static string ByName { get => GetString(nameof(ByName)); }
        public static string ByCreator { get => GetString(nameof(ByCreator)); }
        public static string Sorting { get => GetString(nameof(Sorting)); }

        private static string GetString(string key) => new ResourceManager(typeof(SharedResources)).GetString(key)!;
        
    }
}
