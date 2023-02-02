using System;
using System.Collections.Concurrent;

namespace Topten.RichTextKit.Utils;

internal abstract class SkCacheWrapper<TCache, TCachedType, TConfig>
    where TConfig : IEquatable<TConfig>
    where TCache : new()
{
    public static TCache Default { get; } = new TCache();

    protected readonly ConcurrentDictionary<int, TCachedType> Cache = new();

    protected abstract TCachedType Create(TConfig config);

    public TCachedType Get(TConfig config)
    {
        return Cache.GetOrAdd(config.GetHashCode(), i => Create(config));
    }

    public void ClearCache()
    {
        Cache.Clear();
    }
}

