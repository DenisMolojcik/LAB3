using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3.Models
{
    public class Doctor
    {
        public Doctor()
        {
        }

        public int DoctorId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }

        public ICollection<Therapy> Therapies { get; set; }
        public override string ToString()
        {
            return DoctorId + " " + Name + " " + Age.ToString() + " " + Gender + " " + Position;
        }
    }
}
