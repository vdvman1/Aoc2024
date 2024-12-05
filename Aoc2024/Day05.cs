using Aoc2024.Utilities;

namespace Aoc2024;

public partial class Day05 : DayBase
{
    private readonly Dictionary<int, List<int>> OrderingRules = [];
    private readonly List<List<int>> Pages = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents
        //"""
        //47|53
        //97|13
        //97|61
        //97|47
        //75|29
        //61|13
        //75|53
        //29|13
        //97|29
        //53|29
        //61|53
        //97|53
        //61|29
        //47|13
        //75|47
        //97|75
        //47|61
        //75|61
        //47|29
        //75|13
        //53|13

        //75,47,61,53,29
        //97,61,53,29,13
        //75,29,13
        //75,97,47,61,53
        //61,13,29
        //97,13,75,29,47
        //"""u8
        );

        OrderingRules.Clear();
        Pages.Clear();

        // Parse ordering rules
        while (parser.ParseLineProper() is var lineParser && !lineParser.IsEmpty)
        {
            var before = lineParser.ParsePosInt();
            lineParser.MoveNext(); // Skip "|"
            var after = lineParser.ParsePosInt();
            if (!OrderingRules.TryGetValue(after, out var orderingRules))
            {
                orderingRules = [];
                OrderingRules[after] = orderingRules;
            }

            orderingRules.Add(before);
        }

        // Parse page lists
        while (!parser.IsEmpty && parser.ParseLine() is var lineParser)
        {
            List<int> pages = [];
            pages.Add(lineParser.ParsePosInt());
            while (lineParser.TryMatch((byte)','))
            {
                pages.Add(lineParser.ParsePosInt());
            }

            Pages.Add(pages);
        }
    }

    [Benchmark]
    public override string Solve1()
    {
        var sum = 0;

        foreach (var pages in Pages)
        {
            if (IsValidPageOrder(pages))
            {
                sum += pages[pages.Count / 2];
            }
        }

        return sum.ToString();
    }

    private bool IsValidPageOrder(List<int> pages)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (!OrderingRules.TryGetValue(pages[i], out var orderingRules)) { continue; }

            if (orderingRules.Any(after => pages.IndexOf(after, i + 1) >= 0))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    public override string Solve2()
    {
        throw new NotImplementedException();
    }
}
