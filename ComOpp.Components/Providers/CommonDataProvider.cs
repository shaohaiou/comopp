using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Data;
using ComOpp.Tools;

namespace ComOpp.Components
{
    public abstract class CommonDataProvider
    {
        private static CommonDataProvider _defaultprovider = null;
        private static object _lock = new object();

        #region 初始化
        /// <summary>
        /// 返回默认的数据提供者类
        /// </summary>
        /// <returns></returns>
        public static CommonDataProvider Instance()
        {
            return Instance("MSSQLCommonDataProvider");
        }

        /// <summary>
        /// 从配置文件加载数据库访问提供者类
        /// </summary>
        /// <param name="providerName">提供者名</param>
        /// <returns>漫画提供者</returns>
        public static CommonDataProvider Instance(string providerName)
        {
            string cachekey = GlobalKey.PROVIDER + "_" + providerName;
            CommonDataProvider objType = MangaCache.GetLocal(cachekey) as CommonDataProvider;//从缓存读取
            if (objType == null)
            {
                CommConfig config = CommConfig.GetConfig();
                Provider dataProvider = (Provider)config.Providers[providerName];
                objType = DataProvider.Instance(dataProvider) as CommonDataProvider;
                string path = null;
                HttpContext context = HttpContext.Current;
                if (context != null)
                    path = context.Server.MapPath("~/config/common.config");
                else
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\common.config");
                MangaCache.MaxLocalWithFile(cachekey, objType, path);
            }
            return objType;
        }

        /// <summary>
        ///从配置文件加载默认数据库访问提供者类
        /// </summary>
        private static void LoadDefaultProviders()
        {
            if (_defaultprovider == null)
            {
                lock (_lock)
                {
                    if (_defaultprovider == null)
                    {
                        CommConfig config = CommConfig.GetConfig();
                        Provider dataProvider = (Provider)config.Providers[GlobalKey.DEFAULT_PROVDIER_COMMON];
                        _defaultprovider = DataProvider.Instance(dataProvider) as CommonDataProvider;

                    }
                }
            }
        }

        #endregion

        #region 用户管理

        #region 管理员管理
        
        /// <summary>
        /// 通过管理员名获取管理员
        /// </summary>
        /// <param name="name">管理员名</param>
        /// <returns></returns>
        public abstract AdminInfo GetAdminByName(string id);

        /// <summary>
        /// 管理员是否已经存在
        /// </summary>
        /// <param name="name">管理员ID</param>
        /// <returns></returns>
        public abstract bool ExistsAdmin(int id);

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>添加成功返回ID</returns>
        public abstract int AddAdmin(AdminInfo model);

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>修改是否成功</returns>
        public abstract bool UpdateAdmin(AdminInfo model);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        public abstract bool DeleteAdmin(int AID);

        /// <summary>
        /// 通过ID获取管理员
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>管理员实体信息</returns>
        public abstract AdminInfo GetAdmin(int id);

        /// <summary>
        /// 验证用户登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户ID</returns>
        public abstract int ValiAdmin(string userName, string password);

        /// <summary>
        /// 返回所有用户
        /// </summary>
        /// <returns></returns>
        public abstract List<AdminInfo> GetAllAdmins();

        /// <summary>
        /// 获取普通用户
        /// </summary>
        /// <returns></returns>
        public abstract List<AdminInfo> GetUsers();

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID">管理员ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public abstract bool ChangeAdminPw(int userID, string oldPassword, string newPassword);

        /// <summary>
        /// 获取用于加密的值
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public abstract string GetAdminKey(int userID);

        /// <summary>
        /// 填充后台用户实体类
        /// </summary>
        /// <param name="reader">记录集</param>
        /// <returns>实体类</returns>
        protected AdminInfo PopulateAdmin(IDataReader reader)
        {
            AdminInfo admin = new AdminInfo();
            admin.ID = (int)reader["ID"];
            admin.Administrator = DataConvert.SafeBool(reader["Administrator"]);
            admin.LastLoginIP = reader["LastLoginIP"] as string;
            admin.LastLoginTime = reader["LastLoginTime"] as DateTime?;
            admin.Password = reader["Password"] as string;
            admin.UserName = reader["UserName"] as string;
            admin.UserRole = (UserRoleType)(Int16)reader["UserRole"];
            admin.PowerGroupID = DataConvert.SafeInt(reader["PowerGroupID"]);
            admin.PowerGroupName = reader["PowerGroupName"] as string;
            admin.GroupPower = reader["GroupPower"] as string;

            SerializerData data = new SerializerData();
            data.Keys = reader["PropertyNames"] as string;
            data.Values = reader["PropertyValues"] as string;
            admin.SetSerializerData(data);

            return admin;
        }

