namespace Aoc2024;

public partial class Day05 : DayBase
{
    /*
     * Measured performance:
     * 
     * | Method    | Mean        | Error    | StdDev    |
     * |---------- |------------:|---------:|----------:|
     * | ParseData |    82.41 us | 0.508 us |  0.729 us |
     * | Solve1    |   498.17 us | 2.643 us |  3.790 us |
     * | Solve2    | 2,292.23 us | 8.335 us | 12.218 us |
     */

    private readonly struct Node
    {
        /// <summary>
        /// Pages that must come before this page
        /// </summary>
        public readonly List<int> Before;

        /// <summary>
        /// Pages that must come after this page
        /// </summary>
        public readonly List<int> After;

        public Node()
        {
            Before = [];
            After = [];
        }
    }

    private readonly Dictionary<int, Node> OrderingRules = [];
    private readonly List<List<int>> Pages = [];

    [Benchmark]
    public override void ParseData()
    {
        var parser = new Parser(Contents);

        OrderingRules.Clear();
        Pages.Clear();

        // Parse ordering rules
        while (parser.ParseLineProper() is var lineParser && !lineParser.IsEmpty)
        {
            var before = lineParser.ParsePosInt();
            lineParser.MoveNext(); // Skip "|"
            var after = lineParser.ParsePosInt();

            if (!OrderingRules.TryGetValue(before, out var beforeRules))
            {
                beforeRules = new();
                OrderingRules[before] = beforeRules;
            }
            beforeRules.After.Add(after);

            if (!OrderingRules.TryGetValue(after, out var afterRules))
            {
                afterRules = new();
                OrderingRules[after] = afterRules;
            }
            afterRules.Before.Add(before);
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

            if (orderingRules.Before.Any(before => pages.IndexOf(before, i + 1) >= 0))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    public override string Solve2()
    {
        var sum = 0;

        var tree = new Dictionary<int, Node>();
        var orderedPages = new List<int>();
        var sources = new Queue<(int, List<int>)>();

        foreach (var pages in Pages)
        {
            // Build actual rules DAG, so that any pages that don't exist aren't included in the topological sort
            tree.Clear();
            bool outOfOrder = false;
            foreach (int page in pages)
            {
                if (!OrderingRules.TryGetValue(page, out var orderingRules)) { throw new InvalidOperationException("Unable to order pages without any rules"); }

                if (!tree.TryGetValue(page, out var thisNode))
                {
                    thisNode = new();
                    tree[page] = thisNode;
                }

                foreach (var after in orderingRules.After)
                {
                    if (tree.TryGetValue(after, out var afterNode))
                    {
                        // Page already seen, means it comes before this,
                        // but must be after, so out of order
                        outOfOrder = true;
                        afterNode.Before.Add(page);
                        thisNode.After.Add(after);
                    }

                    // If not seen yet, after page either doesn't exist or is in order and will be handled by its Before list later when encountered
                }

                foreach (var before in orderingRules.Before)
                {
                    if (tree.TryGetValue(before, out var beforeNode))
                    {
                        // Page already seen, means it comes before this
                        // and is in correct order
                        beforeNode.After.Add(page);
                        thisNode.Before.Add(before);
                    }

                    // If not seen yet, before page either doesn't exist or is out of order and will be handled by its After list later when encountered
                }
            }

            if (outOfOrder) // Only out of order page lists should be reordered and counted in the sum
            {
                orderedPages.Clear();

                // Find source nodes (nodes without any incoming edges, in this case Before pages)
                sources.Clear();
                foreach (var (page, rules) in tree)
                {
                    if (rules.Before.Count == 0)
                    {
                        sources.Enqueue((page, rules.After));
                    }
                }

                // Add each source to the page order
                while (sources.TryDequeue(out var node))
                {
                    var (page, children) = node;

                    orderedPages.Add(page);

                    // Remove this node from the tree
                    foreach (var after in children)
                    {
                        var rules = tree[after];
                        rules.Before.Remove(page);

                        // Check if the child node is now a source
                        if (rules.Before.Count == 0)
                        {
                            sources.Enqueue((after, rules.After));
                        }
                    }
                }

                // Sum up middle element
                sum += orderedPages[pages.Count / 2];
            }
        }

        return sum.ToString();
    }
}
