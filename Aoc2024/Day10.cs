namespace Aoc2024;

public partial class Day10 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean      | Error    | StdDev   |
     * |---------- |----------:|---------:|---------:|
     * | ParseData |  10.58 us | 0.208 us | 0.312 us |
     * | Solve1    | 328.62 us | 1.673 us | 2.345 us |
     * | Solve2    | 383.71 us | 1.614 us | 2.314 us |
     */

    private readonly List<List<byte>> Map = [];
    private readonly List<VectorI2d> Trailheads = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);
        Map.Clear();
        Trailheads.Clear();

        int y = 0;
        while (!parser.IsEmpty && parser.ParseLine() is var lineParser)
        {
            int x = 0;
            List<byte> row = [];

            while (lineParser.TryParseDigit(out var height))
            {
                row.Add((byte)height);
                if (height == 0)
                {
                    Trailheads.Add(new(x, y));
                }
                ++x;
            }

            Map.Add(row);
            ++y;
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        var total = 0;
        var visited = Map.ConvertAll(row => new bool[row.Count]);
        var stack = new Stack<VectorI2d>();

        foreach (var head in Trailheads)
        {
            visited.Fill2d(false);
            visited.RefAt(head) = true;

            var nextHeight = Map.At(head) + 1;

            foreach (var (adjacentPos, height) in Map.Adjacent4(head))
            {
                if (height == nextHeight)
                {
                    stack.Push(adjacentPos);
                    visited.RefAt(adjacentPos) = true;
                }
            }

            while (stack.TryPop(out var pos))
            {
                nextHeight = Map.At(pos) + 1;
                foreach (var (adjacentPos, height) in Map.Adjacent4(pos))
                {
                    if (height == nextHeight && visited.MarkAt(adjacentPos))
                    {
                        if (nextHeight == 9)
                        {
                            ++total;
                        }
                        else
                        {
                            stack.Push(adjacentPos);
                        }
                    }
                }
            }
        }

        return total.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        var total = 0;
        var stack = new Stack<VectorI2d>();

        foreach (var head in Trailheads)
        {
            var nextHeight = Map.At(head) + 1;

            foreach (var (adjacentPos, height) in Map.Adjacent4(head))
            {
                if (height == nextHeight)
                {
                    stack.Push(adjacentPos);
                }
            }

            while (stack.TryPop(out var pos))
            {
                nextHeight = Map.At(pos) + 1;
                foreach (var (adjacentPos, height) in Map.Adjacent4(pos))
                {
                    if (height == nextHeight)
                    {
                        if (nextHeight == 9)
                        {
                            ++total;
                        }
                        else
                        {
                            stack.Push(adjacentPos);
                        }
                    }
                }
            }
        }

        return total.ToString();
    }
}