        #endregion

        #region 登录记录管理

        public abstract void AddLoginRecord(LoginRecordInfo lr);

        public abstract List<LoginRecordInfo> GetLoginRecord(int uid);

        protected LoginRecordInfo PopulateLoginRecord(IDataReader reader)
        {
            LoginRecordInfo entity = new LoginRecordInfo() 
            {
                ID = (int)reader["ID"],
                AdminID = (int)reader["AdminID"],
                LastLoginIP = reader["LastLoginIP"] as string,
                LoginTime = DataConvert.SafeDate(reader["LoginTime"]),
                UserName = reader["UserName"] as string
            };

            return entity;
        }

        #endregion

        #region 账户组

        public abstract List<PowerGroupInfo> GetPowerGroupList();

        public abstract void AddPowerGroup(PowerGroupInfo entity);

        public abstract void UpdatePowerGroup(PowerGroupInfo entity);

        public abstract void DeletePowerGroup(string ids);

        public static PowerGroupInfo PopulatePowerGroupInfo(IDataReader reader)
        {
            PowerGroupInfo entity = new PowerGroupInfo()
            {
                ID = (int)reader["ID"],
                GroupName = reader["GroupName"] as string,
                GroupPower = reader["GroupPower"] as string,
                MemberCount = DataConvert.SafeInt(reader["MemberCount"]),
                CorporationID = DataConvert.SafeInt(reader["CorporationID"]),
                CorporationName = reader["CorporationName"] as string,
                CanviewGroupIds = reader["CanviewGroupIds"] as string,
                LastUpdateTime = DataConvert.SafeDate(reader["LastUpdateTime"]),
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 用户管理
        
        #endregion

        #endregion

        #region 日志

        public abstract void WriteEventLogEntry(EventLogEntry log);

        public abstract void ClearEventLog(DateTime dt);

        public abstract List<EventLogEntry> GetEventLogs(int pageindex, int pagesize, EventLogQuery query, out int total);

        public abstract EventLogEntry GetEventLogModel(int id);

        /// <summary>
        /// 填充日志信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected EventLogEntry PopulateEventLogEntry(IDataReader reader)
        {
            EventLogEntry eventlog = new EventLogEntry();
            eventlog.EntryID = DataConvert.SafeInt(reader["ID"]);
            eventlog.EventID = DataConvert.SafeInt(reader["EventID"]);
            eventlog.EventType = (EventType)(byte)(reader["EventType"]);
            eventlog.Message = reader["Message"] as string;
            eventlog.Category = reader["Category"] as string;
            eventlog.MachineName = reader["MachineName"] as string;
            eventlog.ApplicationName = reader["ApplicationName"] as string;
            eventlog.PCount = DataConvert.SafeInt(reader["PCount"]);
            eventlog.AddTime = DataConvert.SafeDate(reader["AddTime"]);
            eventlog.LastUpdateTime = reader["LastUpdateTime"] as DateTime?;
            eventlog.ApplicationType = (ApplicationType)(byte)(reader["AppType"]);
            return eventlog;
        }


        #endregion

        #region 公用方法

        public abstract void MoveTop(string tablename, int id, string query);

        public abstract void MoveDown(string tablename, int id, string query);

        public abstract void MoveUp(string tablename, int id, string query);

        #endregion

        #region 系统设置

        #region 公司管理

        public abstract List<CorporationInfo> GetCorporationList();

        public abstract void AddCorporation(CorporationInfo entity);

        public abstract void UpdateCorporation(CorporationInfo entity);

        public abstract void DeleteCorporation(string ids);

        public static CorporationInfo PopulateCorporationInfo(IDataReader reader)
        {
            CorporationInfo entity = new CorporationInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                Sort = DataConvert.SafeInt(reader["Sort"])
            };
            SerializerData data = new SerializerData();
            data.Keys = reader["PropertyNames"] as string;
            data.Values = reader["PropertyValues"] as string;
            entity.SetSerializerData(data);

            return entity;
        }

        #endregion

        #region 模块管理

        public abstract List<ModuleInfo> GetModuleList();

        public abstract void AddModule(ModuleInfo entity);

        public abstract void UpdateModule(ModuleInfo entity);

        public abstract void DeleteModule(string ids);

        public static ModuleInfo PopulateModuleInfo(IDataReader reader)
        {
            ModuleInfo entity = new ModuleInfo()
            {
                ID = (int)reader["ID"],
                ModuleName = reader["ModuleName"] as string,
                ParentName = reader["ParentName"] as string,
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 品牌管理

        public abstract List<CarBrandInfo> GetCarBrandList();

        public abstract int AddCarBrand(CarBrandInfo entity);

        public abstract void UpdateCarBrand(CarBrandInfo entity);

        public abstract void DeleteCarBrand(string ids);

        public static CarBrandInfo PopulateCarBrandInfo(IDataReader reader)
        {
            CarBrandInfo entity = new CarBrandInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                NameIndex = reader["NameIndex"] as string
            };

            return entity;
        }

        #endregion

        #region 车系管理

        public abstract List<CarSeriesInfo> GetCarSeriesList();

        public abstract int AddCarSeries(CarSeriesInfo entity);

        public abstract void UpdateCarSeries(CarSeriesInfo entity);

        public abstract void DeleteCarSeries(string ids);

        public static CarSeriesInfo PopulateCarSeriesInfo(IDataReader reader)
        {
            CarSeriesInfo entity = new CarSeriesInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                BrandID = (int)reader["BrandID"],
                BrandName = reader["BrandName"] as string,
                BrandNameIndex = reader["BrandNameIndex"] as string
            };

            return entity;
        }

        #endregion

        #region 车型管理

        public abstract List<CarModelInfo> GetCarModelList();

        public abstract int AddCarModel(CarModelInfo entity);

        public abstract void UpdateCarModel(CarModelInfo entity);

        public abstract void DeleteCarModel(string ids);

        public static CarModelInfo PopulateCarModelInfo(IDataReader reader)
        {
            CarModelInfo entity = new CarModelInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                BrandID = (int)reader["BrandID"],
                BrandName = reader["BrandName"] as string,
                BrandNameIndex = reader["BrandNameIndex"] as string,
                SeriesName = reader["SeriesName"] as string,
                SeriesID = (int)reader["SeriesID"]
            };

            return entity;
        }

