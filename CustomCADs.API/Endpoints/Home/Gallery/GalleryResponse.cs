namespace CustomCADs.API.Endpoints.Home.Gallery
{
    public class GalleryResponse
    {
        public int Count { get; set; }
        public ICollection<GalleryCadDto> Cads { get; set; } = [];
    }
}
