using System;
using System.Linq;
using System.Runtime.Caching;
using Ubik.Infra.Contracts;

namespace Ubik.Cache.Runtime
{
    public class MemoryDefaultCacheProvider : ICacheProvider
    {
        protected static MemoryCache _cache;
        protected Object _lock = new Object();

        protected virtual MemoryCache CurrentCache
        {
            get
            {
                lock (_lock)
                {
                    if (_cache == null)
                        _cache = MemoryCache.Default;
                }
                return _cache;
            }
        }

        public virtual object GetItem(string key)
        {
            return CurrentCache[key.ToLowerInvariant()];
        }

        public virtual void SetItem(string key, object value)
        {
            if (value == null) return;
            lock (_lock)
            {
                var policy = new CacheItemPolicy
                {
                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                    AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
                };
                CurrentCache.Set(key.ToLowerInvariant(), value, policy);
            }
        }

        public virtual void SetItem(string key, object value, params int[] ttl)
        {
            if (value == null) return;
            var ttlCount = (ttl.Count() > 4) ? 4 : ttl.Count();
            var cacheDur = DateTimeOffset.Now;
            for (var i = 0; i < ttlCount; i++)
            {
                switch (i)
                {
                    case 3:
                        cacheDur = cacheDur.AddSeconds(ttl[3]);
                        break;

                    case 2:
                        cacheDur = cacheDur.AddMinutes(ttl[2]);
                        break;

                    case 1:
                        cacheDur = cacheDur.AddHours(ttl[1]);
                        break;

                    case 0:
                        cacheDur = cacheDur.AddDays(ttl[0]);
                        break;
                }
            }

            lock (_lock)
            {
                var policy = new CacheItemPolicy
                {
                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                    AbsoluteExpiration = cacheDur
                };
                CurrentCache.Set(key.ToLower(), value, policy);
            }
        }

        public virtual void SetItem(string key, object value, DateTime absoluteExpiration)
        {
            if (value == null) return;
            lock (_lock)
            {
                var policy = new CacheItemPolicy
                {
                    SlidingExpiration = ObjectCache.NoSlidingExpiration,
                    AbsoluteExpiration = absoluteExpiration
                };
                CurrentCache.Set(key.ToLower(), value, policy);
            }
        }

        public virtual void RemoveItem(string key)
        {
            if (!CurrentCache.Contains(key.ToLowerInvariant())) return;
            lock (_lock)
            {
                CurrentCache.Remove(key.ToLower());
            }
        }
    }
}