namespace CustomCADs.Domain.ValueObjects;

public class Paths
{
    public string ImagePath { get; } = string.Empty;
    public string FilePath { get; } = string.Empty;
    public string ImageExtension => GetExtension(ImagePath);
    public string FileExtension => GetExtension(FilePath);

    private static string GetExtension(string path) => '.' + path.Split('.')[^1].ToLower();

    public Paths() 
    {
        ImagePath = "";
        FilePath = ""; 
    }

    public Paths(string file, string image)
    {
        ImagePath = image;
        FilePath = file;
    }
}
