using BedcapacityApp_bap_sebastiaan.Models;
using BedcapacityApp_bap_sebastiaan.Interfaces.Services;
using Newtonsoft.Json;
using System.IO;

namespace BedcapacityApp_bap_sebastiaan.Services
{
    public class JsonDataImporter : IDataImporter

    {
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
            using (StreamReader r = new StreamReader("Data.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<DataObjects>(json);
            }

        }


    }
}
