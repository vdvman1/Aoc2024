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

    public static int DigitCount(this long value)
    {
        int digits = 1;
        long pow10 = 10;

        while (pow10 <= value)
        {
            ++digits;
            pow10 *= 10;
        }

        return digits;
    }

    public static long Pow(this long value, int exponent)
    {
        long res = 1;
        while (exponent != 0)
        {
            if ((exponent & 1) == 1)
            {
                res *= value;
            }

            value *= value;
            exponent >>= 1;
        }

        return res;
    }

    public static long SumRange(long start, long count) => count * (2 * start + count - 1) / 2;
}
