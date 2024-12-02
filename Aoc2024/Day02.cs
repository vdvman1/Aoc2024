using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day02 : DayBase
{
    private readonly List<List<int>> Reports = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents
        //"""
        //7 6 4 2 1
        //1 2 7 8 9
        //9 7 6 2 1
        //1 3 2 4 5
        //8 6 4 4 1
        //1 3 6 7 9
        //"""u8
        );

        Reports.Clear();

        while (!parser.IsEmpty)
        {
            var lineParser = parser.ParseLine();
            List<int> levels = [];
            while (!lineParser.IsEmpty)
            {
                levels.Add(lineParser.ParsePosInt());
                lineParser.SkipWhitespace();
            }
            Reports.Add(levels);
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        var count = 0;
        foreach (var levels in Reports)
        {
            if (levels.Count < 2)
            {
                ++count;
                continue;
            }

            var diff = levels[1] - levels[0];
            switch (diff)
            {
                case < -3:
                    // Decreased too much
                    break;
                case <= -1:
                    if (AllDecreasingSafely(levels))
                    {
                        ++count;
                    }
                    break;
                case 0:
                    // Neither increasing nor decreasing
                    break;
                case <= 3:
                    if (AllIncreasingSafely(levels))
                    {
                        ++count;
                    }
                    break;
                default: // > 3
                    // Increased too much
                    break;
            }
        }

        return count.ToString();
    }

    private static bool AllDecreasingSafely(List<int> levels)
    {
        for (int i = 1; i < levels.Count - 1; i++)
        {
            var diff = levels[i + 1] - levels[i];
            if (diff is < -3 or > -1) return false;
        }

        return true;
    }

    private static bool AllIncreasingSafely(List<int> levels)
    {
        for (int i = 1; i < levels.Count - 1; i++)
        {
            var diff = levels[i + 1] - levels[i];
            if (diff is < 1 or > 3) return false;
        }

        return true;
    }

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
