namespace CustomCADs.API.Models.Queries
{
    public class CadQueryDTO
    {
        public string? SearchName { get; set; }
        public string? SearchCreator { get; set; }
        public string? Category { get; set; }
        public string? Sorting { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CadsPerPage { get; set; } = 30;
        public string? Creator { get; set; }
    }
}
