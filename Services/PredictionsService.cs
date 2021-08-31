using System;
using System.Collections.Generic;
using System.Linq;
using BedcapacityApp_bap_sebastiaan.Interfaces.Services;
using BedcapacityApp_bap_sebastiaan.Models;

namespace BedcapacityApp_bap_sebastiaan.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IDataImporter dataImporter;
        public PredictionService(IDataImporter dataImporter)
        {
            this.dataImporter = dataImporter;
        }
        public List<BedPredictionModel> GetBestBeds(Patient patient)
        {
            var beds = new List<BedPredictionModel>();
            beds = GetBedsByDateAndGender(patient);
            return beds;
        }
        public List<BedPredictionModel> GetAlternativeBeds(Patient patient)
        {
            var beds = new List<BedPredictionModel>();
            var possibleKamesByGender = GetPossibleKamersByGender(patient.geslacht);
            beds = GetBedsByNearestLeaveDate(possibleKamesByGender, patient.opnameDatum.AddDays(patient.aantalDagen), patient);

            //ugly
            var data = dataImporter.getData();
            var patients = data.patienten.Where(x => x.opnameDatum.AddDays(x.aantalDagen) > patient.opnameDatum);

            var EmptyBeds = data.bedden.Where(x => !patients.Select(x => x.pId).Contains(x.pId));
            var EmptyRooms = EmptyBeds.GroupBy(x => x.kId).Select(x => new { kamerId = x.Key, legeBedden = x.Count() }).Where(x => x.legeBedden == 2);

            var emptyBedsByEmptyRoom = data.bedden.Where(x => EmptyRooms.Select(x => x.kamerId).Contains(x.kId));
            if (emptyBedsByEmptyRoom.Any())
            {
                foreach (var bed in emptyBedsByEmptyRoom)
                {
                    if (beds.Any(x => x.bed.bId == bed.bId))
                    {
                    }
                    else
                    {
                        beds.Add(new BedPredictionModel { bed = bed, Gender = patient.geslacht, Roomstatus = RoomStatus.Empty });
                    }

                }
            }

            return beds;
        }
        private List<BedPredictionModel> GetBedsByNearestLeaveDate(List<int> kamerIds, DateTime leaveDate, Patient patient)
        {
            List<BedPredictionModel> beds = new List<BedPredictionModel>();

            var data = dataImporter.getData();
            var GetBedsByRoomId = data.bedden.Where(x => kamerIds.Contains(x.kId));
            var GetOccupiedBedIdsByRoomId = GetBedsByRoomId.Where(x => x.pId != 0).Select(p => p.pId);

            var patientsOnOccupiedRoom = data.patienten.Where(x => GetOccupiedBedIdsByRoomId.Contains(x.pId)).OrderBy(x => (leaveDate - x.opnameDatum.AddDays(x.aantalDagen)));
            foreach (var patientVar in patientsOnOccupiedRoom)
            {
                var occupiedbed = data.bedden.FirstOrDefault(x => x.pId == patientVar.pId);

                if (patientVar.opnameDatum.AddDays(patientVar.aantalDagen) < patient.opnameDatum)
                {
                    beds.Add(new BedPredictionModel { bed = occupiedbed, Roomstatus = RoomStatus.HalfOccupied, Gender = patient.geslacht, LeaveDateCurrentPatient = patientVar.opnameDatum.AddDays(patientVar.aantalDagen) });
                }
                var emptybed = data.bedden.FirstOrDefault(x => x.kId == occupiedbed.kId && x.pId == 0);
                if (emptybed != null)
                { beds.Add(new BedPredictionModel { bed = emptybed, Roomstatus = RoomStatus.HalfOccupied, Gender = patient.geslacht }); }
            }
            return beds;
        }
        private List<int> GetPossibleKamersByGender(String gender)
        {
            var data = dataImporter.getData();
            var patientsByGender = data.patienten.Where(x => x.geslacht == gender).Select(x => x.pId).ToList();
            var GetOccupiedBedsByGender = new List<Bed>();
            foreach (var patientId in patientsByGender)
            {
                GetOccupiedBedsByGender.Add(data.bedden.First(x => x.pId == patientId));
            }
            var GetOccupiedRooms = GetOccupiedBedsByGender.GroupBy(x => x.kId).Select(x => new { KamerId = x.Key, AmountOfOccupiedBeds = x.Count() }).ToList();
            var PossibleRooms = GetOccupiedRooms.Select(x => x.KamerId);
            return PossibleRooms.ToList();
        }
        public List<BedPredictionModel> GetBedsByDateAndGender(Patient patient)
        {
            var data = dataImporter.getData();
            var patientsByGenderAndLeaveDate = data.patienten.Where(x => x.geslacht == patient.geslacht && x.opnameDatum.AddDays(x.aantalDagen) == patient.opnameDatum).OrderBy(x => patient.opnameDatum - x.opnameDatum.AddDays(x.aantalDagen)).ToList();
            List<BedPredictionModel> beds = new List<BedPredictionModel>();
            foreach (var leavingPatient in patientsByGenderAndLeaveDate)
            {
                var bed = data.bedden.FirstOrDefault(x => x.pId == leavingPatient.pId);
                if (bed != null)
                {
                    var model = new BedPredictionModel { bed = bed, LeaveDateCurrentPatient = leavingPatient.opnameDatum.AddDays(leavingPatient.aantalDagen), Gender = patient.geslacht };
                    if (data.bedden.Count(x => x.kId == bed.kId && x.pId != 0) == 2)
                    {
                        model.Roomstatus = RoomStatus.Full;
                    }
                    model.Roomstatus = RoomStatus.HalfOccupied;
                    beds.Add(model);
                }
            }
            return beds;

        }

    }
}
