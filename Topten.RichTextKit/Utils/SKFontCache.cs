using System.Collections.Concurrent;
using DesignViewEngine.Collections;
using SkiaSharp;

namespace Topten.RichTextKit.Utils;

internal sealed class SKFontCache
{
    public static SKFontCache Default { get; } = new SKFontCache();

    private readonly ConcurrentDictionary<int, ConcurrentBag<SKFont>> _cache = new();

    public void ClearCache()
    {
        foreach (var bag in _cache)
        {
            if(bag.Value.TryTake(out var font))
                font.Dispose();
        }
        _cache.Clear();
    }

    public SKFontCache()
    {

    }

    public SKFont Get(SKTypeface typeface)
    {
        var bag = _cache.GetOrAdd(typeface.GetHashCode(), i => new ConcurrentBag<SKFont>());
        if (!bag.TryTake(out var font))
            font = new SKFont(typeface);

        return font;
    }

    public void Return(SKFont font)
    {
        var bag = _cache.GetOrAdd(font.Typeface.GetHashCode(), i => new ConcurrentBag<SKFont>());
        bag.Add(font);
    }
}
