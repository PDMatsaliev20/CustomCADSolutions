using System.IO.Compression;

namespace CustomCADs.API.Helpers
{
    public static class FileManager
    {
        public static string GetFileExtension(this IFormFile file)
            => Path.GetExtension(file.FileName);

        public static string GetRelativeImagePath(string name)
            => $"/{Path.Combine("others", "images", name)}";

        public static string GetRelativeCadPath(string name)
            => $"/{Path.Combine("others", "cads", name)}";

        public static string GetImagePath(this IWebHostEnvironment env, string fileName)
            => Path.Combine(env.WebRootPath, "others", "images", fileName);

        public static string GetCadPath(this IWebHostEnvironment env, string fileName)
            => Path.Combine(env.WebRootPath, "others", "cads", fileName);

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
                return GetRelativeCadPath(name);
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
                        return file;
                    }
                }
                return GetRelativeCadPathFromFolder(subfolder, extensionsToSearch);
            }
            return null;
        }

        public static string RenameImage(this IWebHostEnvironment env, string oldName, string newName)
        {
            File.Move(env.GetImagePath(oldName), env.GetImagePath(newName));
            return GetRelativeImagePath(newName);
        }

        public static string RenameCad(this IWebHostEnvironment env, string oldName, string newName)
        {
            File.Move(env.GetCadPath(oldName), env.GetCadPath(newName));
            return GetRelativeCadPath(newName);
        }

        public static void DeleteFile(this IWebHostEnvironment env, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }
            string filePath = Path.Combine(env.WebRootPath, path[1..]);
            File.Delete(filePath);
        }

    }
}
