namespace CustomCADs.API.Helpers
{
    public static class FileManager
    {
        public static string GetFileExtension(this IFormFile cad)
            => Path.GetExtension(cad.FileName);

        public static string GetRelativeCadPath(string name)
            => $"/{Path.Combine("others", "cads", name)}";

        public static string GetCadPath(this IWebHostEnvironment env, string fileName)
            => Path.Combine(env.WebRootPath, "others", "cads", fileName);
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

        public static string RenameFile(this IWebHostEnvironment env, string oldName, string newName)
        {
            File.Move(env.GetCadPath(oldName), env.GetCadPath(newName));
            return GetRelativeCadPath(newName);
        }
        
        public static void DeleteFile(this IWebHostEnvironment env, string path)
        {
            string filePath = Path.Combine(env.WebRootPath, path[1..]);
            File.Delete(filePath);
        }

    }
}
