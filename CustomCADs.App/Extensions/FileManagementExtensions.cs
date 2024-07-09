using System.Text.RegularExpressions;

namespace CustomCADs.App.Extensions
{
    public static class FileManagementExtensions
    {
        public static string GetFileExtension(this IFormFile cad)
            => Path.GetExtension(cad.FileName);

        public static async Task<string?> UploadCadAsync(this IWebHostEnvironment env, IFormFile cad, string cadPath)
        {
            if (cad != null && cad.Length != 0)
            {
                string fullPath = Path.Combine(env.WebRootPath, "others", "cads", cadPath);
                using FileStream stream = new(fullPath, FileMode.Create);
                await cad.CopyToAsync(stream);

                return $"/others/cads/{cadPath}";
            }
            else return null;
        }

        public static async Task<string?> UploadCadFolderAsync(this IWebHostEnvironment env,
            IFormFile cad, string cadName, IEnumerable<IFormFile> files)
        {
            if (cad != null && cad.Length != 0)
            {
                Regex regex = new(@"^\w+/");
                string root = regex.Match(cad.FileName).Value;

                // Removes root folder from path
                string cadPath = cad.FileName[root.Length..];

                // Adds unique root folder to path
                string newRoot = Path.Combine(env.WebRootPath, "others", "cads", cadName);
                Directory.CreateDirectory(newRoot);

                string fullCadPath = Path.Combine(newRoot, cadPath);
                using FileStream cadStream = new(fullCadPath, FileMode.Create);
                await cad.CopyToAsync(cadStream);

                foreach (IFormFile file in files)
                {
                    await file.UploadFileAsync(root, newRoot);
                }

                return $"/others/cads/{cadName}/{cadPath}";
            }
            else return null;
        }

        public static async Task UploadFileAsync(this IFormFile file, string root, string newRoot)
        {
            string filePath = file.FileName[root.Length..];

            Regex newRegex = new(@"/?\w+.\w+$");
            string fileName = newRegex.Match(filePath).Value;

            string folders = filePath[..^fileName.Length];
            if (!string.IsNullOrWhiteSpace(folders))
            {
                Directory.CreateDirectory(Path.Combine(newRoot, folders));
            }

            string fullFilePath = Path.Combine(newRoot, filePath);
            using FileStream stream = new(fullFilePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public static void DeleteFolder(this IWebHostEnvironment env, string path, int maxRetries = 3, int delayMilliseconds = 100)
        {
            string directoryPath = Path.Combine(env.WebRootPath, path);

            if (Directory.Exists(directoryPath))
            {
                DeleteDirectoryWithRetry(directoryPath, maxRetries, delayMilliseconds);
            }
        }

        private static void DeleteDirectoryWithRetry(string directoryPath, int maxRetries, int delayMilliseconds)
        {
            foreach (string file in Directory.GetFiles(directoryPath))
            {
                DeleteFileWithRetry(file, maxRetries, delayMilliseconds);
            }

            foreach (string subDirectory in Directory.GetDirectories(directoryPath))
            {
                DeleteDirectoryWithRetry(subDirectory, maxRetries, delayMilliseconds);
            }

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    Directory.Delete(directoryPath, false);
                    break; // Exit loop if successful
                }
                catch (IOException)
                {
                    if (attempt == maxRetries - 1) throw; // Rethrow if last attempt
                    Thread.Sleep(delayMilliseconds); // Wait before retrying
                }
            }
        }

        private static void DeleteFileWithRetry(string filePath, int maxRetries, int delayMilliseconds)
        {
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        break; // Exit loop if successful
                    }
                }
                catch (IOException)
                {
                    if (attempt == maxRetries - 1) throw; // Rethrow if last attempt
                    Thread.Sleep(delayMilliseconds); // Wait before retrying
                }
            }
        }

        public static void DeleteFile(this IWebHostEnvironment env, string path)
        {
            string filePath = Path.Combine(env.WebRootPath, path[1..]);
            System.IO.File.Delete(filePath);
        }

    }
}
