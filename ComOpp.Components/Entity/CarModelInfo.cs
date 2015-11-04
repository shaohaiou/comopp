using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;


namespace ComOpp.Components
{
    [Serializable]
    public class CarModelInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public int SeriesID { get; set; }

        [JsonIgnore]
        public string SeriesName { get; set; }

        [JsonIgnore]
        public int BrandID { get; set; }

        [JsonIgnore]
        public string BrandName { get; set; }

        [JsonIgnore]
        public string BrandNameIndex { get; set; }
    }
}
