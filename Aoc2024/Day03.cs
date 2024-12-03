﻿using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day03 : DayBase
{
    public override void ParseData()
    {
        // Currently no common parsing
    }

    [Benchmark]
    public override string Solve1()
    {
        var total = 0;
        var parser = new Parser(Contents /*"xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"u8*/);
        while (parser.MovePastNext("mul("u8))
        {
            total += ParseMulParams(ref parser);
        }

        return total.ToString();
    }

    private static int ParseMulParams(ref Parser parser)
    {
        // Parse first number
        if (!parser.TryParseDigit(out int a))
        {
            return 0;
        }

        if (parser.TryMatch((byte)','))
        {
            // Move to next parameter
        }
        else if (parser.TryParseDigit(out int digit))
        {
            a = 10 * a + digit;
            if (parser.TryMatch((byte)','))
            {
                // Move to next parameter
            }
            else if (parser.TryParseDigit(out digit))
            {
                a = 10 * a + digit;
                if (!parser.TryMatch((byte)','))
                {
                    return 0;
                }

                // Move to next parameter
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }

        // Parse second number
        if (!parser.TryParseDigit(out int b))
        {
            return 0;
        }

        if (parser.TryMatch((byte)')'))
        {
            return a * b;
        }
        else if (parser.TryParseDigit(out int digit))
        {
            b = 10 * b + digit;
            if (parser.TryMatch((byte)')'))
            {
                return a * b;
            }
            else if (parser.TryParseDigit(out digit))
            {
                b = 10 * b + digit;
                if (parser.TryMatch((byte)')'))
                {
                    return a * b;
                }

                return 0;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
