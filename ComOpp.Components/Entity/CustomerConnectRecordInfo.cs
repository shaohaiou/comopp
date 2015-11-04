using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    public class CustomerConnectRecordInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonIgnore]
        public int CustomerID { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        [JsonProperty("customername")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户电话
        /// </summary>
        [JsonProperty("customerphone")]
        public string CustomerPhone { get; set; }

        [JsonIgnore]
        public int ConnectUserID { get; set; }

        /// <summary>
        /// 追踪人
        /// </summary>
        [JsonProperty("connnectuser")]
        public string ConnectUser { get; set; }

        /// <summary>
        /// 追踪时间
        /// </summary>
        [JsonProperty("followtime")]
        public string FollowTime { get; set; }

        [JsonIgnore]
        public int ConnectwayID { get; set; }

        [JsonProperty("connectway")]
        public string Connectway
        {
            get
            {
                return ConnectwayID == 0 ? string.Empty : ConnectWays.Instance.GetName(ConnectwayID);
            }
        }

        /// <summary>
        /// 追踪级别ID
        /// </summary>
        [JsonIgnore]
        public int CustomerLevelID { get; set; }

        /// <summary>
        /// 追踪级别
        /// </summary>
        [JsonProperty("customerlevel")]
        public string CustomerLevel
        {
            get
            {
                return CustomerLevelID == 0 ? string.Empty : CustomerLevels.Instance.GetName(CustomerLevelID);
            }
        }

        /// <summary>
        /// 追踪情况
        /// </summary>
        [JsonProperty("connectdetail")]
        public string ConnectDetail { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonProperty("createtime")]
        public string CreateTime { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [JsonIgnore]
        public int CorporationID { get; set; }
    }
}
