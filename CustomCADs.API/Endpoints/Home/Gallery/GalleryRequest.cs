namespace CustomCADs.API.Endpoints.Home.Gallery
{
    public class GalleryRequest
    {
        public string? Sorting { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Creator { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 20;
    }
}
