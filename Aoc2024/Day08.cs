namespace Aoc2024;

public partial class Day08 : DayBase
{
    private readonly Dictionary<byte, List<VectorI2d>> Antennas = [];
    private int Width = 0;
    private int Height = 0;

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);
        Antennas.Clear();

        var line0Parser = parser.ParseLineProper();
        Width = line0Parser.RemainingLength;
        Height = 0;
        ParseLine(line0Parser);

        while (!parser.IsEmpty)
        {
            ++Height;
            ParseLine(parser.ParseLineProper());
        }

        ++Height;
    }

    private void ParseLine(Parser lineParser)
    {
        for (int x = 0; x < Width; x++)
        {
            var freq = lineParser.ParseOne();
            if (freq != (byte)'.')
            {
                if (!Antennas.TryGetValue(freq, out var locations))
                {
                    locations = [];
                    Antennas[freq] = locations;
                }

                locations.Add(new(x, Height));
            }
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        var seen = new bool[Height, Width];
        var count = 0;
        foreach (var locations in Antennas.Values)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                var loc1 = locations[i];
                for (int j = i + 1; j < locations.Count; j++)
                {
                    var loc2 = locations[j];
                    var diff = loc2 - loc1;

                    count += CountAntinode(seen, loc2 + diff) + CountAntinode(seen, loc1 - diff);
                }
            }
        }
        return count.ToString();
    }

    private int CountAntinode(bool[,] seen, VectorI2d pos) => CountAntinode(seen, pos, out _);

    private int CountAntinode(bool[,] seen, VectorI2d pos, out bool outOfBounds)
    {
        if ((uint)pos.X < (uint)Width && (uint)pos.Y < (uint)Height)
        {
            outOfBounds = false;
            ref var existing = ref seen[pos.Y, pos.X];
            if (!existing)
            {
                existing = true;
                return 1;
            }
        }
        else
        {
            outOfBounds = true;
        }

        return 0;
    }

    [Benchmark]
    public override string Solve2()
    {
        var seen = new bool[Height, Width];
        var count = 0;
        foreach (var locations in Antennas.Values)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                var loc1 = locations[i];
                for (int j = i + 1; j < locations.Count; j++)
                {
                    var loc2 = locations[j];
                    var diff = (loc2 - loc1).Simplified();

                    var loc = loc2;
                    count += CountAntinode(seen, loc, out bool outOfBounds);
                    do
                    {
                        loc += diff;
                        count += CountAntinode(seen, loc, out outOfBounds);
                    } while (!outOfBounds);

                    loc = loc2 - diff;
                    count += CountAntinode(seen, loc, out outOfBounds);
                    while (!outOfBounds)
                    {
                        loc -= diff;
                        count += CountAntinode(seen, loc, out outOfBounds);
                    }
                }
            }
        }

        return count.ToString();
    }
}
