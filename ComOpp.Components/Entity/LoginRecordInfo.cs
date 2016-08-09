using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class LoginRecordInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("adminid")]
        public int AdminID { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("lastloginip")]
        public string LastLoginIP { get; set; }

        [JsonProperty("logintime")]
        public DateTime LoginTime { get; set; }
    }
}
