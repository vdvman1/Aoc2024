namespace Aoc2024;

public partial class Day10 : DayBase
{
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
        throw new NotImplementedException();
    }
}
