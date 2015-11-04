using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class PowerGroupInfo
    {
        #region 属性

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 账户组名称
        /// </summary>
        [JsonProperty("groupname")]
        public string GroupName { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        [JsonProperty("powergroup")]
        public string GroupPower { get; set; }

        /// <summary>
        /// 所属公司ID
        /// </summary>
        [JsonIgnore]
        public int CorporationID { get; set; }

        /// <summary>
        /// 所属公司名称
        /// </summary>
        [JsonProperty("corporationname")]
        public string CorporationName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [JsonProperty("lastupdatetime")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 成员数量
        /// </summary>
        [JsonProperty("membercount")]
        public int MemberCount { get; set; }

        /// <summary>
        /// 查看其他权限组线索
        /// </summary>
        [JsonIgnore]
        public string CanviewGroupIds { get; set; }

        #endregion Model
    }
}
