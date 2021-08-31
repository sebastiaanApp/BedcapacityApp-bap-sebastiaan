using System;
namespace BedcapacityApp_bap_sebastiaan.Models
{
    public class BedPredictionModel
    {
        public Bed bed { get; set; }
        public DateTime LeaveDateCurrentPatient { get; set; }
        public string Gender { get; set; }
        public RoomStatus Roomstatus { get; set; }
    }
}
