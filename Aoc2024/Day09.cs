namespace Aoc2024;

public partial class Day09 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method | Mean         | Error     | StdDev    |
     * |------- |-------------:|----------:|----------:|
     * | Solve1 |     120.9 us |   0.21 us |   0.29 us |
     * | Solve2 | 103,313.5 us | 307.59 us | 460.39 us |
     */

    public override void ParseData()
    {
        // No common parsing
    }

    [Benchmark]
    public override string Solve1()
    {
        var fs = Contents;
        (long endID, int rem) = Math.DivRem(fs.Length, 2);
        int endIndex;
        if (rem == 0)
        {
            --endID;
            endIndex = fs.Length - 2;
        }
        else
        {
            endIndex = fs.Length - 1;
        }
        int endCount = fs[endIndex] - '0';
        long checksum = 0;
        int startIndex = 0;
        long startID = 0;
        long compactedIndex = 0;

        while (startIndex <= endIndex)
        {
            for (int blockSize = startID == endID ? endCount : fs[startIndex] - '0'; blockSize > 0; --blockSize)
            {
                checksum += startID * compactedIndex;
                ++compactedIndex;
            }

            ++startIndex;
            if (startIndex > endIndex)
            {
                break;
            }

            for (int emptySize = fs[startIndex] - '0'; emptySize > 0 && endIndex > startIndex; --emptySize)
            {
                checksum += endID * compactedIndex;
                ++compactedIndex;
                --endCount;
                if (endCount == 0)
                {
                    --endID;
                    endIndex -= 2; // Skip over blank space at end
                    endCount = fs[endIndex] - '0';
                }
            }

            ++startIndex;
            ++startID;
        }

        return checksum.ToString();
    }

    private record struct BlockRange(int ID, int Length)
    {
        public const int EMPTY_ID = -1;
    }

    [Benchmark]
    public override string Solve2()
    {
        var unsortedFs = Contents;

        // Collect the full list of blocks and their original positions/ranges
        var sortedFs = new List<BlockRange>(unsortedFs.Length * 2); // Hopefully an overestimate
        for (int i = 0; i < unsortedFs.Length; ++i)
        {
            sortedFs.Add(new(i / 2, unsortedFs[i] - '0'));
            ++i;
            if (i < unsortedFs.Length)
            {
                sortedFs.Add(new(BlockRange.EMPTY_ID, unsortedFs[i] - '0'));
            }
        }

        // The final block ID can be calculated and doesn't need to be fetched from the list
        var (endID, rem) = Math.DivRem(unsortedFs.Length, 2);
        if (rem == 0)
        {
            --endID;
        }

        // Loop from the last block and work backwards
        while (endID >= 0)
        {
            int originalIndex = sortedFs.Count - 1;
            var block = sortedFs[originalIndex];
            while (block.ID != endID)
            {
                --originalIndex;
                block = sortedFs[originalIndex];
            }

            int goalIndex = 0;
            var goalRange = sortedFs[goalIndex];
            while (goalIndex < originalIndex && (goalRange.ID != BlockRange.EMPTY_ID || goalRange.Length < block.Length))
            {
                ++goalIndex;
                goalRange = sortedFs[goalIndex];
            }

            if (goalIndex < originalIndex) // Found an empty block to use
            {
                sortedFs[goalIndex] = block;
                // Ensure remaining space in empty block isn't lost
                if (block.Length < goalRange.Length)
                {
                    sortedFs.Insert(goalIndex + 1, new(BlockRange.EMPTY_ID, goalRange.Length - block.Length));
                    ++originalIndex;
                }

                // Merge potentially newly adjacent empty blocks at the original index
                if (sortedFs[originalIndex - 1] is (BlockRange.EMPTY_ID, var range0))
                {
                    if (originalIndex + 1 < sortedFs.Count && sortedFs[originalIndex + 1] is (BlockRange.EMPTY_ID, var range1))
                    {
                        sortedFs[originalIndex - 1] = new(BlockRange.EMPTY_ID, range0 + block.Length + range1);
                        sortedFs.RemoveRange(originalIndex, 2);
                    }
                    else
                    {
                        sortedFs[originalIndex - 1] = new(BlockRange.EMPTY_ID, range0 + block.Length);
                        sortedFs.RemoveAt(originalIndex);
                    }
                }
                else if (originalIndex + 1 < sortedFs.Count && sortedFs[originalIndex + 1] is (BlockRange.EMPTY_ID, var range))
                {
                    sortedFs[originalIndex] = new(BlockRange.EMPTY_ID, block.Length + range);
                    sortedFs.RemoveAt(originalIndex + 1);
                }
                else
                {
                    sortedFs[originalIndex] = new(BlockRange.EMPTY_ID, block.Length);
                }
            }

            --endID;
        }

        long checksum = 0;
        long fsIndex = 0;
        foreach (var block in sortedFs)
        {
            var (id, range) = block;

            if (id == BlockRange.EMPTY_ID)
            {
                fsIndex += range;
                continue;
            }

            while (range > 0)
            {
                checksum += id * fsIndex;
                ++fsIndex;
                --range;
            }
        }
        return checksum.ToString();
    }
}
