using System.ComponentModel.DataAnnotations;

namespace CustomCADs.Core.Models.Orders
{
    public class OrderPagination
    {
        [Range(1, int.MaxValue)]
        public int CurrentPage { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int CadsPerPage { get; set; } = 12;
    }
}
