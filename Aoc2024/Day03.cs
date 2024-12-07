namespace Aoc2024;

public partial class Day03 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method | Mean     | Error    | StdDev   |
     * |------- |---------:|---------:|---------:|
     * | Solve1 | 25.52 us | 0.145 us | 0.198 us |
     * | Solve2 | 32.27 us | 0.159 us | 0.223 us |
     */

    public override void ParseData()
    {
        // No common parsing
    }

    [Benchmark]
    public override string Solve1()
    {
        var total = 0;
        var parser = new Parser(Contents);
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
        var total = 0;
        var parser = new Parser(Contents);

        while (parser.MovePastAny("mul("u8, "don't()"u8) is int chosen and >= 0)
        {
            switch (chosen)
            {
                case 0: // mul(
                    total += ParseMulParams(ref parser);
                    break;
                case 1: // don't()
                    if (!parser.MovePastNext("do()"u8))
                    {
                        return total.ToString();
                    }
                    break;

            }
        }

        return total.ToString();
    }
}
