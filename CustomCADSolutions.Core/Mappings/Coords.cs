namespace CustomCADSolutions.Core.Mappings
{
    public record struct Coords(int X, int Y, int Z)
    {
        public static implicit operator (int X, int Y, int Z)(Coords value)
        {
            return (value.X, value.Y, value.Z);
        }

        public static implicit operator Coords((short X, short Y, short Z) value)
        {
            return new Coords(value.X, value.Y, value.Z);
        }
    }
}
