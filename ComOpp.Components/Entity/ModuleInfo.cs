using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class ModuleInfo
    {
        #region 属性

        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [JsonProperty("modulename")]
        public string ModuleName { get; set; }

        /// <summary>
        /// 上级别模块名
        /// </summary>
        [JsonProperty("parentname")]
        public string ParentName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        public int Sort { get; set; }

        #endregion
    }
}
