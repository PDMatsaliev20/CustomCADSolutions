using CustomCADs.Domain.Entities.Enums;

namespace CustomCADs.Core.Models.Cads
{
    public class CadQuery
    {
        public string? Creator { get; set; }
        public CadStatus? Status { get; set; }
    }
}
