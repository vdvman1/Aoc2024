namespace Aoc2024.Utilities;

public static class MathPlus
{
    /// <summary>
    /// Calculate the greatest common divisor of two signed integers
    /// </summary>
    /// <remarks>
    /// Based on the pseudocode at https://en.wikipedia.org/wiki/Euclidean_algorithm#Implementations
    /// </remarks>
    /// <returns>The positive greatest common divisor</returns>
    public static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }

        return Math.Abs(a);
    }

    public static long NextPow10(this int value) => NextPow10((long)value);

    public static long NextPow10(this long value)
    {
        long res = 1;

        while (res <= value)
        {
            res *= 10;
        }

        return res;
    }
}
