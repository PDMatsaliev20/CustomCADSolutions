namespace CustomCADSolutions.Core.Models
{
    public record struct Coords(short X, short Y, short Z)
    {
        public static implicit operator (short X, short Y, short Z)(Coords value)
        {
            return (value.X, value.Y, value.Z);
        }

        public static implicit operator Coords((short X, short Y, short Z) value)
        {
            return new Coords(value.X, value.Y, value.Z);
        }
    }
}
