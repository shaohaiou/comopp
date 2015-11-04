using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class GlobalKey
    {
        public static readonly string COMMCONFIG = "cache-config-commconfig";                   //主配置文件缓存键值
        public static readonly string PROVIDER = "cache-provider";                              //数据访问层提供类缓存键值
        public static readonly string DEFAULT_PROVDIER_COMMON = "MSSQLCommonDataProvider";//默认通用数据访问层提供类
        public static readonly string DEFAULT_PROVDIER_CAR = "MSSQLCarDataProvider";//默认汽车数据访问层提供类

        public static readonly string EVENTLOG_KEY = "cache-sign-event";
        public static readonly string SESSION_ADMIN = "Session_Admin";//后台用户Session
        public static readonly string MACHINEKEY_COOKIENAME = "HX_MACHINEKEY";  //客户端唯一标示的cookie键值
        public static readonly string CONTEXT_KEY = "Context";               //当前上下文键值
        public static readonly string REWRITER_KEY = "cache-config-rewriter";

        public static readonly string GLOBALSETTING_KEY = "cache-globalsetting"; //系统全局设置缓存键值

        public static readonly string PROVINCE_LIST = "cache-province-list"; //省份缓存键值
        public static readonly string CITY_LIST = "cache-city-list"; //市缓存键值
        public static readonly string DISTRICT_LIST = "cache-district-list"; //地区缓存键值
        public static readonly string CORPORATION_LIST = "cache-corporation-list"; //公司列表缓存键值
        public static readonly string POWERGROUP_LIST = "cache-powergroup-list"; //账户组列表缓存键值
        public static readonly string MODULE_LIST = "cache-module-list"; //账户组列表缓存键值
        public static readonly string CARBRAND_LIST = "cache-carbrand-list"; //车辆品牌列表缓存键值
        public static readonly string CARSERIES_LIST = "cache-carseries-list"; //车系列表缓存键值
        public static readonly string CARMODEL_LIST = "cache-carmodel-list"; //车型列表缓存键值
        public static readonly string CUSTOMERLEVEL_LIST = "cache-customerlevel-list"; //客户等级列表缓存键值
        public static readonly string INFOTYPE_LIST = "cache-infotype-list"; //信息类型列表缓存键值
        public static readonly string INFOSOURCE_LIST = "cache-infosource-list"; //信息来源列表缓存键值
        public static readonly string CONNECTWAY_LIST = "cache-connectway-list"; //追踪方式列表缓存键值
        public static readonly string GIVEUPCAUSE_LIST = "cache-giveupcause-list"; //放弃原因列表缓存键值
        public static readonly string PAYMENTWAY_LIST = "cache-paymentway-list"; //支付方式列表缓存键值
        public static readonly string IBUYTIME_LIST = "cache-ibuytime-list"; //拟购时间列表缓存键值
        public static readonly string TRACKTAG_LIST = "cache-tracktag-list"; //线索标签列表缓存键值
        public static readonly string TALK_LIST = "cache-talk-list"; //话术列表缓存键值
        public static readonly string NOTICE_LIST = "cache-notice-list"; //公告通知列表缓存键值
        public static readonly string CUSTOMER_LIST = "cache-customer-list"; //客户线索列表缓存键值
        public static readonly string CUSTOMERMOVERECORD_LIST = "cache-customermoverecord-list"; //客户线索流转记录列表缓存键值
        public static readonly string CUSTOMERCONNECTRECORD_LIST = "cache-customerconnectrecord-list"; //客户跟踪记录列表缓存键值
    }
}
