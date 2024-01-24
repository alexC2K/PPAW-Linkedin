namespace Linkedin.Services.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Caches an object and it's value recognized by it's key.
        /// </summary>
        T GetValue<T>(string key);

        /// <summary>
        /// Sets the value and duration of an cached object and adds it in memory.
        /// </summary>
        void SetValue<T>(string key, T value, TimeSpan duration);
        
        /// <summary>
        /// Deletes a cached item from the stored items.
        /// </summary>
        void Clear(string key);

        /// <summary>
        /// Dictionary of all cached items.
        /// </summary>
        IReadOnlyDictionary<string, CacheItem> CachedItems { get; }
    }
}
