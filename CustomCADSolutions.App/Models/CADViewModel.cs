namespace CustomCADSolutions.App.Models
{
    public class CadViewModel
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public IFormFile Cad { get; set; } = null!;
        public string? CreationDate { get; set; }
    }
}
