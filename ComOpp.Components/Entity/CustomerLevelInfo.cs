using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class CustomerLevelInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 降级天数
        /// </summary>
        [JsonProperty("drtday")]
        public int Drtday { get; set; }

        /// <summary>
        /// 报警天数
        /// </summary>
        [JsonProperty("alarmday")]
        public int Alarmday { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        public int Sort { get; set; }

        [JsonIgnore]
        public int CorporationID { get; set; }
    }
}
