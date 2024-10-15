namespace CustomCADs.API.Endpoints.Home.Gallery;

public class GalleryResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ImagePath { get; set; }
    public required string CreatorName { get; set; }
    public required string CreationDate { get; set; }
}
