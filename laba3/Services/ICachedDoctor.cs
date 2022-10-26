using laba3.Models;
using System.Collections.Generic;

namespace laba3.Services
{
    public interface ICachedDoctor
    {
        public IEnumerable<Doctor> GetList(int rowsNumber = 15);
        public void AddList(string cacheKey, int rowsNumber = 15);
        public IEnumerable<Doctor> GetList(string cacheKey, int rowsNumber = 15);
    }
}
