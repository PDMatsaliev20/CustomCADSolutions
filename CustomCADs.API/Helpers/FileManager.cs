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
                await image.CopyToAsync(stream);

                return GetRelativeImagePath(fileName);
            }
            else throw new ArgumentNullException();
        }

        public static async Task<string> UploadCadAsync(this IWebHostEnvironment env, IFormFile cad, string fileName)
        {
            if (cad != null && cad.Length != 0)
            {
                using FileStream stream = new(env.GetCadPath(fileName), FileMode.Create);
                await cad.CopyToAsync(stream);

                return GetRelativeCadPath(fileName);
            }
            else throw new ArgumentNullException();
        }

        public static string RenameImage(this IWebHostEnvironment env, string oldName, string newName)
        {
            File.Move(env.GetImagePath(oldName), env.GetImagePath(newName));
            return GetRelativeCadPath(newName);
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
