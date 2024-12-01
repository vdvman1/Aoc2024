using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InputsSourceGen;

[Generator]
public class DaysGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        string inputs = "Inputs" + Path.DirectorySeparatorChar;
        string ext = ".txt";
        const int dayLen = 2;
        int nameLen = dayLen + ext.Length;
        int len = inputs.Length + nameLen;
        var days = new SortedList<string, string>();

        foreach (var file in context.AdditionalFiles)
        {
            var path = file.Path;
            if (
                path.Length < len
                || !path.EndsWith(ext)
                || string.CompareOrdinal(path, path.Length - len, inputs, 0, inputs.Length) != 0
                || file.GetText()?.ToString() is not string contents)
            {
                continue;
            }

            var day = path.Substring(path.Length - nameLen, dayLen);
            var name = "Day" + day;
            days.Add(day, name);
            var daySrc = $$"""
                namespace Aoc2024;

                public partial class {{name}}
                {
                    private static ReadOnlySpan<byte> Contents => "{{contents.Replace("\n", "\\n")}}"u8;
                }
                """;
            context.AddSource(name + ".g.cs", daySrc);
        }

        var src = $$"""
            namespace Aoc2024;

            public partial class Program
            {
                private static readonly (string Num, DayBase Instance)[] Days = new (string, DayBase)[]
                {
                    {{string.Join(",\n        ", days.Select(pair => $"""("{pair.Key}", new {pair.Value}())"""))}}
                };

                private static readonly Type[] DayTypes = new Type[]{{{string.Join(", ", days.Values.Select(day => $"typeof({day})"))}}};
            }
            """;
        context.AddSource("Program.g.cs", src);
    }

    public void Initialize(GeneratorInitializationContext context) { }
}