namespace Aoc2024.Utilities;

public ref struct Parser
{
    private readonly ReadOnlySpan<byte> Bytes;
    private int Offset = 0;

    public Parser(ReadOnlySpan<byte> bytes, int offset = 0)
    {
        Bytes = bytes;
        Offset = offset;
    }

    public readonly bool IsEmpty => Offset >= Bytes.Length;

    public void MoveNext() => Offset++;

    public int ParsePosInt()
    {
        int result = 0;
        while (!IsEmpty && (Bytes[Offset] - '0') is var c && (uint)c <= 9u)
        {
            result = 10 * result + c;
            MoveNext();
        }
        return result;
    }

    public void SkipWhitespace()
    {
        while (!IsEmpty && char.IsWhiteSpace((char)Bytes[Offset]))
        {
            MoveNext();
        }
    }

    public Parser ParseLine()
    {
        Parser parser;
        var index = Bytes[Offset..].IndexOf((byte)'\n');
        if (index < 0)
        {
            parser = new Parser(Bytes, Offset);
            Offset = Bytes.Length;
            return parser;
        }

        parser = new Parser(Bytes[..(Offset + index)], Offset);
        Offset += index + 1;
        return parser;
    }
}
