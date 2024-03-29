using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using Stripe;
using System.Drawing;

namespace CustomCADSolutions.App.Extensions
{
    public static class UtilityExtensions
    {
        public static bool ProcessPayment(this StripeSettings stripeSettings, string stripeToken)
        {
            StripeConfiguration.ApiKey = stripeSettings.SecretKey;
            ChargeCreateOptions options = new()
            {
                Amount = 100,
                Currency = "bgn",
                Source = stripeToken,
                Description = "Example Charge",
            };
            Charge charge = new ChargeService().Create(options);

            return charge.Status == "succeeded";
        }

        public static IEnumerable<string> GetErrors(this ModelStateDictionary model) => model.Values.Select(v => v.Errors).SelectMany(ec => ec.Select(e => e.ErrorMessage));

        public static string GetId(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.NameIdentifier);

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
                .Where(m => m.Bytes != null)
                .Select(m => new CadViewModel
                {
                    Id = m.Id,
                    Cad = m.Bytes!,
                    Name = m.Name,
                    Category = m.Category.Name,
                    CreationDate = m.CreationDate!.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                    CreatorName = m.Creator!.UserName,
                    Coords = m.Coords,
                    SpinAxis = m.SpinAxis,
                    IsValidated = m.IsValidated,
                    RGB = (GetColorBytes(m.Color)[0], 
                           GetColorBytes(m.Color)[1], 
                           GetColorBytes(m.Color)[2]),
                }).ToArray();

            return inputQuery;
        }

        public static async Task<byte[]> GetBytesFromCadAsync(IFormFile cad)
        {
            using MemoryStream memoryStream = new();
            await cad.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            return fileBytes;
        }

        public static byte[] GetColorBytes(Color color)
        {
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;
            byte a = color.A;

            return new byte[] { r, g, b, a };
        }

        public static byte[] CombineBytes(byte[] bytes1, byte[] bytes2)
        {
            byte[] combinedBytes = new byte[bytes1.Length + bytes2.Length];
            Buffer.BlockCopy(bytes1, 0, combinedBytes, 0, bytes1.Length);
            Buffer.BlockCopy(bytes2, 0, combinedBytes, bytes1.Length, bytes2.Length);
            return combinedBytes;
        }

        public static byte[] CombineBytes(byte[] bytes1, Color color)
        {
            byte[] bytes2 = GetColorBytes(color);
            byte[] combinedBytes = new byte[bytes1.Length + bytes2.Length];
            Buffer.BlockCopy(bytes1, 0, combinedBytes, 0, bytes1.Length);
            Buffer.BlockCopy(bytes2, 0, combinedBytes, bytes1.Length, bytes2.Length);
            return combinedBytes;
        }

        public static byte[] GenerateObjWithColor(byte[] bytes1, Color color)
        {
            byte[] bytes2 = GetColorBytes(color);
            byte[] combinedBytes = new byte[bytes1.Length + bytes2.Length];
            Buffer.BlockCopy(bytes1, 0, combinedBytes, 0, bytes1.Length);
            Buffer.BlockCopy(bytes2, 0, combinedBytes, bytes1.Length, bytes2.Length);
            return combinedBytes;
        }
    }
}
