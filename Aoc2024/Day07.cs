using System.Runtime.InteropServices;

namespace Aoc2024;

public partial class Day07 : DayBase
{
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