        #endregion

        #region 省市区

        public abstract List<ProvinceInfo> GetProvinceList();

        public static ProvinceInfo PopulateProvinceInfo(IDataReader reader)
        {
            ProvinceInfo entity = new ProvinceInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                OrderID = (int)reader["OrderID"]
            };

            return entity;
        }

        public abstract List<CityInfo> GetCityList();

        public static CityInfo PopulateCityInfo(IDataReader reader)
        {
            CityInfo entity = new CityInfo()
            {
                ID = (int)reader["ID"],
                ProvinceID = (int)reader["ProvinceID"],
                Name = reader["Name"] as string,
                AreaCode = reader["AreaCode"] as string,
            };

            return entity;
        }

        public abstract List<DistrictInfo> GetDistrictList();

        public static DistrictInfo PopulateDistrictInfo(IDataReader reader)
        {
            DistrictInfo entity = new DistrictInfo()
            {
                ID = (int)reader["ID"],
                CityID = (int)reader["CityID"],
                Name = reader["Name"] as string,
                PostCode = reader["PostCode"] as string,
            };

            return entity;
        }

        #endregion

        #endregion

        #region 基础设置

        #region 客户等级管理

        public abstract List<CustomerLevelInfo> GetCustomerLevelList();

        public abstract void AddCustomerLevel(CustomerLevelInfo entity);

        public abstract void UpdateCustomerLevel(CustomerLevelInfo entity);

        public abstract void DeleteCustomerLevel(string ids);

