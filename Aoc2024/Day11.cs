namespace Aoc2024;

public partial class Day11 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean             | Error          | StdDev         |
     * |---------- |-----------------:|---------------:|---------------:|
     * | ParseData |         27.94 ns |       0.452 ns |       0.648 ns |
     * | Solve1    |    231,254.15 ns |   3,292.179 ns |   4,615.175 ns |
     * | Solve2    | 10,659,883.19 ns | 331,175.815 ns | 485,433.256 ns |
     */

    private readonly List<long> Stones = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);
        Stones.Clear();

        while (!parser.IsEmpty)
        {
            Stones.Add(parser.ParsePosInt());
            parser.MoveNext();
        }
    }

    [Benchmark]
    public override string Solve1() => Solve(25);

    [Benchmark]
    public override string Solve2() => Solve(75);

    private readonly record struct Key(long Stone, int Iterations);

    private string Solve(int iterations)
    {
        Dictionary<Key, long> cache = [];
        long count = 0;

        foreach (var initialStone in Stones)
        {
            count += CountSplits(cache, initialStone, iterations);
        }

        return count.ToString();
    }

    private static long CountSplits(Dictionary<Key, long> cache, long value, int iterations)
    {
        if (iterations == 0) return 1;

        var key = new Key(value, iterations);
        if (cache.TryGetValue(key, out var count)) return count;

        if (value == 0)
        {
            count = CountSplits(cache, 1, iterations - 1);
        }
        else if (Math.DivRem(value.DigitCount(), 2) is (var halfDigits, 0))
        {
            var (left, right) = Math.DivRem(value, 10L.Pow(halfDigits));
            count = CountSplits(cache, left, iterations - 1) + CountSplits(cache, right, iterations - 1);
        }
        else
        {
            count = CountSplits(cache, value * 2024, iterations - 1);
        }

        cache[key] = count;
        return count;
    }
}
