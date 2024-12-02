using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day02 : DayBase
{
    /* 
     * Measured performance:
     * 
     * | Method    | Mean     | Error    | StdDev   |
     * |---------- |---------:|---------:|---------:|
     * | ParseData | 98.22 us | 1.122 us | 1.645 us |
     * | Solve1    | 14.56 us | 0.061 us | 0.090 us |
     * | Solve2    | 26.36 us | 0.109 us | 0.160 us |
     */

    private readonly List<List<int>> Reports = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);

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
            if (levels.Count < 2 || IsSafe(levels, 0, 1))
            {
                ++count;
            }
        }

        return count.ToString();
    }

    private static bool IsSafe(List<int> levels, int i0, int i1, out int diff)
    {
        diff = levels[i1] - levels[i0];
        return diff switch
        {
            < -3 => false,// Decreased too much
            <= -1 => AllDecreasingSafely(levels, start: i1),
            0 => false,// Neither increasing nor decreasing
            <= 3 => AllIncreasingSafely(levels, start: i1),
            // > 3
            _ => false,// Increased too much
        };
    }

    private static bool IsSafe(List<int> levels, int i0, int i1) => IsSafe(levels, i0, i1, out _);

    private static bool AllDecreasingSafely(List<int> levels, int start = 1, int ignoreIndex = int.MaxValue)
    {
        int end = levels.Count - 1;
        if (ignoreIndex == end)
        {
            --end;
        }

        for (int i = start; i < end; i++)
        {
            int diff;
            if (i + 1 == ignoreIndex)
            {
                diff = levels[i + 2] - levels[i];
                i++;
            }
            else
            {
                diff = levels[i + 1] - levels[i];
            }

            if (diff is < -3 or > -1) return false;
        }

        return true;
    }

    private static bool AllIncreasingSafely(List<int> levels, int start = 1, int ignoreIndex = int.MaxValue)
    {
        int end = levels.Count - 1;
        if (ignoreIndex == end)
        {
            --end;
        }

        for (int i = start; i < end; i++)
        {
            int diff;
            if (i + 1 == ignoreIndex)
            {
                diff = levels[i + 2] - levels[i];
                i++;
            }
            else
            {
                diff = levels[i + 1] - levels[i];
            }

            if (diff is < 1 or > 3) return false;
        }

        return true;
    }

    [Benchmark]
    public override string Solve2()
    {
        var count = 0;
        foreach (var levels in Reports)
        {
            if (levels.Count < 2)
            {
                ++count;
                continue;
            }

            // Try without removing
            if (IsSafe(levels, 0, 1, out int diff0))
            {
                ++count;
                continue;
            }

            // Try without first
            if (levels.Count == 2)
            {
                ++count;
                continue;
            }

            if (IsSafe(levels, 0, 2))
            {
                ++count;
                continue;
            }

            // Try without second
            // Count == 2 case already handled

            if (IsSafe(levels, 1, 2))
            {
                ++count;
                continue;
            }

            // Try without each remaining index
            // Direction is always matching levels[1]-levels[0] as those two are now always included
            switch (diff0)
            {
                case < -3:
                    // Decreased too much
                    break;
                case <= -1:
                    if (AllDecreasingSafelyWithoutOne(levels))
                    {
                        ++count;
                        continue;
                    }
                    break;
                case 0:
                    // Neither increasing nor decreasing
                    break;
                case <= 3:
                    if (AllIncreasingSafelyWithoutOne(levels))
                    {
                        ++count;
                        continue;
                    }
                    break;
                default: // > 3
                    // Increased too much
                    break;
            }
        }

        return count.ToString();
    }

    private static bool AllDecreasingSafelyWithoutOne(List<int> levels)
    {
        for (int i = 2; i < levels.Count; i++)
        {
            if (AllDecreasingSafely(levels, ignoreIndex: i))
            {
                return true;
            }
        }

        return false;
    }

    private static bool AllIncreasingSafelyWithoutOne(List<int> levels)
    {
        for (int i = 2; i < levels.Count; i++)
        {
            if (AllIncreasingSafely(levels, ignoreIndex: i))
            {
                return true;
            }
        }

        return false;
    }
}
