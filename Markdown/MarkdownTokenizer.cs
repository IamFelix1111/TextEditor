using System.Text;

namespace Markdown;

public sealed class MarkdownTokenizer : IAsyncEnumerator<MarkdownToken>
{
    private readonly StringBuilder text = new();
    private readonly Memory<char> buffer = new char[1];
    private readonly CancellationToken cancellationToken;
    private char? peeked;
    private int count;
    private bool is_doing;
    private bool is_done;
    private MarkdownToken current = default!;

    public TextReader Input { get; }

    public MarkdownTokenizer(TextReader input, CancellationToken cancellationToken = default)
    {
        Input = input ?? throw new ArgumentNullException(nameof(input));
        this.cancellationToken = cancellationToken;

        ((IAsyncEnumerator<MarkdownToken>)this).MoveNextAsync().AsTask().Wait(cancellationToken);
    }

    public MarkdownTokenizer(string input, CancellationToken cancellationToken = default)
        : this(new StringReader(input), cancellationToken)
    { }

    private async ValueTask<char?> PeekAsync()
    {
        if (peeked.HasValue)
            return peeked.Value;
        else
        {
            int read = await Input.ReadAsync(buffer, cancellationToken);
            if (read >= 0) return peeked = buffer.Span[0];
            else return null;
        }
    }

    private async ValueTask<char?> ReadAsync()
    {
        if (peeked.HasValue)
        {
            char _return = peeked.Value;
            peeked = null;
            return _return;
        }
        else
        {
            int read = await Input.ReadAsync(buffer, cancellationToken);
            if (read >= 0) return buffer.Span[0];
            else return null;
        }
    }

    private void GetLock(CancellationToken cancellationToken = default)
    {
        if (is_doing)
            throw new InvalidOperationException("Tokenization is already in progress.");

        if (!Monitor.TryEnter(this))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new OperationCanceledException("Unable to acquire tokenization lock.");
        }

        if (is_doing)
        {
            Monitor.Exit(this);
            throw new InvalidOperationException("Tokenization is already in progress.");
        }

        is_doing = true;
        Monitor.Exit(this);
    }

    private void ReleaseLock()
    {
        is_doing = false;
        if (Monitor.IsEntered(this))
            Monitor.Exit(this);
    }

    MarkdownToken IAsyncEnumerator<MarkdownToken>.Current => current;

    async ValueTask<bool> IAsyncEnumerator<MarkdownToken>.MoveNextAsync()
    {
        GetLock();

        if (is_done) return false;

        char? c_;

        if ((c_ = await ReadAsync()).HasValue)
            do
            {
                char c = c_.Value;

                if (c == '\r')
                    continue;

                if (!c.IsSpecial)
                {
                    text.Append(c);
                    continue;
                }

                if (text.Length > 0)
                {
                    current = Text(text.ToString());
                    text.Clear();
                    ReleaseLock();
                    return true;
                }

                count = 1;

                while (await PeekAsync() == c)
                {
                    await ReadAsync();
                    count++;
                }

                current = Repeat(c, count);
                ReleaseLock();
                return true;
            }
            while ((c_ = await ReadAsync()).HasValue);
        
        if (text.Length > 0)
            current = Text(text.ToString());
        else
        {
            current = EndOfFile;
            is_done = true;
        }
        ReleaseLock();
        return true;
    }

    async ValueTask IAsyncDisposable.DisposeAsync() => GC.SuppressFinalize(this);
}
