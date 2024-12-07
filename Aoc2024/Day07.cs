using System.Runtime.InteropServices;

namespace Aoc2024;

public partial class Day07 : DayBase
{
    private readonly record struct Equation(long Result, List<int> Terms);
    private readonly List<Equation> Equations = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents
        //"""
        //190: 10 19
        //3267: 81 40 27
        //83: 17 5
        //156: 15 6
        //7290: 6 8 6 15
        //161011: 16 10 13
        //192: 17 8 14
        //21037: 9 7 18 13
        //292: 11 6 16 20
        //"""u8
        );
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
        throw new NotImplementedException();
    }
}
