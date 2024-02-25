namespace CustomCADSolutions.App.Models
{
    public class CadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public IFormFile Cad { get; set; } = null!;
        public bool Validated { get; internal set; }
        public string? CreationDate { get; set; }
        public (short, short, short) Coords { get; set; }
        public double SpinFactor { get; set; }
        public char? SpinAxis { get; set; }
    }
}
