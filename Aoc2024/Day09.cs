namespace Aoc2024;

public partial class Day09 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method | Mean         | Error      | StdDev     |
     * |------- |-------------:|-----------:|-----------:|
     * | Solve1 |     66.60 us |   0.291 us |   0.427 us |
     * | Solve2 | 62,217.41 us | 328.575 us | 471.232 us |
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

            int emptySize = fs[startIndex] - '0';
            while (endCount <= emptySize)
            {
                checksum += endID * MathPlus.SumRange(compactedIndex, endCount);
                compactedIndex += endCount;
                emptySize -= endCount;
                --endID;
                endIndex -= 2; // Skip over blank space at end
                endCount = fs[endIndex] - '0';
            }

            checksum += endID * MathPlus.SumRange(compactedIndex, emptySize);
            compactedIndex += emptySize;
            endCount -= emptySize;

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

        int searchStart = 0;
        int originalIndex = sortedFs.Count - 1;
        // Loop from the last block and work backwards
        while (endID >= 0)
        {
            var block = sortedFs[originalIndex];
            while (block.ID != endID)
            {
                --originalIndex;
                block = sortedFs[originalIndex];
            }

            int goalIndex = searchStart;
            var goalRange = sortedFs[goalIndex];
            while (goalIndex < originalIndex && goalRange.ID != BlockRange.EMPTY_ID)
            {
                ++searchStart;
                ++goalIndex;
                goalRange = sortedFs[goalIndex];
            }
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
                    // Merge potentially newly adjacent empty blocks at the original index, while avoiding unnecessary insertions/deletions
                    if (sortedFs[originalIndex - 1] is (BlockRange.EMPTY_ID, var range0))
                    {
                        sortedFs.ShiftElement(originalIndex, goalIndex + 1) = new(BlockRange.EMPTY_ID, goalRange.Length - block.Length);
                        // sortedFs[originalIndex - 1] is now at sortedFs[originalIndex]

                        if (originalIndex + 1 < sortedFs.Count && sortedFs[originalIndex + 1] is (BlockRange.EMPTY_ID, var range1))
                        {
                            sortedFs[originalIndex] = new(BlockRange.EMPTY_ID, range0 + block.Length + range1);
                            sortedFs.RemoveAt(originalIndex + 1);
                        }
                        else
                        {
                            sortedFs[originalIndex] = new(BlockRange.EMPTY_ID, range0 + block.Length);
                        }

                        // sortedFs[originalIndex] is now empty, reduce by one to avoid checking it when looking for the next block to move
                        --originalIndex;
                    }
                    else if (originalIndex + 1 < sortedFs.Count && sortedFs[originalIndex + 1] is (BlockRange.EMPTY_ID, var range))
                    {
                        sortedFs.ShiftElement(originalIndex, goalIndex + 1) = new(BlockRange.EMPTY_ID, goalRange.Length - block.Length);
                        sortedFs[originalIndex + 1] = new(BlockRange.EMPTY_ID, block.Length + range);
                    }
                    else
                    {
                        sortedFs.Insert(goalIndex + 1, new(BlockRange.EMPTY_ID, goalRange.Length - block.Length));
                        // sortedFs[originalIndex] is now at sortedFs[originalIndex + 1]
                        sortedFs[originalIndex + 1] = new(BlockRange.EMPTY_ID, block.Length);
                    }
                }
                else
                {
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

                        // sortedFs[originalIndex - 1] is known to be empty, reduce by 2 to avoid checking it when looking for the next block to move
                        originalIndex -= 2;
                    }
                    else if (originalIndex + 1 < sortedFs.Count && sortedFs[originalIndex + 1] is (BlockRange.EMPTY_ID, var range))
                    {
                        sortedFs[originalIndex] = new(BlockRange.EMPTY_ID, block.Length + range);
                        sortedFs.RemoveAt(originalIndex + 1);
                        // sortedFs[originalIndex] is now empty, reduce by one to avoid checking it when looking for the next block to move
                        --originalIndex;
                    }
                    else
                    {
                        sortedFs[originalIndex] = new(BlockRange.EMPTY_ID, block.Length);
                        // sortedFs[originalIndex] is now empty, reduce by one to avoid checking it when looking for the next block to move
                        --originalIndex;
                    }
                }
            }

            --endID;
        }

        long checksum = 0;
        long fsIndex = 0;
        foreach (var block in sortedFs)
        {
            var (id, range) = block;

            if (id != BlockRange.EMPTY_ID)
            {
                checksum += id * MathPlus.SumRange(fsIndex, range);
            }

            fsIndex += range;
        }
        return checksum.ToString();
    }
}
