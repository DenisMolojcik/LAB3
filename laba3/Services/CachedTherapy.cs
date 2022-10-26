using laba3.Data;
using laba3.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace laba3.Services
{
    public class CachedTherapy : ICachedTherapy
    {
        private readonly Context _context;
        private readonly IMemoryCache _memoryCache;
        public CachedTherapy(Context context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public void AddList(string cacheKey, int rowsNumber = 15)
        {
            IEnumerable<Therapy> therapies = _context.Therapies.Take(rowsNumber).ToList();
            if (therapies != null)
            {
                _memoryCache.Set(cacheKey, therapies, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(280)
                });
            }
        }

        public IEnumerable<Therapy> GetList(int rowsNumber = 15)
        {
            return _context.Therapies.Take(rowsNumber).ToList();
        }

        public IEnumerable<Therapy> GetList(string cacheKey, int rowsNumber = 15)
        {
            IEnumerable<Therapy> therapies;
            if (!_memoryCache.TryGetValue(cacheKey, out therapies))
            {
                therapies = _context.Therapies.Take(rowsNumber).ToList();
                if (therapies != null)
                {
                    _memoryCache.Set(cacheKey, therapies, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(280)));
                }
            }
            return therapies;
        }
    }
}
