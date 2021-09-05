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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataImporter _dataImporter;
        public List<Patient> patients;

        public IndexModel(ILogger<IndexModel> logger, IDataImporter dataImporter)
        {
            _logger = logger;
            _dataImporter = dataImporter;
        }

        public void OnGet()
        {
            patients = _dataImporter.getData().patienten;
        }
    }
}
