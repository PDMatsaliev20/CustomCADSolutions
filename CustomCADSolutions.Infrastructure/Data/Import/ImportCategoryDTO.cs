using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Infrastructure.Data.DataProcessor.ImportDtos
{
    public class ImportCategoryDTO
    {
        [JsonProperty("Name")]
        [MinLength(5)]
        [MaxLength(100)]
        public string CategoryName { get; set; } = null!;

        [JsonProperty("CADs")]
        public ICollection<ImportCADModel> CADModels { get; set; } = new List<ImportCADModel>();
    }
}
