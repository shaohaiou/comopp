using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    public class CityInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonIgnore]
        public int ProvinceID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string AreaCode { get; set; }
    }
}
