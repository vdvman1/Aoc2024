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
