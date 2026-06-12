namespace Markdown;

public static class Helper
{
    extension(char c)
    {
        public bool IsBlank => " \t\r\n".Contains(c);
        public bool IsSpecial =>
        c switch
        {
            ' ' or '\r' or '\n' or
            '#' or '*' or '_' or '`' or '~' or '?' or
            '/' or '\\' or
            '[' or ']' or '(' or ')' or '{' or '}' or
            '!' or '@' or '$' or '%' or '^' or '&' or '=' or '|' or
            '"' or '\'' or
            ';' or ':' or '.' or ',' or
            '<' or '>' or
            '-' or '+'
                => true,

            _ => false,
        };
    }

    extension(string s)
    {
        public string TransCode =>
        s.Replace("\\", @"\\")
         .Replace("\n", @"\n")
         .Replace("\r", @"\r")
         .Replace("\t", @"\t")
         .Replace("\"", @"\""");
    }
}
