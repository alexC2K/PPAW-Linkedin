namespace Linkedin.Services.Cache
{
    public class CacheItem
    {
        private object _value;
        private DateTime _expires;

        public object Value => _value;
        public DateTime Expires => _expires;

        public CacheItem(object value, DateTime expires)
        {
            _value = value;
            _expires = expires;
        }
    }
}