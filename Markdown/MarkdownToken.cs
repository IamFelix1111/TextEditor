global using static Markdown.MarkdownToken;
using System.Diagnostics;

namespace Markdown;

public enum MarkdownTokenKind
{
    Text = 1,

    Space = ' ',
    NewLine = '\n',

    Hash = '#',
    Star = '*',
    Underscore = '_',
    Backtick = '`',
    Tilde = '~',
    Question = '?',

    Slash = '/',
    Backslash = '\\',

    LeftBracket = '[',
    RightBracket = ']',
    LeftParen = '(',
    RightParen = ')',
    LeftCurlyBrace = '{',
    RightCurlyBrace = '}',

    Exclamation = '!',
    At = '@',
    Dollar = '$',
    Percent = '%',
    Caret = '^',
    Ampersand = '&',
    Equal = '=',
    VerticalBar = '|',

    DoubleQuote = '"',
    SingleQuote = '\'',

    Semicolon = ';',
    Colon = ':',
    Dot = '.',
    Comma = ',',

    LessThan = '<',
    GreaterThan = '>',

    Dash = '-',
    Plus = '+',

    EndOfFile = '\0',
}

[DebuggerDisplay("{ToString(),nq}")]
public abstract record class MarkdownToken(MarkdownTokenKind Kind, string Value)
{
    public override string ToString() =>
        $"<{Kind}: \"{Value.TransCode}\">";

    public bool Is(MarkdownTokenKind kind) =>
        Kind == kind;

    public virtual bool Is(char character) => false;

    [DebuggerDisplay("{ToString(),nq}")]
    public sealed record class RepeatedToken(char Character, int Count)
        : MarkdownToken((MarkdownTokenKind)Character, new(Character, Count))
    {
        public override string ToString() =>
            $"<{Kind}: {Count}>";

        public override bool Is(char character) =>
            Character == character;
    }

    [DebuggerDisplay("{ToString(),nq}")]
    public sealed record class EndOfFileToken()
        : MarkdownToken(MarkdownTokenKind.EndOfFile, string.Empty)
    {
        public override string ToString() =>
            "<EndOfFile>";
    }

    public static MarkdownToken Text(string value) => new(MarkdownTokenKind.Text, value);

    public static RepeatedToken Repeat(char character, int count = 1) => new(character, count);

    public static RepeatedToken Space(int count = 1) => new(' ', count);
    public static RepeatedToken NewLine(int count = 1) => new('\n', count);
    public static RepeatedToken Hash(int count = 1) => new('#', count);
    public static RepeatedToken Star(int count = 1) => new('*', count);
    public static RepeatedToken Underscore(int count = 1) => new('_', count);
    public static RepeatedToken Backtick(int count = 1) => new('`', count);
    public static RepeatedToken Tilde(int count = 1) => new('~', count);
    public static RepeatedToken Question(int count = 1) => new('?', count);

    public static RepeatedToken Slash(int count = 1) => new('/', count);
    public static RepeatedToken Backslash(int count = 1) => new('\\', count);

    public static RepeatedToken LeftBracket(int count = 1) => new('[', count);
    public static RepeatedToken RightBracket(int count = 1) => new(']', count);
    public static RepeatedToken LeftParen(int count = 1) => new('(', count);
    public static RepeatedToken RightParen(int count = 1) => new(')', count);
    public static RepeatedToken LeftCurlyBrace(int count = 1) => new('{', count);
    public static RepeatedToken RightCurlyBrace(int count = 1) => new('}', count);

    public static RepeatedToken Exclamation(int count = 1) => new('!', count);
    public static RepeatedToken At(int count = 1) => new('@', count);
    public static RepeatedToken Dollar(int count = 1) => new('$', count);
    public static RepeatedToken Percent(int count = 1) => new('%', count);
    public static RepeatedToken Caret(int count = 1) => new('^', count);
    public static RepeatedToken Ampersand(int count = 1) => new('&', count);
    public static RepeatedToken Equal(int count = 1) => new('=', count);
    public static RepeatedToken VerticalBar(int count = 1) => new('|', count);

    public static RepeatedToken DoubleQuote(int count = 1) => new('"', count);
    public static RepeatedToken SingleQuote(int count = 1) => new('\'', count);

    public static RepeatedToken Semicolon(int count = 1) => new(';', count);
    public static RepeatedToken Colon(int count = 1) => new(':', count);
    public static RepeatedToken Dot(int count = 1) => new('.', count);
    public static RepeatedToken Comma(int count = 1) => new(',', count);

    public static RepeatedToken LessThan(int count = 1) => new('<', count);
    public static RepeatedToken GreaterThan(int count = 1) => new('>', count);

    public static RepeatedToken Dash(int count = 1) => new('-', count);
    public static RepeatedToken Plus(int count = 1) => new('+', count);

    public static EndOfFileToken EndOfFile { get; } = new();
}
