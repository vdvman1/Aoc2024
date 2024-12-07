using Aoc2024.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Aoc2024;

public partial class Day06 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean        | Error     | StdDev    |
     * |---------- |------------:|----------:|----------:|
     * | ParseData |    101.5 us |   0.66 us |   0.95 us |
     * | Solve1    |    102.5 us |   0.37 us |   0.49 us |
     * | Solve2    | 64,495.8 us | 124.03 us | 177.88 us |
     */

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
        var parser = new Parser(Contents);
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
        // Copy map to avoid mutating original
        var map = Map.ConvertAll(row => row.ToList());

        var count = 1; // Include the guard starting position
        var direction = GuardStartDirection;
        var pos = GuardStartPos;
        var nextPos = pos + direction;
        while (0 <= nextPos.Y && nextPos.Y < map.Count && 0 <= nextPos.X && map[nextPos.Y] is var row && nextPos.X < row.Count)
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
                    nextPos += direction;
                    break;
            }
        }

        return count.ToString();
    }

    [InlineArray(4)]
    private struct CellDirections
    {
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Actually used as the inline array element zero")]
        [SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Not valid for inline array elements")]
        private bool TravelledInDirection;
    }

    [Benchmark]
    public override string Solve2()
    {
        var originalVisitedMap = Map.ConvertAll(row => new CellDirections[row.Count]);
        var pos = GuardStartPos;
        var direction = GuardStartDirection;
        originalVisitedMap[pos.Y][pos.X][direction.GetUnitHash()] = true;

        // Avoid allocating a new map for every attempt
        var attemptVisitedMap = originalVisitedMap.ConvertAll(row => row.ToArray());

        var triedLocations = Map.ConvertAll(row => new bool[row.Count]);

        var count = 0;
        var nextPos = pos + direction;
        while (0 <= nextPos.Y && nextPos.Y < Map.Count && 0 <= nextPos.X && Map[nextPos.Y] is var row && nextPos.X < row.Count)
        {
            var cell = row[nextPos.X];
            // Don't bother attempting to place obstacles where one already exists, and don't attempt at the guard's starting position
            if (cell == Cell.Empty)
            {
                ref bool tried = ref triedLocations[nextPos.Y][nextPos.X];
                if (!tried)
                {
                    tried = true;
                    originalVisitedMap.CopyTo2d(attemptVisitedMap);
                    if (FormsLoop(attemptVisitedMap, pos, direction))
                    {
                        ++count;
                    }
                }
            }

            // Update original map with current path
            if (cell == Cell.Obstacle)
            {
                direction = direction.Rotated90CW();
                nextPos = pos + direction;
            }
            else
            {
                pos = nextPos;
                nextPos += direction;
            }
            originalVisitedMap[pos.Y][pos.X][direction.GetUnitHash()] = true;
        }

        return count.ToString();
    }

    private bool FormsLoop(List<CellDirections[]> visitedMap, VectorI2d pos, VectorI2d direction)
    {
        var nextPos = pos + direction;
        var tempObstaclePos = nextPos;
        while (0 <= nextPos.Y && nextPos.Y < Map.Count && 0 <= nextPos.X && Map[nextPos.Y] is var row && nextPos.X < row.Count)
        {
            ref bool visited = ref visitedMap[nextPos.Y][nextPos.X][direction.GetUnitHash()];
            if (visited)
            {
                return true;
            }

            if (nextPos == tempObstaclePos || row[nextPos.X] == Cell.Obstacle)
            {
                direction = direction.Rotated90CW();
                // Also mark the new direction at the current position as visited
                visitedMap[pos.Y][pos.X][direction.GetUnitHash()] = true;
                nextPos = pos + direction;
            }
            else
            {
                pos = nextPos;
                nextPos += direction;
                // Mark the new position as visited in the current direction
                visited = true;
            }
        }

        return false;
    }
}
