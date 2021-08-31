using System;
using System.Collections.Generic;
using BedcapacityApp_bap_sebastiaan.Models;

namespace BedcapacityApp_bap_sebastiaan.Interfaces.Services
{
    public interface IPredictionService
    {
        List<BedPredictionModel> GetBestBeds(Patient patient);
        List<BedPredictionModel> GetBedsByDateAndGender(Patient patient);
        List<BedPredictionModel> GetAlternativeBeds(Patient patient);
    }
}
