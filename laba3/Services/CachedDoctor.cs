using laba3.Data;
using laba3.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace laba3.Services
{
    public class CachedDoctor : ICachedDoctor
    {
        private readonly Context _context;
        private readonly IMemoryCache _memoryCache;
        public CachedDoctor(Context context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public void AddList(string cacheKey, int rowsNumber = 15)
        {
            IEnumerable<Doctor> doctors = _context.Doctors.Take(rowsNumber).ToList();
            if (doctors != null)
            {
                _memoryCache.Set(cacheKey, doctors, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(280)
                });
            }
        }

        public IEnumerable<Doctor> GetList(int rowsNumber = 15)
        {
            IEnumerable<Doctor> doctors = _context.Doctors.ToList();

            return _context.Doctors.Take(rowsNumber).ToList();
        }

        public IEnumerable<Doctor> GetList(string cacheKey, int rowsNumber = 15)
        {
            IEnumerable<Doctor> doctors;
            if (!_memoryCache.TryGetValue(cacheKey, out doctors))
            {
                doctors = _context.Doctors.Take(rowsNumber).ToList();
                if (doctors != null)
                {
                    _memoryCache.Set(cacheKey, doctors, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(280)));
                }
            }
            return doctors;
        }
    }
}
