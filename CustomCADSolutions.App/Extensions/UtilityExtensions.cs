using CustomCADSolutions.App.Models.Cads;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using Stripe;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using CustomCADSolutions.App.Models.Orders;
using System.Net.Http;
using System.Text.Json;
using System.Text;

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
                    RGB = m.Color.GetColorBytes(),
                }).ToArray();

            return inputQuery;
        }

        public static async Task<byte[]> GetBytesAsync(this IFormFile cad)
        {
            using MemoryStream memoryStream = new();
            await cad.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            return fileBytes;
        }

        public static (byte, byte, byte) GetColorBytes(this Color color)
            => (color.R, color.G, color.B );

        public static bool TryGet<T>(this HttpClient httpClient, string path, out T? result) 
        {
            var response = httpClient.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStream();
                result = JsonSerializer.Deserialize<T>(stream);
                return result != null;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public static bool TryPost<TInput, T>(this HttpClient httpClient, string path, TInput input, out T? result)
        {
            string json = JsonSerializer.Serialize(input);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(path, content).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                Stream stream = response.Content.ReadAsStream();
                result = JsonSerializer.Deserialize<T>(stream);
                return result != null;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}
