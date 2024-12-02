using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day01 : DayBase
{
    /* 
     * Measured performance:
     * 
     * | Method    | Mean      | Error    | StdDev   |
     * |---------- |----------:|---------:|---------:|
     * | ParseData | 120.51 us | 0.709 us | 0.994 us |
     * | Solve1    |  19.19 us | 0.120 us | 0.173 us |
     * | Solve2    |  92.04 us | 1.520 us | 2.229 us |
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
        var counts = Locations2.GroupBy(loc => loc, (loc, duplicates) => new KeyValuePair<int, int>(loc, duplicates.Count())).ToDictionary();
        return Locations1.Sum(loc1 => loc1 * counts.GetValueOrDefault(loc1)).ToString();
    }
}
