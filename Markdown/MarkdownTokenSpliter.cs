using System.Runtime.CompilerServices;
using System.Text;

namespace Markdown;

public class MarkdownTokenizer(TextReader input) : IAsyncEnumerable<MarkdownToken>
{
    public MarkdownTokenizer(string input)
        : this(new StringReader(input))
    { }

    public TextReader Input { get; } = input ?? throw new ArgumentNullException(nameof(input));
    
    public async IAsyncEnumerable<MarkdownToken> TokenizeAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        StringBuilder text = new();
        Memory<char> buffer = new();
        Span<char> span = buffer.Span;

        while (await Input.ReadAsync(buffer ,cancellationToken) >= 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            char c = span[0];

            if (!c.IsSpecial)
            {
                text.Append(c);
                continue;
            }

            if (text.Length > 0)
            {
                yield return Text(text.ToString());
                text.Clear();
            }

            int count = 1;

            while (Input.Peek() == c)
                count += await Input.ReadAsync(buffer, cancellationToken);

            yield return c switch
            {
                ' ' => Space(count),
                '\n' => NewLine(count),

                '#' => Hash(count),
                '*' => Star(count),
                '_' => Underscore(count),
                '`' => Backtick(count),
                '~' => Tilde(count),
                '?' => Question(count),

                '/' => Splash(count),
                '\\' => BackSplash(count),

                '[' => LeftBracket(count),
                ']' => RightBracket(count),
                '(' => LeftParen(count),
                ')' => RightParen(count),
                '{' => LeftCurlyBrace(count),
                '}' => RightCurlyBrace(count),

                '!' => Exclamation(count),
                '@' => At(count),
                '$' => Dollar(count),
                '%' => Precent(count),
                '^' => Caret(count),
                '&' => Ampersand(count),
                '=' => Equal(count),
                '|' => VerticalBar(count),

                '"' => DoubleQuote(count),
                '\'' => SingleQuote(count),

                ';' => Semicolon(count),
                ':' => Clolon(count),
                '.' => Dot(count),
                ',' => Comma(count),

                '<' => LessThan(count),
                '>' => GreaterThan(count),

                '-' => Dash(count),
                '+' => Plus(count),

                _ => Text(new string(c, count))
            };
        }

        if (text.Length > 0)
            yield return Text(text.ToString());

        yield return EndOfFile;
    }

    IAsyncEnumerator<MarkdownToken> IAsyncEnumerable<MarkdownToken>.GetAsyncEnumerator(CancellationToken cancellationToken)
        => TokenizeAsync(cancellationToken)
        .GetAsyncEnumerator(cancellationToken);
}
