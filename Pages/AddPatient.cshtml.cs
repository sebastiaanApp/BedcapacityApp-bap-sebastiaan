using BedcapacityApp_bap_sebastiaan.Interfaces.Services;
using BedcapacityApp_bap_sebastiaan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BedcapacityApp_bap_sebastiaan.Pages
{
    public class AddPatientModel : PageModel
    {
        private readonly ILogger<AddPatientModel> _logger;
        private readonly IPredictionService predictionService;

        public AddPatientModel(ILogger<AddPatientModel> logger, IPredictionService predictionService)
        {
            _logger = logger;
            this.predictionService = predictionService;
        }

        public void OnGet()
        {

        }
        public void OnPost()
        {
            optimalBeds = predictionService.GetBestBeds(new Patient { geslacht = geslacht, opnameDatum = opnameDatum, aantalDagen = aantalDagen });
            var alternative = predictionService.GetAlternativeBeds(new Patient { geslacht = geslacht, opnameDatum = opnameDatum, aantalDagen = aantalDagen });
            var toRemove = alternative.Where(x => optimalBeds.Select(x => x.bed.bId).Contains(x.bed.bId)).ToList();

            foreach (var item in toRemove)
            {
                alternative.Remove(item);
            }
            alternativeBeds = alternative;
        }

        [BindProperty]

        public string patientnaam { get; set; }
        [BindProperty]

        public string geslacht { get; set; }
        [BindProperty]

        public int aantalDagen { get; set; }
        [BindProperty]

        public DateTime opnameDatum { get; set; }
        [BindProperty]

        public List<BedPredictionModel> optimalBeds { get; set; }
        public List<BedPredictionModel> alternativeBeds { get; set; }
    }
}
