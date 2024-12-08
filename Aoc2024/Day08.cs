namespace Aoc2024;

public partial class Day08 : DayBase
{
    private readonly Dictionary<byte, List<VectorI2d>> Antennas = [];
    private int Width = 0;
    private int Height = 0;

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents
        //"""
        //............
        //........0...
        //.....0......
        //.......0....
        //....0.......
        //......A.....
        //............
        //............
        //........A...
        //.........A..
        //............
        //............
        //"""u8
        );
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

                    var antinode = loc2 + diff;
                    if ((uint)antinode.X < (uint)Width && (uint)antinode.Y < (uint)Height)
                    {
                        ref var existing = ref seen[antinode.Y, antinode.X];
                        if (!existing)
                        {
                            existing = true;
                            ++count;
                        }
                    }

                    antinode = loc1 - diff;
                    if ((uint)antinode.X < (uint)Width && (uint)antinode.Y < (uint)Height)
                    {
                        ref var existing = ref seen[antinode.Y, antinode.X];
                        if (!existing)
                        {
                            existing = true;
                            ++count;
                        }
                    }
                }
            }
        }
        return count.ToString();
    }

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
