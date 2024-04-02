namespace CustomCADSolutions.App.Mappings.CadDTOs
{
    public class CadExportDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string? CreatorName { get; set; } = null!;
        public string? CreationDate { get; set; } = null!;
        public bool IsValidated { get; set; }
        public short[] Coords { get; set; } = new short[3];
        public char? SpinAxis { get; set; }
        public int FOV { get; set; }
        public byte[] RGB { get; set; } = new byte[3];
    }
}
