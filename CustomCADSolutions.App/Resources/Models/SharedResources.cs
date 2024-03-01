using System.Resources;

namespace CustomCADSolutions.App.Resources.Shared
{
    public class SharedResources
    {
        public static string Required { get => GetString(nameof(Required))!; }
        public static string Length { get => GetString(nameof(Length))!; }
        public static string Range { get => GetString(nameof(Range))!; }
        public static string Name { get => GetString(nameof(Name))!; }
        public static string Category { get => GetString(nameof(Category))!; }
        public static string Description { get => GetString(nameof(Description))!; }
        public static string File { get => GetString(nameof(Category))!; }
        public static string Speed { get => GetString(nameof(Speed))!; }
        public static string Axis { get => GetString(nameof(Axis))!; }

        private static string GetString(string key)
            => new ResourceManager(typeof(SharedResources)).GetString(key)!;
        
    }
}
