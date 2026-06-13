namespace Markdown;

public static class Helper
{
    extension(char c)
    {
        public bool IsBlank =>
            c is ' ' or '\t' or '\r' or '\n';

        public bool IsSpecial =>
           c is ' ' or '\r' or '\n' or
                '#' or '*' or '_' or '`' or '~' or '?' or
                '/' or '\\' or
                '[' or ']' or '(' or ')' or '{' or '}' or
                '!' or '@' or '$' or '%' or '^' or '&' or '=' or '|' or
                '"' or '\'' or
                ';' or ':' or '.' or ',' or
                '<' or '>' or
                '-' or '+';
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
