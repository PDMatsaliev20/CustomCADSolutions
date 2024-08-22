namespace CustomCADs.API.Models.Cads
{
    public class CadCountsDTO(int @unchecked, int validated, int reported, int banned)
    {
        public int Unchecked { get; set; } = @unchecked;
        public int Validated { get; set; } = validated;
        public int Reported { get; set; } = reported;
        public int Banned { get; set; } = banned;
    }
}
