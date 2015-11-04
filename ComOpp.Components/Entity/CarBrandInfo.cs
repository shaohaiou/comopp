using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class CarBrandInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string NameIndex { get; set; }

        [JsonProperty("name")]
        public string BindName
        {
            get
            {
                return NameIndex + "-" + Name;
            }
        }
    }
}
