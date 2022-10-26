using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3.Models
{
    public class CostMediciane
    {
        public int CostMedicianeId { get; set; }
        public string Manufacturer { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Medician> Medicianes { get; set; }

    }
}
