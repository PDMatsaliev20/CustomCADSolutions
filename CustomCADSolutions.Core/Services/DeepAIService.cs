using CustomCADSolutions.Core.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace CustomCADSolutions.Core.Services
{
    public class DeepAIService : IDeepAIService
    {
        private readonly string apiKey = "9ae52d36-ef65-43ff-ae4e-920c6b74a455";
        private readonly string apiUrl = "https://api.deepai.org/api/text2img";

        public async Task<string> GenerateImage(string description)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            var content = new StringContent(JsonConvert.SerializeObject(new { text = description, }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Request failed: {response.StatusCode} {error}");
                throw new HttpRequestException();
            }

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody; // The response should be parsed as per your requirement
        }
    }
}
