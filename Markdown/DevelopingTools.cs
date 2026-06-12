#if DEBUG
global using static Markdown.DevelopingTools;

namespace Markdown;

internal static class DevelopingTools
{
    public static NotImplementedException TODO(string message = "TODO") => new(message);

    public static T TODO<T>(string message = "TODO") => throw TODO(message);
}
#endif
