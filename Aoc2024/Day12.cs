namespace Aoc2024;

public partial class Day12 : DayBase
{
    private readonly List<byte[]> Farm = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents
        //"""
        //RRRRIICCFF
        //RRRRIICCCF
        //VVRRRCCFFF
        //VVRCCCJFFF
        //VVVVCJJCFE
        //VVIVCCJJEE
        //VVIIICJJEE
        //MIIIIIJJEE
        //MIIISIJEEE
        //MMMISSJEEE
        //"""u8
        );
        Farm.Clear();

        while (!parser.IsEmpty && parser.ParseLine().Remainder is var line)
        {
            if (line.Length == 0) continue;

            if (line[^1] == (byte)'\r')
            {
                line = line[..^1];
            }

            Farm.Add(line.ToArray());
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        long cost = 0;
        var visited = Farm.ConvertAll(row => new bool[row.Length]);
        for (int y = 0; y < Farm.Count; ++y)
        {
            var row = Farm[y];
            for (int x = 0; x < row.Length; x++)
            {
                var pos = new VectorI2d(x, y);
                if (visited.MarkAt(pos))
                {
                    var (perimeter, area) = FindRegion(visited, pos, Farm.At(pos));
                    cost += perimeter * area;
                }
            }
        }

        return cost.ToString();
    }

    private (int perimeter, int area) FindRegion(List<bool[]> visited, VectorI2d pos, byte type)
    {
        int perimeter = 0;
        int area = 1;
        int checkedSides = 0;

        foreach (var (adjacentPos, adjacentType) in Farm.Adjacent4(pos))
        {
            ++checkedSides;

            if (adjacentType == type)
            {
                if (visited.MarkAt(adjacentPos))
                {
                    var (nextPerimeter, nextArea) = FindRegion(visited, adjacentPos, type);
                    perimeter += nextPerimeter;
                    area += nextArea;
                }
            }
            else
            {
                perimeter++;
            }
        }

        // Account for edges of the farm
        perimeter += 4 - checkedSides;

        return (perimeter, area);
    }

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
