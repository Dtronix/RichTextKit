using System.Collections.Concurrent;
using System.Diagnostics;
using SkiaSharp;

namespace Topten.RichTextKit.Utils;

internal sealed class SkTypefaceCache
{
    public readonly struct Metrics
    {
        public readonly string FamilyName;
        public readonly int FontWeight;
        public readonly int FontWidth;
        public readonly SKFontStyleSlant FontSlant;

        public Metrics(SKTypeface typeface, string fontFamily)
        {
            FamilyName = fontFamily;
            FontWeight = typeface.FontWeight;
            FontWidth = typeface.FontWidth;
            FontSlant = typeface.FontSlant;
        }
    }
    public static SkTypefaceCache Default { get; } = new SkTypefaceCache();

    private readonly ConcurrentDictionary<int, SKTypeface> _cache = new();
    private readonly ConcurrentDictionary<SKTypeface, Metrics> _metricCache = new();

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
            var familyName = style.FontFamily;
            var typeface = SKTypeface.FromFamilyName(
                familyName,
                (SKFontStyleWeight)(style.FontWeight + extraWeight),
                style.FontWidth,
                style.FontItalic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright
            );
            if (typeface == null)
            {
                typeface = SKTypeface.CreateDefault();
            }
            else
            {
                _metricCache.TryAdd(typeface, new Metrics(typeface, familyName));
            }

            return typeface;

        });
    }

    public Metrics GetMetrics(SKTypeface typeface)
    {
        return _metricCache.GetOrAdd(typeface, skTypeface => new Metrics(typeface, typeface.FamilyName));
    }
}
