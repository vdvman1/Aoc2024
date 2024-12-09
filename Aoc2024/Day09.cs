namespace Aoc2024;

public partial class Day09 : DayBase
{
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

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
