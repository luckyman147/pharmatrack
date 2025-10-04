using System;
using Microsoft.Extensions.Caching.Memory;
using PharmaTrack.Application.Interfaces.services;

namespace PharmaTrack.Infrastructure.Services;


    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache cache) => _cache = cache;

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var opts = new MemoryCacheEntryOptions();
            if (expiry.HasValue) opts.SetAbsoluteExpiration(expiry.Value);
            _cache.Set(key, value, opts);
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            _cache.TryGetValue(key, out T? val);
            return Task.FromResult(val);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }