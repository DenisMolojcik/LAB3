using laba3.Models;
using System.Collections.Generic;

namespace laba3.Services
{
    public interface ICachedTherapy
    {
        public IEnumerable<Therapy> GetList(int rowsNumber = 15);
        public void AddList(string cacheKey, int rowsNumber = 15);
        public IEnumerable<Therapy> GetList(string cacheKey, int rowsNumber = 15);
    }
}
