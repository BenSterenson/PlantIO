using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantIO.Modules
{
    public class PlantIOSample
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string date { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string time { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }

        [JsonProperty(PropertyName = "scale")]
        public string scale { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int value { get; set; }


        public PlantIOSample() { }

        public PlantIOSample(int id, string date, string time, string type, string scale, int value)
        {
            this.id = id;
            this.date = date;
            this.time = time;
            this.type = type;
            this.scale = scale;
            this.value = value;
        }

    }

    public class PlantIORestApi
    {
        [JsonProperty(PropertyName = "rows")]
        public List<PlantIOSample> rows { get; set; }

        public PlantIORestApi() { }

        public PlantIORestApi(List<PlantIOSample> arr)
        {
            this.rows = arr;
        }
    }
}