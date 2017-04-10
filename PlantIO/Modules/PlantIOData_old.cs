using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantIO.Modules
{
    class PlantIOData_old
    {
        [JsonProperty(PropertyName = "src")]
        public PlantIOSrc src { get; set; }

        [JsonProperty(PropertyName = "scale")]
        public string scale { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int value { get; set; }


        public PlantIOData_old() {}

        public PlantIOData_old(PlantIOSrc src, string scale, int value)
        {
            this.src = src;
            this.scale = scale;
            this.value = value;
        }

    }

    public class PlantIOSrc
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }

        public PlantIOSrc() { }

        public PlantIOSrc(string id, string type)
        {
            this.id = id;
            this.type = type;
        }
    }
}
