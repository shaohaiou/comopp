using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class IbuytimeInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 数据等级
        /// <para>0：系统默认数据</para>
        /// </summary>
        [JsonProperty("datalevel")]
        public int DataLevel { get; set; }

        /// <summary>
        /// 所属公司
        /// <para>0：系统数据</para>
        /// </summary>
        [JsonIgnore]
        public int CorporationID { get; set; }
    }
}
