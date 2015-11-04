using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class TalkInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [JsonIgnore]
        public string Content { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [JsonProperty("realname")]
        public string Realname { get; set; }

        /// <summary>
        /// 发布人ID
        /// </summary>
        [JsonIgnore]
        public int PublicUserID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [JsonProperty("addtime")]
        public DateTime AddTime { get; set; }

        [JsonIgnore]
        public int CorporationID { get; set; }
    }
}
