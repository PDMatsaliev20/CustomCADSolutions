using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace CustomCADSolutions.App.Extensions
{
    public static class Utilities
    {
        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string GetCadPath(this IWebHostEnvironment hostingEnvironment, string name, int id, string extension = ".stl")
            => Path.Combine(hostingEnvironment.WebRootPath, "others", "cads", $"{name}{id}{extension}");

        public static IEnumerable<string> GetErrors(this ModelStateDictionary model) => model.Values.Select(v => v.Errors).SelectMany(ec => ec.Select(e => e.ErrorMessage));

        public static async Task UploadCadAsync(this IWebHostEnvironment hostingEnvironment, IFormFile cad, int id, string name, string extension = ".stl")
        {
            string filePath = hostingEnvironment.GetCadPath(name, id, extension);
            using FileStream fileStream = new(filePath, FileMode.Create);
            await cad.CopyToAsync(fileStream);
        }

        public static void EditCad(this IWebHostEnvironment hostingEnvironment, int id, string oldName, string newName)
        {
            string source = hostingEnvironment.GetCadPath(oldName, id);
            string destination = hostingEnvironment.GetCadPath(newName, id);
            File.Move(source, destination);
        }

        public static void DeleteCad(this IWebHostEnvironment hostingEnvironment, string name, int id)
        {
            string filePath = hostingEnvironment.GetCadPath(name, id);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
