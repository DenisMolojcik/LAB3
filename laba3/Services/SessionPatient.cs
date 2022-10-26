using laba3.Data;
using laba3.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace laba3.Services
{
    public class SessionPatient
    {
        private readonly Context _context;
        public SessionPatient(Context context)
        {
            _context = context;
        }
        public IEnumerable<Patient> GetList(int rowsNumber = 15)
        {
            return _context.Patients.Take(rowsNumber).ToList();
        }
    }
}
