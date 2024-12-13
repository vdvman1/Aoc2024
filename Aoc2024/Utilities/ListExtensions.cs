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
}
