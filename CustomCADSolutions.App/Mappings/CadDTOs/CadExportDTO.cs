namespace CustomCADSolutions.App.Mappings.CadDTOs
{
    public class CadExportDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;  
        public string? CreatorId { get; set; }
        public string? CreatorName { get; set; } 
        public string? CreationDate { get; set; } 
        public byte[]? Bytes { get; set; } 
        public bool IsValidated { get; set; }
        public int[] Coords { get; set; } = new int[3];
        public char? SpinAxis { get; set; }
        public byte[] RGB { get; set; } = new byte[3];
    }
}
