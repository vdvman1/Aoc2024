﻿namespace Aoc2024;

public partial class Day04 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean       | Error     | StdDev    |
     * |---------- |-----------:|----------:|----------:|
     * | ParseData |   4.996 us | 0.2269 us | 0.3397 us |
     * | Solve1    | 157.238 us | 1.6996 us | 2.3265 us |
     * | Solve2    |  97.799 us | 0.4960 us | 0.7270 us |
     */

    private readonly List<byte[]> Grid = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);

        // TODO: Avoid allocating a new array for each line using a 2d array

        Grid.Clear();
        while (!parser.IsEmpty && parser.ParseLine().Remainder is var line && line.Length > 0)
        {
            if (line[^1] == (byte)'\r')
            {
                line = line[..^1];
            }

            Grid.Add(line.ToArray());
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        var count = 0;

        for (int y = 0; y < Grid.Count; y++)
        {
            var row = Grid[y];
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] != (byte)'X') { continue; }

                // Up
                if (y >= 3)
                {
                    var rowM1 = Grid[y - 1];
                    var rowM2 = Grid[y - 2];
                    var rowM3 = Grid[y - 3];

                    // Diagonal up-left
                    if (x >= 3 && IsMas(rowM1[x - 1], rowM2[x - 2], rowM3[x - 3]))
                    {
                        ++count;
                    }

                    // Up
                    if (IsMas(rowM1[x], rowM2[x], rowM3[x]))
                    {
                        ++count;
                    }

                    // Diagonal up-right
                    if (x + 3 < row.Length && IsMas(rowM1[x + 1], rowM2[x + 2], rowM3[x + 3]))
                    {
                        ++count;
                    }
                }

                // Left
                if (x >= 3 && IsMas(row[x - 1], row[x - 2], row[x - 3]))
                {
                    ++count;
                }

                // Down
                if (y + 3 < Grid.Count)
                {
                    var rowP1 = Grid[y + 1];
                    var rowP2 = Grid[y + 2];
                    var rowP3 = Grid[y + 3];

                    // Diagonal down-left
                    if (x >= 3 && IsMas(rowP1[x - 1], rowP2[x - 2], rowP3[x - 3]))
                    {
                        ++count;
                    }

                    // Down
                    if (IsMas(rowP1[x], rowP2[x], rowP3[x]))
                    {
                        ++count;
                    }

                    // Diagonal up-right
                    if (x + 3 < row.Length && IsMas(rowP1[x + 1], rowP2[x + 2], rowP3[x + 3]))
                    {
                        ++count;
                    }
                }

                // Right
                // Intentionally last so the next search position can be adjusted
                if (x + 3 < Grid.Count && IsMas(row[x + 1], row[x + 2], row[x + 3]))
                {
                    ++count;
                    x += 3; // XMAS.Length == 4, for loop adds one, so add 3
                }
            }
        }

        return count.ToString();
    }

    private static bool IsMas(byte b, byte c, byte d) => b == (byte)'M' && c == (byte)'A' && d == (byte)'S';

    [Benchmark]
    public override string Solve2()
    {
        var count = 0;
        // Outer edges can't be the center of an "X-MAS"
        for (int y = 1; y < Grid.Count - 1; y++)
        {
            var row = Grid[y];
            for (int x = 1; x < row.Length - 1; x++)
            {
                if (row[x] != (byte)'A') { continue; }

                var rowM1 = Grid[y - 1];
                var rowP1 = Grid[y + 1];

                switch (rowM1[x - 1])
                {
                    case (byte)'M':
                        if (rowP1[x + 1] != (byte)'S') continue;
                        break;
                    case (byte)'S':
                        if (rowP1[x + 1] != (byte)'M') continue;
                        break;
                    default:
                        continue;
                }

                switch (rowM1[x + 1])
                {
                    case (byte)'M':
                        if (rowP1[x - 1] != (byte)'S') continue;
                        break;
                    case (byte)'S':
                        if (rowP1[x - 1] != (byte)'M') continue;
                        break;
                    default:
                        continue;
                }

                ++count;
            }
        }
        return count.ToString();
    }
}
