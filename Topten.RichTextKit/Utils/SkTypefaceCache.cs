using System.Collections.Concurrent;
using System.Diagnostics;
using SkiaSharp;

namespace Topten.RichTextKit.Utils;

internal sealed class SkTypefaceCache
{
    public static SkTypefaceCache Default { get; } = new SkTypefaceCache();

    private readonly ConcurrentDictionary<int, SKTypeface> _cache = new();

    public void ClearCache()
    {
        _cache.Clear();
    }

    public SkTypefaceCache()
    {
        
    }

    public SKTypeface Get(IStyle style, int extraWeight)
    {
        return _cache.GetOrAdd(style.GetHashCode() + extraWeight, i =>
        {
            return SKTypeface.FromFamilyName(
                style.FontFamily,
                (SKFontStyleWeight)(style.FontWeight + extraWeight),
                style.FontWidth,
                style.FontItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright
            ) ?? SKTypeface.CreateDefault();
        });
    }
}
