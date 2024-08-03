namespace CustomCADs.App.Models.Cads.View
{
    public class CadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Extension { get; set; } = null!;
        public string Path { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string CreationDate { get; set; } = null!;
        public decimal Price { get; set; }
        public double[] Coords { get; set; } = new double[3];
        public double[] PanCoords { get; set; } = new double[3];
        public short Fov { get; set; } = 90;
    }
}
