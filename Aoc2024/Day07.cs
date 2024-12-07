using System.Runtime.InteropServices;

namespace Aoc2024;

public partial class Day07 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean         | Error       | StdDev      |
     * |---------- |-------------:|------------:|------------:|
     * | ParseData |     149.4 us |     2.51 us |     3.59 us |
     * | Solve1    |   1,107.2 us |    10.13 us |    14.20 us |
     * | Solve2    | 353,388.0 us | 3,536.62 us | 4,957.84 us |
     */

    private readonly record struct Equation(long Result, List<int> Terms);
    private readonly List<Equation> Equations = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);
        Equations.Clear();

        while (!parser.IsEmpty && parser.ParseLine() is var lineParser)
        {
            var result = lineParser.ParsePosLong();
            lineParser.Skip(2);
            List<int> terms = [];
            while (!lineParser.IsEmpty)
            {
                terms.Add(lineParser.ParsePosInt());
                lineParser.MoveNext();
            }

            Equations.Add(new(result, terms));
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        long sum = 0;

        foreach (var (result, terms) in Equations)
        {
            if (HasSolution1(result, terms[0], CollectionsMarshal.AsSpan(terms)[1..]))
            {
                sum += result;
            }
        }

        return sum.ToString();
    }

    private static bool HasSolution1(long goal, long partialResult, ReadOnlySpan<int> remainingTerms)
    {
        if (remainingTerms.Length == 0)
        {
            return partialResult == goal;
        }

        return HasSolution1(goal, partialResult + remainingTerms[0], remainingTerms[1..])
            || HasSolution1(goal, partialResult * remainingTerms[0], remainingTerms[1..]);
    }

    [Benchmark]
    public override string Solve2()
    {
        long sum = 0;

        foreach (var (result, terms) in Equations)
        {
            if (HasSolution2(result, terms[0], CollectionsMarshal.AsSpan(terms)[1..]))
            {
                sum += result;
            }
        }

        return sum.ToString();
    }

    private static bool HasSolution2(long goal, long partialResult, ReadOnlySpan<int> remainingTerms)
    {
        if (remainingTerms.Length == 0)
        {
            return partialResult == goal;
        }

        return HasSolution2(goal, partialResult + remainingTerms[0], remainingTerms[1..])
            || HasSolution2(goal, partialResult * remainingTerms[0], remainingTerms[1..])
            || HasSolution2(goal, long.Parse(partialResult.ToString() + remainingTerms[0]), remainingTerms[1..]);
    }
}
