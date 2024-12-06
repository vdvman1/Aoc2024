namespace Aoc2024.Utilities;

public record struct VectorI2d(int X, int Y)
{
    public static readonly VectorI2d UP = new(0, -1);
    public static readonly VectorI2d LEFT = new(-1, 0);
    public static readonly VectorI2d RIGHT = new(1, 0);
    public static readonly VectorI2d DOWN = new(0, 1);

    public static VectorI2d operator +(VectorI2d a, VectorI2d b) => new(a.X + b.X, a.Y + b.Y);

    public readonly VectorI2d Rotated90CW()
    {
        // Since +y is down, positive angles rotate CW
        // x = x*cos(pi/2) - y*sin(pi/2)
        // y = x*sin(pi/2) + y*cos(pi/2)
        // cos(pi/2) = 0
        // sin(pi/2) = 1
        // x = x*0 - y*1
        // y = x*1 + y*0
        // x = -y
        // y = x
        return new(-Y, X);
    }
}