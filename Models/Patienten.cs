using System;

namespace BedcapacityApp_bap_sebastiaan.Models
{
    public class Patient
    {
        public int pId { get; set; }
        public string patientnaam { get; set; }
        public string geslacht { get; set; }
        public int aantalDagen { get; set; }
        public DateTime opnameDatum { get; set; }
    }
}
