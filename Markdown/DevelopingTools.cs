#if DEBUG
#pragma warning disable IDE0079 // Remove unnecessary suppression. This is used to disable the warning for the entire file.
#pragma warning disable IDE0005 // Using directive is unnecessary. Remove unnecessary usings.
global using static Markdown.DevelopingTools;
#pragma warning restore IDE0005 // Using directive is unnecessary. Remove unnecessary usings.
#pragma warning restore IDE0079 // Remove unnecessary suppression. This is used to disable the warning for the entire file.

namespace Markdown;

internal static class DevelopingTools
{
    public static NotImplementedException TODO(string message = "TODO") => new(message);

    public static T TODO<T>(string message = "TODO") => throw TODO(message);
}
#endif
