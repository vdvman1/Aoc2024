using System.Runtime.InteropServices;

namespace Aoc2024;

public partial class Day07 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean     | Error   | StdDev  |
     * |---------- |---------:|--------:|--------:|
     * | ParseData | 144.7 us | 1.15 us | 1.68 us |
     * | Solve1    | 118.6 us | 0.29 us | 0.43 us |
     * | Solve2    | 201.6 us | 0.53 us | 0.77 us |
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
            if (HasSolution1(result, CollectionsMarshal.AsSpan(terms)))
            {
                sum += result;
            }
        }

        return sum.ToString();
    }

    private static bool HasSolution1(long goal, ReadOnlySpan<int> terms)
    {
        switch (terms)
        {
            case [int n]:
                return n == goal;
            case [.. var rest, int n]:
                if (Math.DivRem(goal, n) is (var newGoal, 0L) && HasSolution1(newGoal, rest))
                {
                    return true;
                }

                if (goal >= n && HasSolution1(goal - n, rest))
                {
                    return true;
                }
                break;
        }

        return false;
    }

    [Benchmark]
    public override string Solve2()
    {
        long sum = 0;

        foreach (var (result, terms) in Equations)
        {
            if (HasSolution2(result, CollectionsMarshal.AsSpan(terms)))
            {
                sum += result;
            }
        }

        return sum.ToString();
    }

    private static bool HasSolution2(long goal, ReadOnlySpan<int> terms)
    {
        switch (terms)
        {
            case [int n]:
                return n == goal;
            case [.. var rest, int n]:
                if (Math.DivRem(goal, n) is (var newGoal, 0L) && HasSolution2(newGoal, rest))
                {
                    return true;
                }

                (newGoal, var bottomDigits) = Math.DivRem(goal, n.NextPow10());
                if (bottomDigits == n && HasSolution2(newGoal, rest))
                {
                    return true;
                }

                if (goal >= n && HasSolution2(goal - n, rest))
                {
                    return true;
                }
                break;
        }

        return false;
    }
}
