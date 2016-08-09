using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComOpp.Tools;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    [Serializable]
    public class AdminInfo : ExtendedAttributes
    {
        #region 属性

        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// 用户是否是超级管理员
        /// </summary>
        [JsonIgnore]
        public bool Administrator { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [JsonIgnore]
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [JsonProperty("lastlogin")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        [JsonIgnore]
        public UserRoleType UserRole { get; set; }

        /// <summary>
        /// 权限组ID
        /// </summary>
        [JsonIgnore]
        public int PowerGroupID { get; set; }

        /// <summary>
        /// 权限组名称
        /// </summary>
        [JsonProperty("group")]
        public string PowerGroupName { get; set; }

        /// <summary>
        /// 权限组的权限值
        /// </summary>
        [JsonProperty("power")]
        public string GroupPower { get; set; }

        /// <summary>
        /// 密码明文
        /// </summary>
        [JsonProperty("passwordtext")]
        public string PasswordText
        {
            get { return GetString("PasswordText", ""); }
            set { SetExtendedAttribute("PasswordText", value); }
        }

        /// <summary>
        /// 登录次数
        /// </summary>
        [JsonProperty("logintimes")]
        public int LoginTimes
        {
            get { return GetInt("logintimes", 0); }
            set { SetExtendedAttribute("logintimes", value.ToString()); }
        }

        /// <summary>
        /// 状态
        /// <para>1：正常；2：锁定</para>
        /// </summary>
        [JsonProperty("state")]
        public string State
        {
            get { return GetString("State", "1"); }
            set { SetExtendedAttribute("State", value); }
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonProperty("mobile")]
        public string Mobile
        {
            get { return GetString("Mobile", ""); }
            set { SetExtendedAttribute("Mobile", value); }
        }

        /// <summary>
        /// QQ号
        /// </summary>
        [JsonProperty("qq")]
        public string QQ
        {
            get { return GetString("QQ", ""); }
            set { SetExtendedAttribute("QQ", value); }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [JsonProperty("realname")]
        public string Realname
        {
            get { return GetString("Realname", ""); }
            set { SetExtendedAttribute("Realname", value); }
        }

        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        public int Sex
        {
            get { return GetInt("Sex", 0); }
            set { SetExtendedAttribute("Sex", value.ToString()); }
        }

        /// <summary>
        /// 所属公司ID
        /// </summary>
        [JsonIgnore]
        public int CorporationID
        {
            get
            {
                return GetInt("CorporationID", 0);
            }
            set { SetExtendedAttribute("CorporationID", value.ToString()); }
        }

        /// <summary>
        /// 所属公司
        /// </summary>
        [JsonProperty("corp")]
        public string Corporation
        {
            get
            {
                if (Administrator) return "超级管理员";
                return GetString("Corporation", "");
            }
            set { SetExtendedAttribute("Corporation", value); }
        }

        #endregion Model

        [JsonProperty("realnameandgroupname")]
        public string RealnameAndGroupname
        {
            get
            {
                return Realname + "(" + PowerGroupName +")";
            }
        }
    }
}
