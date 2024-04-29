using System.Resources;

namespace CustomCADSolutions.App.Resources.Shared
{
    public class ErrorMessageResources
    {
        public static string Required { get => GetString(nameof(Required)); }
        public static string Length { get => GetString(nameof(Length)); }
        public static string Range { get => GetString(nameof(Range)); }
        public static string InvalidEmail { get => GetString(nameof(InvalidEmail)); }
        public static string PasswordMismatch { get  => GetString(nameof(PasswordMismatch)); }

        private static string GetString(string key) => new ResourceManager(typeof(ErrorMessageResources)).GetString(key)!;
    }
}
