namespace CustomCADs.API.Dtos
{
    public class OrderResultDto<TOrder> where TOrder : class
    {
        public int Count { get; set; }
        public ICollection<TOrder> Orders { get; set; } = [];
    }
}
