using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Newtonsoft.Json;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.Components
{
    //事件类型
    public enum EventType
    {
        [Description("全部")]
        Entire = -1,//全部
        [Description("信息")]
        Information = 0,//信息
        [Description("警告")]
        Warning = 1,//警告
        [Description("错误")]
        Error = 2,//错误
        [Description("调试信息")]
        Debug = 3//调试信息
    }

    /// <summary>
    /// 事件日志实体类
    /// </summary>
    [Serializable]
    public class EventLogEntry
    {
        /// <summary>
        /// 信息
        /// </summary>
        [JsonIgnore]
        public string Message
        {
            get;
            set;
        }

        [JsonProperty("message")]
        public string MessageShort
        {
            get
            {
                return StrHelper.GetSubString(Message, 100, "...");
            }
        }

        /// <summary>
        /// 机器名
        /// </summary>
        [JsonIgnore]
        public string MachineName
        {
            get;
            set;
        }

        /// <summary>
        /// 分类
        /// </summary>
        [JsonIgnore]
        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// 实体ID
        /// </summary>
        [JsonProperty("id")]
        public int EntryID
        {
            get;
            set;
        }

        /// <summary>
        /// 事件ID
        /// </summary>
        [JsonProperty("eventid")]
        public int EventID
        {
            get;
            set;
        }

        /// <summary>
        /// 事件日期
        /// </summary>
        [JsonIgnore]
        public DateTime EventDate
        {
            get;
            set;
        }

        [JsonProperty("eventdate")]
        public string EventDateString
        {
            get
            {
                return EventDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        [JsonIgnore]
        public EventType EventType
        {
            get;
            set;
        }

        [JsonProperty("eventtype")]
        public string EventTypeName
        {
            get
            {
                return EnumExtensions.GetDescription<EventType>(((int)EventType).ToString());
            }
        }

        /// <summary>
        /// 应用程序名
        /// </summary>
        [JsonIgnore]
        public string ApplicationName { get; set; }

        /// <summary>
        /// 应用程序ID
        /// </summary>
        [JsonIgnore]
        public int ApplicationID { get; set; }

        /// <summary>
        /// 应用程序类型
        /// </summary>
        [JsonIgnore]
        public ApplicationType ApplicationType { get; set; }
        [JsonIgnore]
        public int PCount { get; set; }

        [JsonIgnore]
        public DateTime AddTime { get; set; }

        [JsonIgnore]
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 相关错误
        /// </summary>
        public Exception Ex { get; set; }

        [JsonIgnore]
        public string Uniquekey { get; set; }
    }
}
