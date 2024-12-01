using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day01 : DayBase
{
    private readonly List<int> Locations1 = [];
    private readonly List<int> Locations2 = [];

    [Benchmark]
    public override void ParseData()
    {

        var parser = new Parser(Contents
        //"""
        //3   4
        //4   3
        //2   5
        //1   3
        //3   9
        //3   3
        //"""u8
        );

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

    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
