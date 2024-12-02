using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day01 : DayBase
{
    /* 
     * Measured performance:
     * 
     * | Method    | Mean        | Error     | StdDev    |
     * |---------- |------------:|----------:|----------:|
     * | ParseData |   120.04 us |  0.171 us |  0.250 us |
     * | Solve1    |    20.02 us |  0.107 us |  0.160 us |
     * | Solve2    | 7,389.06 us | 66.119 us | 98.964 us |
     */

    private readonly List<int> Locations1 = [];
    private readonly List<int> Locations2 = [];

    [Benchmark]
    public override void ParseData()
    {

        var parser = new Parser(Contents);

        Locations1.Clear();
        Locations2.Clear();

        while (!parser.IsEmpty)
        {
            Locations1.Add(parser.ParsePosInt());
            parser.SkipWhitespace(); // Spaces
            Locations2.Add(parser.ParsePosInt());
            parser.SkipWhitespace(); // Newline
        }

        Locations1.Sort();
        Locations2.Sort();
    }

    [Benchmark]
    public override string Solve1()
    {
        var total = 0;

        foreach (var (loc1, loc2) in Enumerable.Zip(Locations1, Locations2))
        {
            total += Math.Abs(loc2 - loc1);
        }

        return total.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        return Locations1.Sum(loc1 => loc1 * Locations2.Count(loc2 => loc1 == loc2)).ToString();
    }
}
