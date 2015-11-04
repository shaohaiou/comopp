using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class InfoTypeInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 锁定级别状态
        /// </summary>
        [JsonProperty("locked")]
        public int Locked { get; set; }

        /// <summary>
        /// 锁定级别天数
        /// </summary>
        [JsonProperty("lockday")]
        public int Lockday { get; set; }

        /// <summary>
        /// 锁定级别
        /// </summary>
        [JsonProperty("locklevel")]
        public string Locklevel { get; set; }

        /// <summary>
        /// 锁定级别名称
        /// </summary>
        [JsonProperty("locklevelname")]
        public string LocklevelName { get; set; }

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

        /// <summary>
        /// 系统数据ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 系统数据的Name
        /// </summary>
        public string ParentName { get; set; }

        [JsonProperty("showname")]
        public string ShowName
        {
            get
            {
                if (ParentID > 0) return ParentName;
                return Name;
            }
        }
    }
}
