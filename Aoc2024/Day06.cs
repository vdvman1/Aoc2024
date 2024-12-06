using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day06 : DayBase
{
    private enum Cell : byte
    {
        Empty,
        Obstacle,
        GuardVisited
    }

    private readonly List<List<Cell>> Map = [];
    private VectorI2d GuardStartPos = new();
    private VectorI2d GuardStartDirection = new();

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents
        //"""
        //....#.....
        //.........#
        //..........
        //..#.......
        //.......#..
        //..........
        //.#..^.....
        //........#.
        //#.........
        //......#...
        //"""u8
        );
        Map.Clear();

        int y = 0;
        while (!parser.IsEmpty && parser.ParseLine() is var lineParser)
        {
            Map.Add(ParseRow(y, ref lineParser));
            ++y;
        }
    }

    private List<Cell> ParseRow(int y, ref Parser lineParser)
    {
        var row = new List<Cell>();

        for (int x = 0; ; x++)
        {
            switch (lineParser.ParseOne())
            {
                case (byte)'.':
                    row.Add(Cell.Empty);
                    break;
                case (byte)'#':
                    row.Add(Cell.Obstacle);
                    break;
                case (byte)'^':
                    row.Add(Cell.GuardVisited);
                    GuardStartPos = new(x, y);
                    GuardStartDirection = VectorI2d.UP;
                    break;
                case (byte)'>':
                    row.Add(Cell.GuardVisited);
                    GuardStartPos = new(x, y);
                    GuardStartDirection = VectorI2d.RIGHT;
                    break;
                case (byte)'v':
                    row.Add(Cell.GuardVisited);
                    GuardStartPos = new(x, y);
                    GuardStartDirection = VectorI2d.DOWN;
                    break;
                case (byte)'<':
                    row.Add(Cell.GuardVisited);
                    GuardStartPos = new(x, y);
                    GuardStartDirection = VectorI2d.LEFT;
                    break;
                case (byte)0:
                case (byte)'\r':
                    return row;
            }
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        var count = 1; // Include the guard starting position
        var direction = GuardStartDirection;
        var pos = GuardStartPos;
        var nextPos = pos + direction;
        while (0 <= nextPos.Y && nextPos.Y < Map.Count && 0 <= nextPos.X && Map[nextPos.Y] is var row && nextPos.X < row.Count)
        {
            switch (row[nextPos.X])
            {
                case Cell.Obstacle:
                    direction = direction.Rotated90CW();
                    nextPos = pos + direction;
                    break;
                case Cell.Empty:
                    row[nextPos.X] = Cell.GuardVisited;
                    ++count;
                    goto case Cell.GuardVisited;
                case Cell.GuardVisited:
                    pos = nextPos;
                    nextPos = nextPos + direction;
                    break;
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
