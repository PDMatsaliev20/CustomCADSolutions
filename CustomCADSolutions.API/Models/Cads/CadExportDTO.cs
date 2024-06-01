namespace CustomCADSolutions.API.Models.Cads
{
    public class CadExportDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CreatorId { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string CreationDate { get; set; } = null!;
        public bool IsValidated { get; set; }
        public decimal Price { get; set; }
        public string Path { get; set; } = null!;
        public int[] Coords { get; set; } = new int[3];
        public int Fov { get; set; } = 90;
    }
}
