using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class Category
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
