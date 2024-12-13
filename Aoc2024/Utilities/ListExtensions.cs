namespace Aoc2024.Utilities;

public static class ListExtensions
{
    public static void CopyTo2d<T>(this List<T[]> source, List<T[]> destination)
    {
        for (int i = 0; i < source.Count; i++)
        {
            source[i].CopyTo(destination[i], 0);
        }
    }

    public static void Fill2d<T>(this List<T[]> grid, T value)
    {
        foreach (var row in grid)
        {
            Array.Fill(row, value);
        }
    }

    public static ref T RefAt<T>(this List<T[]> grid, VectorI2d index) => ref grid[index.Y][index.X];

    public static T At<T>(this List<T[]> grid, VectorI2d index) => grid[index.Y][index.X];

    public static T At<T>(this List<List<T>> grid, VectorI2d index) => grid[index.Y][index.X];

    public static ref T RefAt<T>(this T[,] grid, VectorI2d index) => ref grid[index.Y, index.X];

    public static IEnumerable<KeyValuePair<VectorI2d, T>> Adjacent4<T>(this List<List<T>> grid, VectorI2d center)
    {
        var index = center + VectorI2d.UP;
        if (index.Y >= 0)
        {
            yield return KeyValuePair.Create(index, grid.At(index));
        }

        index = center + VectorI2d.DOWN;
        if (index.Y < grid.Count)
        {
            yield return KeyValuePair.Create(index, grid.At(index));
        }

        var row = grid[center.Y];
        index = center + VectorI2d.LEFT;
        if (index.X >= 0)
        {
            yield return KeyValuePair.Create(index, row[index.X]);
        }

        index = center + VectorI2d.RIGHT;
        if (index.X < row.Count)
        {
            yield return KeyValuePair.Create(index, row[index.X]);
        }
    }

    public static IEnumerable<KeyValuePair<VectorI2d, T>> Adjacent4<T>(this List<T[]> grid, VectorI2d center)
    {
        var index = center + VectorI2d.UP;
        if (index.Y >= 0)
        {
            yield return KeyValuePair.Create(index, grid.At(index));
        }

        index = center + VectorI2d.DOWN;
        if (index.Y < grid.Count)
        {
            yield return KeyValuePair.Create(index, grid.At(index));
        }

        var row = grid[center.Y];
        index = center + VectorI2d.LEFT;
        if (index.X >= 0)
        {
            yield return KeyValuePair.Create(index, row[index.X]);
        }

        index = center + VectorI2d.RIGHT;
        if (index.X < row.Length)
        {
            yield return KeyValuePair.Create(index, row[index.X]);
        }
    }

    public static bool MarkAt(this List<bool[]> grid, VectorI2d pos)
    {
        ref var marked = ref grid.RefAt(pos);
        if (marked) return false;

        marked = true;
        return true;
    }

    public static bool MarkAt(this bool[,] grid, VectorI2d pos)
    {
        ref var marked = ref grid.RefAt(pos);
        if (marked) return false;

        marked = true;
        return true;
    }
}
