using static CustomCADs.Domain.Cads.CadConstants;

namespace CustomCADs.Domain.Cads.ValueObjects;

public class Coordinates
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Coordinates() { }

    public Coordinates(double x, double y, double z)
    {
        static bool RangeCheck(double coord) => coord > CoordMin && coord < CoordMax;

        if (RangeCheck(x) && RangeCheck(y) && RangeCheck(z))
        {
            X = x;
            Y = y;
            Z = z;
        }
        else throw new ArgumentOutOfRangeException();
    }

    public bool Equals(Coordinates other)
        => X == other.X && Y == other.Y && Z == other.Z;

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj is Coordinates coords)
        {
            return Equals(coords);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}
