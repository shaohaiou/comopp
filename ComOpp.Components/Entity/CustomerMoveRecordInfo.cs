using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ComOpp.Tools;

namespace ComOpp.Components
{
    public class CustomerMoveRecordInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonIgnore]
        public int CustomerID { get; set; }

        /// <summary>
        /// 线索状态
        /// </summary>
        [JsonProperty("state")]
        public int CustomerStatus { get; set; }

        /// <summary>
        /// 线索状态名称
        /// </summary>
        [JsonProperty("customerstatus")]
        public string CustomerStatusName
        {
            get
            {
                return EnumExtensions.GetDescription<CustomerStatus>(Enum.GetName(typeof(CustomerStatus), CustomerStatus));
            }
        }

        /// <summary>
        /// 原始线索状态
        /// </summary>
        [JsonProperty("stated")]
        public int CustomerStatusSource { get; set; }

        /// <summary>
        /// 线索状态名称
        /// </summary>
        [JsonProperty("customerstatussource")]
        public string CustomerStatusSourceName
        {
            get
            {
                return CustomerStatusSource == 0 ? string.Empty : EnumExtensions.GetDescription<CustomerStatus>(Enum.GetName(typeof(CustomerStatus), CustomerStatusSource));
            }
        }

        /// <summary>
        /// 线索所有人ID
        /// </summary>
        [JsonProperty("ownerid")]
        public int OwnerID { get; set; }

        /// <summary>
        /// 线索所有人
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// 最后操作人ID
        /// </summary>
        [JsonIgnore]
        public int LastUpdateUserID { get; set; }

        /// <summary>
        /// 最后操作人
        /// </summary>
        [JsonProperty("lastupdateuser")]
        public string LastUpdateUser { get; set; }

        [JsonProperty("createtime")]
        public string CreateTime { get; set; }

        /// <summary>
        /// 系统消息
        /// </summary>
        [JsonProperty("systemmsg")]
        public string SystemMsg { get; set; }
    }
}
