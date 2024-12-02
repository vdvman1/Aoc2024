namespace Aoc2024.Utilities;

public ref struct Parser
{
    private ReadOnlySpan<byte> Bytes;

    public Parser(ReadOnlySpan<byte> bytes)
    {
        Bytes = bytes;
    }

    public readonly bool IsEmpty => Bytes.IsEmpty;

    public void MoveNext() => Bytes = Bytes[1..];

    public int ParsePosInt()
    {
        int result = 0;
        while (!Bytes.IsEmpty && (Bytes[0] - '0') is var c && (uint)c <= 9u)
        {
            result = 10 * result + c;
            MoveNext();
        }
        return result;
    }

    public void SkipWhitespace()
    {
        while (!Bytes.IsEmpty && char.IsWhiteSpace((char)Bytes[0]))
        {
            MoveNext();
        }
    }

    public Parser ParseLine()
    {
        Parser parser;
        var index = Bytes.IndexOf((byte)'\n');
        if (index < 0)
        {
            parser = new Parser(Bytes);
            Bytes = Bytes[^0..];
            return parser;
        }

        parser = new Parser(Bytes[..index]);
        Bytes = Bytes[(index + 1)..];
        return parser;
    }
}
