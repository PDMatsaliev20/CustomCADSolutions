using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Core.Services;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace CustomCADSolutions.App.Extensions
{
    public static class UtilityExtensions
    {
        public static IEnumerable<string> GetErrors(this ModelStateDictionary model) => model.Values.Select(v => v.Errors).SelectMany(ec => ec.Select(e => e.ErrorMessage));

        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string GetCadPath(this IWebHostEnvironment hostingEnvironment, string name, int id, string extension = ".stl")
            => Path.Combine(hostingEnvironment.WebRootPath, "others", "cads", $"{name}{id}{extension}");

        public static async Task<CadQueryInputModel> QueryCads(this ICadService cadService, CadQueryInputModel inputQuery, bool validated = true, bool unvalidated = false)
        {
            if (inputQuery.CadsPerPage % inputQuery.Cols != 0)
            {
                inputQuery.CadsPerPage = inputQuery.Cols * (inputQuery.CadsPerPage / inputQuery.Cols);
            }
            CadQueryModel query = await cadService.GetAllAsync(
                category: inputQuery.Category,
                creatorName: inputQuery.Creator,
                searchName: inputQuery.SearchName,
                searchCreator: inputQuery.SearchCreator,
                sorting: inputQuery.Sorting,
                validated: validated,
                unvalidated: unvalidated,
                currentPage: inputQuery.CurrentPage,
                modelsPerPage: inputQuery.CadsPerPage);

            inputQuery.TotalCadsCount = query.TotalCount;
            inputQuery.Cads = query.CadModels
                .Select(m => new CadViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Category = m.Category.Name,
                    CreationDate = m.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = m.Creator!.UserName,
                    Coords = m.Coords,
                    SpinAxis = m.SpinAxis,
                    SpinFactor = m.SpinFactor,
                    IsValidated = m.IsValidated,
                }).ToArray();

            return inputQuery;
        }

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
