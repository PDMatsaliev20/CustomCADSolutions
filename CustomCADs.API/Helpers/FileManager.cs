using System.IO.Compression;

namespace CustomCADs.API.Helpers
{
    public static class FileManager
    {
        public static string GetFileExtension(this IFormFile file)
            => Path.GetExtension(file.FileName);

        public static string GetRelativePath(string folder, string name)
            => $"\\{Path.Combine("files", folder, name)}";

        public static string GetPath(this IWebHostEnvironment env, string folder, string fileName)
            => Path.Combine(env.WebRootPath, "files", folder, fileName);

        public static async Task<string> UploadOrderAsync(this IWebHostEnvironment env, IFormFile image, string fileName)
        {
            if (image != null && image.Length != 0)
            {
                string a = env.WebRootPath;
                string b = GetRelativePath("orders", fileName);
                string c = Path.Combine(a, b);
                using FileStream stream = new(env.GetPath("orders", fileName), FileMode.Create);
                await image.CopyToAsync(stream).ConfigureAwait(false);

                return GetRelativePath("orders", fileName);
            }
            else throw new ArgumentNullException();
        }

        public static async Task<string> UploadImageAsync(this IWebHostEnvironment env, IFormFile image, string fileName)
        {
            if (image != null && image.Length != 0)
            {
                using FileStream stream = new(env.GetPath("images", fileName), FileMode.Create);
                await image.CopyToAsync(stream).ConfigureAwait(false);

                return GetRelativePath("images", fileName);
            }
            else throw new ArgumentNullException();
        }

        public static async Task<string> UploadCadAsync(this IWebHostEnvironment env, IFormFile cad, string name, string extension)
        {
            if (cad.Length != 0)
            {
                string uploadedFilePath = env.GetPath("cads", $"{name}{extension}");
                using (FileStream stream = new(uploadedFilePath, FileMode.Create))
                {
                    await cad.CopyToAsync(stream).ConfigureAwait(false);
                }

                if (cad.GetFileExtension() == ".zip" && cad.IsValidZipFile())
                {
                    string extractedFolderPath = env.GetPath("cads", $"{name}");
                    SafeExtractZipFile(cad, extractedFolderPath);

                    ZipFile.ExtractToDirectory(uploadedFilePath, extractedFolderPath);
                    File.Delete(uploadedFilePath);
                    return GetRelativeCadPathFromFolder(extractedFolderPath, ["gltf"])
                           ?? throw new ArgumentNullException("No CAD in the folder.");
                }
                return GetRelativePath("cads", name + extension);
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

        public static bool IsValidZipFile(this IFormFile file)
        {
            try
            {
                using Stream stream = file.OpenReadStream();
                using ZipArchive zip = new(stream, ZipArchiveMode.Read, true);
                return zip.Entries.Count > 0;
            }
            catch (InvalidDataException)
            {
                return false;
            }
        }

        public static void SafeExtractZipFile(IFormFile zipFile, string extractPath)
        {
            Stream stream = zipFile.OpenReadStream();
            ZipArchive archive = new(stream);
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

                    if (!destinationPath.StartsWith(Path.GetFullPath(extractPath)))
                    {
                        throw new IOException("Attempt to extract file outside of target directory.");
                    }
                }
            }
        }

        public static async Task<byte[]> GetCadBytes(this IWebHostEnvironment env, string name, string extension)
        {
            string path = env.GetPath("cads", name);
            if (extension == ".glb")
            {
                byte[] bytes = await File.ReadAllBytesAsync(path + extension).ConfigureAwait(false);
                using MemoryStream memoryStream = new(bytes);
                return memoryStream.ToArray();
            }
            else if (extension == ".gltf")
            {
                using MemoryStream memoryStream = new();
                using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
                {
                    string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        string relativePath = Path.GetRelativePath(path, file);
                        archive.CreateEntryFromFile(file, relativePath);
                    }
                }
                memoryStream.Position = 0;
                return memoryStream.ToArray();
            }
            else throw new Exception();
        }

        public static void DeleteFile(this IWebHostEnvironment env, string folder, string name, string extension)
        {
            switch (folder)
            {
                case "orders":
                case "images":
                    File.Delete(env.GetPath(folder, name + extension));
                    break;

                case "cads":
                    switch (extension)
                    {
                        case ".glb":
                            File.Delete(env.GetPath("cads", name + extension));
                            break;
                        case ".gltf":
                            Directory.Delete(env.GetPath("cads", name), true);
                            break;
                        default:
                            throw new Exception("Unsupported CAD type.");
                    }
                    break;

                default: throw new InvalidOperationException();
            }
        }
    }
}
