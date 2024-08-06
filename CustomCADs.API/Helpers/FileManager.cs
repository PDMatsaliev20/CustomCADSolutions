using System.IO.Compression;

namespace CustomCADs.API.Helpers
{
    public static class FileManager
    {
        public static string GetFileExtension(this IFormFile file)
            => Path.GetExtension(file.FileName);

        public static string GetRelativeImagePath(string name)
            => $"\\{Path.Combine("files", "images", name)}";

        public static string GetRelativeCadPath(string name)
            => $"\\{Path.Combine("files", "cads", name)}";

        public static string GetImagePath(this IWebHostEnvironment env, string fileName)
            => Path.Combine(env.WebRootPath, "files", "images", fileName);

        public static string GetCadPath(this IWebHostEnvironment env, string fileName)
            => Path.Combine(env.WebRootPath, "files", "cads", fileName);

        public static async Task<string> UploadImageAsync(this IWebHostEnvironment env, IFormFile image, string fileName)
        {
            if (image != null && image.Length != 0)
            {
                using FileStream stream = new(env.GetImagePath(fileName), FileMode.Create);
                await image.CopyToAsync(stream).ConfigureAwait(false);

                return GetRelativeImagePath(fileName);
            }
            else throw new ArgumentNullException();
        }

        public static async Task<string> UploadCadAsync(this IWebHostEnvironment env, IFormFile cad, string name, string extension)
        {
            if (cad.Length != 0)
            {
                string uploadedFilePath = env.GetCadPath($"{name}{extension}");
                using (FileStream stream = new(uploadedFilePath, FileMode.Create))
                {
                    await cad.CopyToAsync(stream).ConfigureAwait(false);
                }

                if (cad.GetFileExtension() == ".zip")
                {
                    string extractedFolderPath = env.GetCadPath($"{name}");
                    ZipFile.ExtractToDirectory(uploadedFilePath, extractedFolderPath);

                    File.Delete(uploadedFilePath);
                    return GetRelativeCadPathFromFolder(extractedFolderPath, ["gltf"])
                           ?? throw new ArgumentNullException("No CAD in the folder.");
                }
                return GetRelativeCadPath(name + extension);
            }
            else throw new ArgumentNullException("Empty file.");
        }

        private static string? GetRelativeCadPathFromFolder(string folder, string[] extensionsToSearch)
        {
            foreach (string subfolder in Directory.EnumerateDirectories(folder))
            {
                foreach (string file in Directory.EnumerateFiles(folder))
                {
                    if (extensionsToSearch.Contains(file.Split('.')[^1].ToLower()))
                    {
                        var fileParts = file.Split("\\").SkipWhile(a => a != "cads");
                        string relativeFilePath = string.Join("/", fileParts);
                        return $"/files/{relativeFilePath}";
                    }
                }
                return GetRelativeCadPathFromFolder(subfolder, extensionsToSearch);
            }
            return null;
        }

        public static void DeleteImage(this IWebHostEnvironment env, string fileName)
            => File.Delete(env.GetImagePath(fileName));
        
        public static void DeleteCad(this IWebHostEnvironment env, string name, string extension)
        {
            switch (extension)
            {
                case ".glb":
                    File.Delete(env.GetCadPath(name + extension));
                    break;
                case ".gltf":
                    Directory.Delete(env.GetCadPath(name), true);
                    break;
                default:
                    throw new Exception("Unsupported CAD type.");
            }                
        }
    }
}
