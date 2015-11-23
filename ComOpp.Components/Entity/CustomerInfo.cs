using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ComOpp.Tools;

namespace ComOpp.Components
{
    [Serializable]
    public class CustomerInfo
    {
        public CustomerInfo()
        {
            Address = string.Empty;
            QuotedpriceInfo = string.Empty;
            PromotionInfo = string.Empty;
            Owner = string.Empty;
            TracktagID = string.Empty;
            Tracktag = string.Empty;
        }

        [JsonProperty("id")]
        public int ID { get; set; }

        #region 基本信息

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 客户电话
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 号码归属
        /// </summary>
        [JsonProperty("phonevest")]
        public string PhoneVest { get; set; }

        /// <summary>
        /// 备用电话
        /// </summary>
        [JsonProperty("backupphone")]
        public string BackupPhone { get; set; }

        /// <summary>
        /// 省份ID
        /// </summary>
        [JsonIgnore]
        public int ProvinceID { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        [JsonProperty("province")]
        public string Province
        {
            get
            {
                return Districts.Instance.GetProvinceName(ProvinceID);
            }
        }

        /// <summary>
        /// 市ID
        /// </summary>
        [JsonIgnore]
        public int CityID { get; set; }

        /// <summary>
        /// 市名称
        /// </summary>
        [JsonProperty("city")]
        public string City
        {
            get
            {
                return Districts.Instance.GetCityName(CityID);
            }
        }

        /// <summary>
        /// 地区ID
        /// </summary>
        [JsonIgnore]
        public int DistrictID { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [JsonProperty("district")]
        public string District
        {
            get
            {
                return Districts.Instance.GetDistrictName(DistrictID);
            }
        }

        /// <summary>
        /// 具体地址
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// 微信帐号
        /// </summary>
        [JsonProperty("weixinaccount")]
        public string WeixinAccount { get; set; }

        #endregion

        #region 拟购信息

        /// <summary>
        /// 信息类型ID
        /// </summary>
        [JsonIgnore]
        public int InfoTypeID { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
        [JsonProperty("infotype")]
        public string InfoType
        {
            get
            {
                return InfoTypeID == 0 ? "-" : InfoTypes.Instance.GetName(InfoTypeID);
            }
        }

        /// <summary>
        /// 信息来源ID
        /// </summary>
        [JsonIgnore]
        public int InfoSourceID { get; set; }

        /// <summary>
        /// 信息来源
        /// </summary>
        [JsonProperty("infosource")]
        public string InfoSource
        {
            get
            {
                return InfoSourceID == 0 ? "-" : InfoSources.Instance.GetName(InfoSourceID);
            }
        }

        /// <summary>
        /// 支付方式ID
        /// </summary>
        [JsonIgnore]
        public int PaymentWayID { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        [JsonProperty("paymentway")]
        public string PaymentWay
        {
            get
            {
                return PaymentWayID == 0 ? "-" : PaymentWays.Instance.GetName(PaymentWayID);
            }
        }

        /// <summary>
        /// 拟购品牌ID
        /// </summary>
        [JsonIgnore]
        public int IbuyCarBrandID { get; set; }

        /// <summary>
        /// 拟购品牌
        /// </summary>
        [JsonProperty("ibuycarbrand")]
        public string IbuyCarBrand
        {
            get
            {
                return IbuyCarBrandID == 0 ? "-" : Cars.Instance.GetCarBrandName(IbuyCarBrandID);
            }
        }

        /// <summary>
        /// 拟购车系ID
        /// </summary>
        [JsonIgnore]
        public int IbuyCarSeriesID { get; set; }

        /// <summary>
        /// 拟购车系
        /// </summary>
        [JsonProperty("ibuycarseries")]
        public string IbuyCarSeries
        {
            get
            {
                return IbuyCarSeriesID == 0 ? "-" : Cars.Instance.GetCarSeriesName(IbuyCarSeriesID);
            }
        }

        /// <summary>
        /// 拟购车型ID
        /// </summary>
        [JsonIgnore]
        public int IbuyCarModelID { get; set; }

        /// <summary>
        /// 拟购车型
        /// </summary>
        [JsonProperty("ibuycarmodel")]
        public string IbuyCarModel
        {
            get
            {
                return IbuyCarModelID == 0 ? "-" : Cars.Instance.GetCarModelName(IbuyCarModelID);
            }
        }

        /// <summary>
        /// 拟购时间ID
        /// </summary>
        [JsonIgnore]
        public int IbuyTimeID { get; set; }

        /// <summary>
        /// 拟购时间
        /// </summary>
        [JsonProperty("ibuytime")]
        public string IbuyTime
        {
            get
            {
                return IbuyTimeID == 0 ? "-" : Ibuytimes.Instance.GetName(IbuyTimeID);
            }
        }
        
        #endregion

        #region 沟通信息

        /// <summary>
        /// 报价信息
        /// </summary>
        [JsonProperty("quotedpriceinfo")]
        public string QuotedpriceInfo { get; set; }

        /// <summary>
        /// 促销信息
        /// </summary>
        [JsonProperty("promotioninfo")]
        public string PromotionInfo { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [JsonProperty("remarkinfo")]
        public string RemarkInfo { get; set; }

        #endregion

        #region 其他

        /// <summary>
        /// 线索创建人ID
        /// </summary>
        [JsonProperty("createuserid")]
        public int CreateUserID { get; set; }

        /// <summary>
        /// 线索创建人
        /// </summary>
        [JsonProperty("createuser")]
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("createtime")]
        public string CreateTime { get; set; }

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
        /// 线索所有人的权限组ID
        /// </summary>
        [JsonIgnore]
        public int OwnerPowerGroupID { get; set; }

        /// <summary>
        /// 客户性别
        /// </summary>
        [JsonProperty("sex")]
        public int CustomerSex { get; set; }

        /// <summary>
        /// 线索标签ID（多标签）
        /// </summary>
        [JsonIgnore]
        public string TracktagID { get; set; }

        /// <summary>
        /// 线索标签
        /// </summary>
        [JsonProperty("tracktag")]
        public string Tracktag { get; set; }

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
        /// 最近追踪级别ID
        /// </summary>
        [JsonProperty("lastcustomerlevelid")]
        public int LastCustomerLevelID { get; set; }

        /// <summary>
        /// 最近追踪级别
        /// </summary>
        [JsonProperty("lastcustomerlevel")]
        public string LastCustomerLevel
        {
            get
            {
                return LastCustomerLevelID == 0 ? "-" : CustomerLevels.Instance.GetName(LastCustomerLevelID);
            }
        }

        /// <summary>
        /// 最近追踪方式ID
        /// </summary>
        [JsonIgnore]
        public int LastConnectwayID { get; set; }

        /// <summary>
        /// 最近追踪方式
        /// </summary>
        [JsonProperty("lastconnectway")]
        public string LastConnectway
        {
            get
            {
                return LastConnectwayID == 0 ? "-" : ConnectWays.Instance.GetName(LastConnectwayID);
            }
        }

        /// <summary>
        /// 最近追踪时间
        /// </summary>
        [JsonProperty("lastconnecttime")]
        public string LastConnectTime { get; set; }

        /// <summary>
        /// 最近追踪人ID
        /// </summary>
        [JsonIgnore]
        public int LastConnectUserID { get; set; }

        /// <summary>
        /// 最近追踪人
        /// </summary>
        [JsonProperty("lastconnectuser")]
        public string LastConnectUser { get; set; }

        /// <summary>
        /// 最近追踪情况
        /// </summary>
        [JsonProperty("lastconnectdetail")]
        public string LastConnectDetail { get; set; }

        /// <summary>
        /// 预约到店时间
        /// </summary>
        [JsonProperty("reservationtime")]
        public string ReservationTime { get; set; }

        /// <summary>
        /// 到店时间
        /// </summary>
        [JsonProperty("visittime")]
        public string VisitTime { get; set; }

        /// <summary>
        /// 离开时间
        /// </summary>
        [JsonProperty("leavetime")]
        public string LeaveTime { get; set; }

        /// <summary>
        /// 接待时长（分钟）
        /// </summary>
        [JsonProperty("visitduration")]
        public int VisitDuration { get; set; }

        /// <summary>
        /// 来店人数
        /// </summary>
        [JsonProperty("visitnumber")]
        public int VisitNumber { get; set; }

        /// <summary>
        /// 是否到店
        /// </summary>
        [JsonProperty("isvisit")]
        public int IsVisit { get; set; }

        /// <summary>
        /// 追踪报警
        /// </summary>
        [JsonProperty("connectalarm")]
        public string ConnectAlarm
        {
            get
            {
                if (LastCustomerLevelID == 0)
                {
                    DateTime createtime = DateTime.Parse(CreateTime);
                    if (DateTime.Now.Subtract(createtime).TotalDays <= 6) return "0";
                    if (DateTime.Now.Subtract(createtime).TotalDays <= 7) return "1";
                    else return "2";
                }
                else
                {
                    CustomerLevelInfo lastcustomerlevel = CustomerLevels.Instance.GetModel(LastCustomerLevelID, true);
                    if (lastcustomerlevel != null)
                    {
                        DateTime lastconnecttime = DateTime.Parse(LastConnectTime);
                        if (DateTime.Now.Subtract(lastconnecttime).TotalDays <= (lastcustomerlevel.Alarmday - 1) || lastcustomerlevel.Alarmday == 0) return "0";
                        if (DateTime.Now.Subtract(lastconnecttime).TotalDays <= lastcustomerlevel.Alarmday) return "1";
                        else return "2";
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 追踪次数
        /// </summary>
        [JsonProperty("tracetimes")]
        public int ConnectTimes { get; set; }

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

        /// <summary>
        /// 最后操作时间
        /// </summary>
        [JsonProperty("lastupdatetime")]
        public string LastUpdateTime { get; set; }

        /// <summary>
        /// 选购品牌ID
        /// </summary>
        [JsonIgnore]
        public int SbuyCarBrandID { get; set; }

        /// <summary>
        /// 选购品牌
        /// </summary>
        [JsonProperty("sbuycarbrand")]
        public string SbuyCarBrand
        {
            get
            {
                return SbuyCarBrandID == 0 ? "-" : Cars.Instance.GetCarBrandName(SbuyCarBrandID);
            }
        }

        /// <summary>
        /// 选购车系ID
        /// </summary>
        [JsonIgnore]
        public int SbuyCarSeriesID { get; set; }

        /// <summary>
        /// 选购车系
        /// </summary>
        [JsonProperty("sbuycarseries")]
        public string SbuyCarSeries
        {
            get
            {
                return SbuyCarSeriesID == 0 ? "-" : Cars.Instance.GetCarSeriesName(SbuyCarSeriesID);
            }
        }

        /// <summary>
        /// 选购车型ID
        /// </summary>
        [JsonIgnore]
        public int SbuyCarModelID { get; set; }

        /// <summary>
        /// 选购车型
        /// </summary>
        [JsonProperty("sbuycarmodel")]
        public string SbuyCarModel
        {
            get
            {
                return SbuyCarModelID == 0 ? "-" : Cars.Instance.GetCarModelName(SbuyCarModelID);
            }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("ordernumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        [JsonProperty("knockdownprice")]
        public string KnockdownPrice { get; set; }

        /// <summary>
        /// 预订成交时间
        /// </summary>
        [JsonProperty("placeordertime")]
        public string PlaceOrderTime { get; set; }

        /// <summary>
        /// 提车时间
        /// </summary>
        [JsonProperty("picupcartime")]
        public string PicupcarTime { get; set; }

        /// <summary>
        /// 失败原因ID
        /// </summary>
        [JsonIgnore]
        public int GiveupCauseID { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        [JsonProperty("givecause")]
        public string GiveupCause
        {
            get
            {
                return GiveupCauseID == 0 ? "-" : GiveupCauses.Instance.GetName(GiveupCauseID);
            }
        }

        /// <summary>
        /// 失败原因分析
        /// </summary>
        [JsonProperty("failurecauseanalyze")]
        public string FailureCauseAnalyze { get; set; }

        /// <summary>
        /// 市场专员ID
        /// </summary>
        [JsonIgnore]
        public int MarketDirectorID { get; set; }

        /// <summary>
        /// 市场专员
        /// </summary>
        [JsonProperty("marketdirector")]
        public string MarketDirector { get; set; }

        /// <summary>
        /// DCC专员ID
        /// </summary>
        [JsonIgnore]
        public int DCCDirectorID { get; set; }

        /// <summary>
        /// DCC专员
        /// </summary>
        [JsonProperty("dccdirector")]
        public string DCCDirector { get; set; }

        /// <summary>
        /// 展厅专员ID
        /// </summary>
        [JsonIgnore]
        public int ExhibitionDirectorID { get; set; }

        /// <summary>
        /// 展厅专员
        /// </summary>
        [JsonProperty("exhibitiondirector")]
        public string ExhibitionDirector { get; set; }

        /// <summary>
        /// 直销专员ID
        /// </summary>
        [JsonIgnore]
        public int DirectorID { get; set; }

        /// <summary>
        /// 直销专员
        /// </summary>
        [JsonProperty("director")]
        public string Director { get; set; }

        /// <summary>
        /// 自动编号
        /// </summary>
        [JsonProperty("showno")]
        public string ShowNo
        {
            get 
            {
                return CorporationID.ToString("000") + ID.ToString("000000");
            }
        }

        /// <summary>
        /// 系统备注
        /// </summary>
        [JsonProperty("systemremark")]
        public string SystemRemark { get; set; }

        /// <summary>
        /// 是否潜客（0：否，1：是）
        /// </summary>
        [JsonProperty("lurkstatus")]
        public int LurkStatus { get; set; }

        /// <summary>
        /// 是否转出审核（0：否，1：是）
        /// </summary>
        [JsonProperty("checkstatus")]
        public int CheckStatus { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        [JsonIgnore]
        public int DelState { get; set; }

        #endregion

        /// <summary>
        /// 所属公司
        /// </summary>
        [JsonIgnore]
        public int CorporationID { get; set; }

        /// <summary>
        /// 提交至模块的时间
        /// </summary>
        [JsonProperty("posttime")]
        public string PostTime { get; set; }
    }
}
