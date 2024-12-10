using System.Runtime.InteropServices;

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

    public static ref T ShiftElement<T>(this List<T> list, int fromIndex, int toIndex)
    {
        var span = CollectionsMarshal.AsSpan(list);
        var elem = span[fromIndex];
        span[toIndex..fromIndex].CopyTo(span[(toIndex + 1)..(fromIndex + 1)]);
        span[toIndex] = elem;
        return ref span[toIndex];
    }
}
