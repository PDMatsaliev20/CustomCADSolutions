using System.ComponentModel.DataAnnotations;

namespace CustomCADs.Core.Models
{
    public class PaginationModel
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        
        [Range(0, int.MaxValue)]
        public int Limit { get; set; } = 20;
    }
}