        public static CustomerLevelInfo PopulateCustomerLevelInfo(IDataReader reader)
        {
            CustomerLevelInfo entity = new CustomerLevelInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                Drtday = (int)reader["Drtday"],
                Alarmday = (int)reader["Alarmday"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 信息类型管理

        public abstract List<InfoTypeInfo> GetInfoTypeList();

        public abstract void AddInfoType(InfoTypeInfo entity);

        public abstract void UpdateInfoType(InfoTypeInfo entity);

        public abstract void DeleteInfoType(string ids);

        public static InfoTypeInfo PopulateInfoTypeInfo(IDataReader reader)
        {
            InfoTypeInfo entity = new InfoTypeInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                ParentID = (int)reader["ParentID"],
                ParentName = reader["ParentName"] as string,
                Locked = (int)reader["Locked"],
                Lockday = (int)reader["Lockday"],
                Locklevel = reader["Locklevel"] as string,
                LocklevelName = reader["LocklevelName"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 信息来源管理

        public abstract List<InfoSourceInfo> GetInfoSourceList();

        public abstract void AddInfoSource(InfoSourceInfo entity);

        public abstract void UpdateInfoSource(InfoSourceInfo entity);

        public abstract void DeleteInfoSource(string ids);

        public static InfoSourceInfo PopulateInfoSourceInfo(IDataReader reader)
        {
            InfoSourceInfo entity = new InfoSourceInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 追踪方式管理

        public abstract List<ConnectWayInfo> GetConnectWayList();

        public abstract void AddConnectWay(ConnectWayInfo entity);

        public abstract void UpdateConnectWay(ConnectWayInfo entity);

        public abstract void DeleteConnectWay(string ids);

        public static ConnectWayInfo PopulateConnectWayInfo(IDataReader reader)
        {
            ConnectWayInfo entity = new ConnectWayInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 放弃原因管理

        public abstract List<GiveupCauseInfo> GetGiveupCauseList();

        public abstract void AddGiveupCause(GiveupCauseInfo entity);

        public abstract void UpdateGiveupCause(GiveupCauseInfo entity);

        public abstract void DeleteGiveupCause(string ids);

        public static GiveupCauseInfo PopulateGiveupCauseInfo(IDataReader reader)
        {
            GiveupCauseInfo entity = new GiveupCauseInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 支付方式管理

        public abstract List<PaymentWayInfo> GetPaymentWayList();

        public abstract void AddPaymentWay(PaymentWayInfo entity);

        public abstract void UpdatePaymentWay(PaymentWayInfo entity);

        public abstract void DeletePaymentWay(string ids);

        public static PaymentWayInfo PopulatePaymentWayInfo(IDataReader reader)
        {
            PaymentWayInfo entity = new PaymentWayInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 拟购时间管理

        public abstract List<IbuytimeInfo> GetIbuytimeList();

        public abstract void AddIbuytime(IbuytimeInfo entity);

        public abstract void UpdateIbuytime(IbuytimeInfo entity);

        public abstract void DeleteIbuytime(string ids);

        public static IbuytimeInfo PopulateIbuytimeInfo(IDataReader reader)
        {
            IbuytimeInfo entity = new IbuytimeInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion

        #region 线索标签管理

        public abstract List<TracktagInfo> GetTracktagList();

        public abstract void AddTracktag(TracktagInfo entity);

        public abstract void UpdateTracktag(TracktagInfo entity);

        public abstract void DeleteTracktag(string ids);

        public static TracktagInfo PopulateTracktagInfo(IDataReader reader)
        {
            TracktagInfo entity = new TracktagInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                DataLevel = (int)reader["DataLevel"],
                CorporationID = (int)reader["CorporationID"],
                Sort = DataConvert.SafeInt(reader["Sort"])
            };

            return entity;
        }

        #endregion
        
        #endregion

        #region 话术管理

        public abstract List<TalkInfo> GetTalkList();

        public abstract void AddTalk(TalkInfo entity);

        public abstract void UpdateTalk(TalkInfo entity);

        public abstract void DeleteTalk(string ids);

        public static TalkInfo PopulateTalkInfo(IDataReader reader)
        {
            TalkInfo entity = new TalkInfo()
            {
                ID = (int)reader["ID"],
                Title = reader["Title"] as string,
                Content = reader["Content"] as string,
                Tag = reader["Tag"] as string,
                Realname = reader["Realname"] as string,
                PublicUserID = (int)reader["PublicUserID"],
                AddTime = DataConvert.SafeDate(reader["AddTime"]),
                CorporationID = (int)reader["CorporationID"]
            };

            return entity;
        }

        #endregion

        #region 公告通知管理

        public abstract List<NoticeInfo> GetNoticeList();

        public abstract void AddNotice(NoticeInfo entity);

        public abstract void UpdateNotice(NoticeInfo entity);

        public abstract void DeleteNotice(string ids);

        public static NoticeInfo PopulateNoticeInfo(IDataReader reader)
        {
            NoticeInfo entity = new NoticeInfo()
            {
                ID = (int)reader["ID"],
                Title = reader["Title"] as string,
                Content = reader["Content"] as string,
                DataLevel = (int)reader["DataLevel"],
                Realname = reader["Realname"] as string,
                PublicUserID = (int)reader["PublicUserID"],
                AddTime = DataConvert.SafeDate(reader["AddTime"]),
                CorporationID = (int)reader["CorporationID"]
            };

            return entity;
        }

        #endregion

        #region 客户线索

        public abstract void UpdateCustomerPhoneVest(CustomerInfo entity);

        public abstract int AddCustomer(CustomerInfo entity);

        public abstract int UpdateCustomer(CustomerInfo entity);

        public abstract void DeleteCustomer(string ids, int corpid,int userid,string username);

        public abstract int UpdateCustomerLastConnect(CustomerInfo entity);

        public abstract CustomerInfo GetCustomerByPhone(string phone,int corpid);

        public abstract CustomerInfo GetCustomerByID(int id);

        public abstract List<CustomerInfo> GetCustomerListForPhoneVest();

        public abstract List<CustomerInfo> GetCustomerListByCorporation(int cid);

        public abstract void SetCustomerLevel(int id, int level);

        public static CustomerInfo PopulateCustomerInfo(IDataReader reader)
        {
            CustomerInfo entity = new CustomerInfo()
            {
                ID = (int)reader["ID"],
                Name = reader["Name"] as string,
                Phone = reader["Phone"] as string,
                PhoneVest = reader["PhoneVest"] as string,
                BackupPhone = reader["BackupPhone"] as string,
                ProvinceID = (int)reader["ProvinceID"],
                CityID = (int)reader["CityID"],
                DistrictID = (int)reader["DistrictID"],
                Address = reader["Address"] as string,
                WeixinAccount = reader["WeixinAccount"] as string,
                InfoTypeID = (int)reader["InfoTypeID"],
                InfoSourceID = (int)reader["InfoSourceID"],
                PaymentWayID = (int)reader["PaymentWayID"],
                IbuyCarBrandID = (int)reader["IbuyCarBrandID"],
                IbuyCarSeriesID = (int)reader["IbuyCarSeriesID"],
                IbuyCarModelID = (int)reader["IbuyCarModelID"],
                IbuyTimeID = (int)reader["IbuyTimeID"],
                QuotedpriceInfo = reader["QuotedpriceInfo"] as string,
                PromotionInfo = reader["PromotionInfo"] as string,
                RemarkInfo = reader["RemarkInfo"] as string,
                CreateUserID = (int)reader["CreateUserID"],
                CreateUser = reader["CreateUser"] as string,
                CreateTime = reader["CreateTime"] as string,
                OwnerID = (int)reader["OwnerID"],
                Owner = reader["Owner"] as string,
                OwnerPowerGroupID = (int)reader["OwnerPowerGroupID"],
                CustomerSex = (int)reader["CustomerSex"],
                TracktagID = reader["TracktagID"] as string,
                Tracktag = reader["Tracktag"] as string,
                CustomerStatusSource = (int)reader["CustomerStatusSource"],
                CustomerStatus = (int)reader["CustomerStatus"],
                LastCustomerLevelID = (int)reader["LastCustomerLevelID"],
                LastConnectwayID = (int)reader["LastConnectwayID"],
                LastConnectTime = reader["LastConnectTime"] as string,
                LastConnectUserID = (int)reader["LastConnectUserID"],
                LastConnectUser = reader["LastConnectUser"] as string,
                LastConnectDetail = reader["LastConnectDetail"] as string,
                ReservationTime = reader["ReservationTime"] as string,
                VisitTime = reader["VisitTime"] as string,
                LeaveTime = reader["LeaveTime"] as string,
                VisitDuration = (int)reader["VisitDuration"],
                VisitNumber = (int)reader["VisitNumber"],
                IsVisit = (int)reader["IsVisit"],
                ConnectTimes = (int)reader["ConnectTimes"],
                LastUpdateUserID = (int)reader["LastUpdateUserID"],
                LastUpdateUser = reader["LastUpdateUser"] as string,
                LastUpdateTime = reader["LastUpdateTime"] as string,
                SbuyCarBrandID = (int)reader["SbuyCarBrandID"],
                SbuyCarSeriesID = (int)reader["SbuyCarSeriesID"],
                SbuyCarModelID = (int)reader["SbuyCarModelID"],
                OrderNumber = reader["OrderNumber"] as string,
                KnockdownPrice = reader["KnockdownPrice"] as string,
                PlaceOrderTime = reader["PlaceOrderTime"] as string,
                PicupcarTime = reader["PicupcarTime"] as string,
                GiveupCauseID = (int)reader["GiveupCauseID"],
                FailureCauseAnalyze = reader["FailureCauseAnalyze"] as string,
                MarketDirectorID = (int)reader["MarketDirectorID"],
                MarketDirector = reader["MarketDirector"] as string,
                DCCDirectorID = (int)reader["DCCDirectorID"],
                DCCDirector = reader["DCCDirector"] as string,
                ExhibitionDirectorID = (int)reader["ExhibitionDirectorID"],
                ExhibitionDirector = reader["ExhibitionDirector"] as string,
                DirectorID = (int)reader["DirectorID"],
                Director = reader["Director"] as string,
                SystemRemark = reader["SystemRemark"] as string,
                LurkStatus = (int)reader["LurkStatus"],
                CheckStatus = (int)reader["CheckStatus"],
                DelState = (bool)reader["DelState"] ? 1 : 0,
                PostTime = reader["PostTime"] as string,
                CorporationID = (int)reader["CorporationID"]
            };

            return entity;
        }

        #endregion

        #region 线索流转记录

        public abstract int AddCustomerMoveRecord(CustomerMoveRecordInfo entity);

        public abstract List<CustomerMoveRecordInfo> GetCustomerMoveRecordListByCustomerID(int cid);

        public abstract CustomerMoveRecordInfo GetCustomerMoveRecordByID(int id);

        public static CustomerMoveRecordInfo PopulateCustomerMoveRecordInfo(IDataReader reader)
        {
            CustomerMoveRecordInfo entity = new CustomerMoveRecordInfo()
            {
                ID = (int)reader["ID"],
                CustomerID = (int)reader["CustomerID"],
                CustomerStatus = (int)reader["CustomerStatus"],
                CustomerStatusSource = (int)reader["CustomerStatusSource"],
                OwnerID = (int)reader["OwnerID"],
                Owner = reader["Owner"] as string,
                LastUpdateUserID = (int)reader["LastUpdateUserID"],
                LastUpdateUser = reader["LastUpdateUser"] as string,
                CreateTime = reader["CreateTime"] as string,
                SystemMsg = reader["SystemMsg"] as string
            };

            return entity;
        }

        #endregion

        #region 客户跟踪记录

        public abstract int AddCustomerConnectRecord(CustomerConnectRecordInfo entity);

        public abstract List<CustomerConnectRecordInfo> GetCustomerConnectRecordListByCustomerID(int cid);

        public abstract CustomerConnectRecordInfo GetCustomerConnectRecordByID(int id);

        public abstract List<CustomerConnectRecordInfo> GetCustomerConnectRecordList(CustomerConnectRecordQuery query, int pageindex, int pagesize, ref int recordcount);

        public static CustomerConnectRecordInfo PopulateCustomerConnectRecordInfo(IDataReader reader)
        {
            CustomerConnectRecordInfo entity = new CustomerConnectRecordInfo()
            {
                ID = (int)reader["ID"],
                CustomerID = (int)reader["CustomerID"],
                CustomerName = reader["CustomerName"] as string,
                CustomerPhone = reader["CustomerPhone"] as string,
                ConnectUserID = (int)reader["ConnectUserID"],
                ConnectUser = reader["ConnectUser"] as string,
                FollowTime = reader["FollowTime"] as string,
                ConnectwayID = (int)reader["ConnectwayID"],
                CustomerLevelID = (int)reader["CustomerLevelID"],
                ConnectDetail = reader["ConnectDetail"] as string,
                CreateTime = reader["CreateTime"] as string
            };

            return entity;
        }

        #endregion
    }
}
