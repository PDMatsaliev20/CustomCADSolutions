namespace CustomCADSolutions.App.Models.Cads
{
    public class CadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public byte[] Cad { get; set; } = null!;
        public bool IsValidated { get; internal set; }
        public string? CreationDate { get; set; }
        public (short, short, short) Coords { get; set; }
        public char? SpinAxis { get; set; }
        public short Fov { get; set; } = 90;
        public string? TexturePath { get; set; } = "/textures/texture5.jpg";
    }
}
