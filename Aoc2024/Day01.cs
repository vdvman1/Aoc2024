using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day01 : DayBase
{
    /* 
     * Measured performance:
     * 
     * | Method    | Mean     | Error    | StdDev   |
     * |---------- |---------:|---------:|---------:|
     * | ParseData | 97.74 us | 0.326 us | 0.446 us |
     * | Solve1    | 17.20 us | 0.109 us | 0.156 us |
     * | Solve2    | 30.51 us | 0.182 us | 0.267 us |
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
        var counts = new Dictionary<int, int>(capacity: Locations2.Count);
        foreach (var loc in Locations2)
        {
            counts.TryGetValue(loc, out var count);
            counts[loc] = count + 1;
        }

        return Locations1.Sum(loc1 => loc1 * counts.GetValueOrDefault(loc1)).ToString();
    }
}
