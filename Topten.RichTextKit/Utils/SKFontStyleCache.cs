using System;
using System.Collections.Concurrent;
using SkiaSharp;

namespace Topten.RichTextKit.Utils
{
    internal sealed class SKFontStyleCache
    {
        public static SKFontStyleCache Default { get; } = new SKFontStyleCache();

        private readonly ConcurrentDictionary<int, SKFontStyle> _cache = new();

        public void ClearCache()
        {
            foreach (var skFontStyle in _cache)
            {
                skFontStyle.Value.Dispose();
            }
            _cache.Clear();
        }

        public SKFontStyleCache()
        {

        }

        public SKFontStyle Get(int weight, int width, SKFontStyleSlant slant)
        {
            var hash = HashCode.Combine(weight, width, slant);
            return _cache.GetOrAdd(hash, i => new SKFontStyle(weight, width, slant));
        }
    }
}
