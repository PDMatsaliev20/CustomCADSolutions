using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Models.Cads.View
{
    public class CadsCountModel
    {
        [JsonPropertyName("userCadsCount")]
        public int UserCadsCount { get; set; }

        [JsonPropertyName("unvalidatedCadsCount")]
        public int UnvalidatedCadsCount { get; set; }
    }
}
