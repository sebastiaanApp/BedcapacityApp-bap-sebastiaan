using BedcapacityApp_bap_sebastiaan.Models;
using BedcapacityApp_bap_sebastiaan.Interfaces.Services;
using Newtonsoft.Json;
using System.IO;
using BedcapacityApp_bap_sebastiaan.Configurations;
using Microsoft.Extensions.Options;

namespace BedcapacityApp_bap_sebastiaan.Services
{
    public class JsonDataImporter : IDataImporter

    {
        private readonly DataConfiguration dataConfiguration;
        public JsonDataImporter(IOptions<DataConfiguration> dataConfig)
        {
            dataConfiguration = dataConfig.Value;
        }

        /*public void SeedData()
        {
            using (StreamReader r = new StreamReader("Data.json"))
            {
                string json = r.ReadToEnd();
                DataObjects items = JsonConvert.DeserializeObject<DataObjects>(json);
            }
        }*/

        //1 keer data inladen
        public DataObjects getData()
        {
            // alle data in dataObject steken
            using (StreamReader r = new StreamReader(dataConfiguration.DataFileName))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<DataObjects>(json);
            }

        }


    }
}
