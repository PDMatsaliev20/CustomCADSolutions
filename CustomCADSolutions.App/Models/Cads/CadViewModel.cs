using System.Text.Json.Serialization;

namespace CustomCADSolutions.App.Models.Cads
{
    public class CadViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public bool IsValidated { get; set; }
        public string? CreationDate { get; set; }
        public int[] Coords { get; set; } = new int[3];
        public byte[] RGB { get; set; } = new byte[3];
        public char? SpinAxis { get; set; }
        public short Fov { get; set; } = 90;
        public string? TexturePath { get; set; } = "/textures/texture5.jpg";
    }
}
