using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantIO.Modules
{
    class PlantIOHandshake
    {
        [JsonProperty(PropertyName = "host")]
        public string host { get; set; }

        [JsonProperty(PropertyName = "port")]
        public string port { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string path { get; set; }

        [JsonProperty(PropertyName = "expiry")]
        public string expiry { get; set; }


        public PlantIOHandshake() {}
        public PlantIOHandshake(string host, string port, string path, string expiry)
        {
            this.host = host;
            this.port = port;
            this.path = path;
            this.expiry = expiry;
        }

    }
}
