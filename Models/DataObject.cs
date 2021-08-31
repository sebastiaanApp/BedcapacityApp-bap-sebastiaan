using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedcapacityApp_bap_sebastiaan.Models
{
    public class DataObjects
    {
        public List<Patient> patienten { get; set; }
        public List<Kamer> kamers { get; set; }
        public List<Bed> bedden { get; set; }
    }
}
