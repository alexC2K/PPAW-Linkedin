namespace Linkedin.Services.Cache
{
    public class CacheService : ICacheService
    {
        private Dictionary<string, CacheItem> _cachedItems = new Dictionary<string, CacheItem>();
        public IReadOnlyDictionary<string, CacheItem> CachedItems => _cachedItems;

        public T? GetValue<T>(string key)
        {
            if (_cachedItems.TryGetValue(key, out var cachedItem))
            {
                if (cachedItem.Expires > DateTime.UtcNow)
                {
                    return (T)cachedItem.Value;
                }

                _cachedItems.Remove(key);
            }

            return default(T);
        }

        public void SetValue<T>(string key, T value, TimeSpan duration)
        {
            var expirationTime = DateTime.UtcNow.Add(duration);
            var cachedItem = new CacheItem(value, expirationTime);

            _cachedItems[key] = cachedItem;
        }

        public void Clear(string key)
        {
            if (_cachedItems.TryGetValue(key, out _))
            {
                _cachedItems.Remove(key);
            }
        }
    }
}
