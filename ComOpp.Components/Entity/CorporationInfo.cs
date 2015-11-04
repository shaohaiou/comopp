using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComOpp.Tools;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    /// <summary>
    /// 公司
    /// </summary>
    [Serializable]
    public class CorporationInfo : ExtendedAttributes
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 授权品牌ID
        /// </summary>
        [JsonIgnore]
        public int BrandID
        {
            get { return GetInt("BrandID", 0); }
            set { SetExtendedAttribute("BrandID", value.ToString()); }
        }

        /// <summary>
        /// 所属区域（省）
        /// </summary>
        [JsonIgnore]
        public int ProvinceID
        {
            get { return GetInt("ProvinceID", 0); }
            set { SetExtendedAttribute("ProvinceID", value.ToString()); }
        }

        /// <summary>
        /// 所属区域（市）
        /// </summary>
        [JsonIgnore]
        public int CityID
        {
            get { return GetInt("CityID", 0); }
            set { SetExtendedAttribute("CityID", value.ToString()); }
        }

        /// <summary>
        /// 所属区域（地区）
        /// </summary>
        [JsonIgnore]
        public int DistrictID
        {
            get { return GetInt("DistrictID", 0); }
            set { SetExtendedAttribute("DistrictID", value.ToString()); }
        }

        /// <summary>
        /// DCC电话营销员负责邀约到店
        /// </summary>
        [JsonIgnore]
        public int IsProcess
        {
            get { return GetInt("IsProcess", 1); }
            set { SetExtendedAttribute("IsProcess", value.ToString()); }
        }

        /// <summary>
        /// 主动转出
        /// <para>导入|集客 -> 潜客数据库</para>
        /// </summary>
        [JsonIgnore]
        public int Trackmove
        {
            get { return GetInt("Trackmove", 1); }
            set { SetExtendedAttribute("Trackmove", value.ToString()); }
        }

        /// <summary>
        /// 转出审核
        /// <para>导入|集客 -> 潜客数据库</para>
        /// </summary>
        [JsonIgnore]
        public int Movecheck
        {
            get { return GetInt("Movecheck", 1); }
            set { SetExtendedAttribute("Movecheck", value.ToString()); }
        }

        /// <summary>
        /// 强制转出天数
        /// <para>清洗|邀约 -> 潜客数据库</para>
        /// </summary>
        [JsonIgnore]
        public int Forcedoffday
        {
            get { return GetInt("Forcedoffday", 0); }
            set { SetExtendedAttribute("Forcedoffday", value.ToString()); }
        }

        /// <summary>
        /// 强制转出忽略客户级别
        /// <para>清洗|邀约 -> 潜客数据库</para>
        /// </summary>
        [JsonIgnore]
        public string Offcustomerlevel
        {
            get { return GetString("Offcustomerlevel", ""); }
            set { SetExtendedAttribute("Offcustomerlevel", value); }
        }

        /// <summary>
        /// 主动转出天数
        /// <para>清洗|邀约 -> 潜客数据库</para>
        /// </summary>
        [JsonIgnore]
        public int Voluntaryoffday
        {
            get { return GetInt("Voluntaryoffday", 0); }
            set { SetExtendedAttribute("Voluntaryoffday", value.ToString()); }
        }

        /// <summary>
        /// 主动转出审核
        /// <para>清洗|邀约 -> 潜客数据库</para>
        /// </summary>
        [JsonIgnore]
        public int Offcheck
        {
            get { return GetInt("Offcheck", 1); }
            set { SetExtendedAttribute("Offcheck", value.ToString()); }
        }

        /// <summary>
        /// 强制转出天数
        /// <para>追踪|促成 -> 清洗|邀约</para>
        /// </summary>
        [JsonIgnore]
        public int Forcedoutday
        {
            get { return GetInt("Forcedoutday", 0); }
            set { SetExtendedAttribute("Forcedoutday", value.ToString()); }
        }

        /// <summary>
        /// 强制转出忽略客户级别
        /// <para>追踪|促成 -> 清洗|邀约</para>
        /// </summary>
        [JsonIgnore]
        public string Forcedoutdaylevel
        {
            get { return GetString("Forcedoutdaylevel", ""); }
            set { SetExtendedAttribute("Forcedoutdaylevel", value); }
        }

        /// <summary>
        /// 主动转出天数
        /// <para>追踪|促成 -> 清洗|邀约</para>
        /// </summary>
        [JsonIgnore]
        public int Voluntaryoutday
        {
            get { return GetInt("Voluntaryoutday", 0); }
            set { SetExtendedAttribute("Voluntaryoutday", value.ToString()); }
        }

        /// <summary>
        /// 主动转出审核
        /// <para>追踪|促成 -> 清洗|邀约</para>
        /// </summary>
        [JsonIgnore]
        public int Outcheck
        {
            get { return GetInt("Outcheck", 1); }
            set { SetExtendedAttribute("Outcheck", value.ToString()); }
        }
    }
}
