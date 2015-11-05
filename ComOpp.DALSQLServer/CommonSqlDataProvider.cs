using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ComOpp.Tools;
using ComOpp.Components;
using System.Data.SqlClient;
using System.Data;

namespace ComOpp.DALSQLServer
{
    public class CommonSqlDataProvider : CommonDataProvider
    {
        private string _con;
        private string _dbowner;
        private static object sync_helper = new object();

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="constr">连接字符串</param>
        /// <param name="owner">数据库所有者</param>
        public CommonSqlDataProvider(string constr, string owner)
        {
            CommConfig config = CommConfig.GetConfig();
            _con = EncryptString.DESDecode(constr, config.AppSetting["key"]);
            _dbowner = owner;
        }
        #endregion

        #region 用户管理

        #region 管理员管理

        /// <summary>
        ///  获取用于加密的值
        /// </summary>
        /// <param name="userID">管理员ID</param>
        /// <returns>用于加密的值</returns>
        public override string GetAdminKey(int userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CheckKey from ComOpp_AdminUser ");
            strSql.Append(" where ID=@ID ");
            object o = SqlHelper.ExecuteScalar(_con, CommandType.Text, strSql.ToString(), new SqlParameter("@ID", userID));
            return o as string;
        }

        /// <summary>
        /// 管理员是否已经存在
        /// </summary>
        /// <param name="name">管理员ID</param>
        /// <returns>管理员是否存在</returns>
        public override bool ExistsAdmin(int id)
        {
            string sql = "select count(1) from ComOpp_AdminUser where ID=@ID";
            int i = Convert.ToInt32(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, new SqlParameter("@ID", id)));
            if (i > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 通过用户名获得后台管理员信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>管理员实体信息</returns>
        public override AdminInfo GetAdminByName(string UserName)
        {
            string sql = "select * from ViewAdmin where UserName=@UserName";
            AdminInfo admin = null;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@UserName", UserName)))
            {
                if (reader.Read())
                {
                    admin = PopulateAdmin(reader);
                }
            }
            return admin;
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>添加成功返回ID</returns>
        public override int AddAdmin(AdminInfo model)
        {
            SerializerData data = model.GetSerializerData();
            string sql = @"
            INSERT INTO ComOpp_AdminUser(UserName,Password,Administrator,LastLoginIP,LastLoginTime,[PropertyNames],[PropertyValues],[UserRole],[PowerGroupID])
            VALUES (@UserName,@Password,@Administrator,@LastLoginIP,@LastLoginTime,@PropertyNames,@PropertyValues,@UserRole,@PowerGroupID)
            ;SELECT @@IDENTITY";
            SqlParameter[] p = 
            {
                new SqlParameter("@UserName",model.UserName),
                new SqlParameter("@Password",model.Password),
                new SqlParameter("@Administrator",model.Administrator),
                new SqlParameter("@LastLoginIP",model.LastLoginIP),
                new SqlParameter("@LastLoginTime",model.LastLoginTime),
                new SqlParameter("@UserRole",model.UserRole),
                new SqlParameter("@PowerGroupID",model.PowerGroupID),
                new SqlParameter("@PropertyNames",data.Keys),
                new SqlParameter("@PropertyValues",data.Values)
            };
            model.ID = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
            return model.ID;
        }

        /// <summary>
        /// 更新管理员
        /// </summary>
        /// <param name="model">后台用户实体类</param>
        /// <returns>修改是否成功</returns>
        public override bool UpdateAdmin(AdminInfo model)
        {
            SerializerData data = model.GetSerializerData();
            string sql = @"UPDATE ComOpp_AdminUser SET
            UserName = @UserName
            ,Password = @Password
            ,Administrator = @Administrator
            ,LastLoginIP = @LastLoginIP
            ,LastLoginTime = @LastLoginTime
            ,UserRole = @UserRole
            ,PowerGroupID = @PowerGroupID
            ,[PropertyNames] = @PropertyNames
            ,[PropertyValues] = @PropertyValues
            WHERE ID = @ID
            ";
            SqlParameter[] p = 
            {
                new SqlParameter("@UserName",model.UserName),
                new SqlParameter("@Password",model.Password),
                new SqlParameter("@Administrator",model.Administrator),
                new SqlParameter("@LastLoginIP",model.LastLoginIP),
                new SqlParameter("@LastLoginTime",model.LastLoginTime),
                new SqlParameter("@UserRole",(int)model.UserRole),
                new SqlParameter("@PowerGroupID",model.PowerGroupID),
                new SqlParameter("@PropertyNames",data.Keys),
                new SqlParameter("@PropertyValues",data.Values),
                new SqlParameter("@ID",model.ID)
            };
            int result = SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <returns>删除是否成功</returns>
        public override bool DeleteAdmin(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ComOpp_AdminUser ");
            strSql.Append(" where ID=@ID ");
            int result = SqlHelper.ExecuteNonQuery(_con, CommandType.Text, strSql.ToString(), new SqlParameter("@ID", id));
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 通过ID获取管理员
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>管理员实体信息</returns>
        public override AdminInfo GetAdmin(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from ViewAdmin");
            strSql.Append(" where ID=@ID ");
            AdminInfo admin = null;
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, strSql.ToString(), new SqlParameter("@ID", id)))
            {
                if (reader.Read())
                {
                    admin = PopulateAdmin(reader);
                }
            }
            return admin;
        }

        /// <summary>
        /// 验证用户登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>用户ID</returns>
        public override int ValiAdmin(string userName, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from ComOpp_AdminUser");
            strSql.Append(" where UserName=@UserName and Password=@PassWord");

            object obj = SqlHelper.ExecuteScalar(_con, CommandType.Text, strSql.ToString(), new SqlParameter("@UserName", userName), new SqlParameter("@PassWord", password));
            if (obj == null)
            {
                return -2;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 返回所有用户
        /// </summary>
        /// <returns>返回所有用户</returns>
        public override List<AdminInfo> GetAllAdmins()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ViewAdmin");


            List<AdminInfo> admins = new List<AdminInfo>();
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, strSql.ToString()))
            {
                while (reader.Read())
                {
                    admins.Add(PopulateAdmin(reader));
                }
            }
            return admins;
        }

        public override List<AdminInfo> GetUsers()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ViewAdmin WHERE [Administrator] = 0");


            List<AdminInfo> admins = new List<AdminInfo>();
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, strSql.ToString()))
            {
                while (reader.Read())
                {
                    admins.Add(PopulateAdmin(reader));
                }
            }
            return admins;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID">管理员ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>修改密码是否成功</returns>
        public override bool ChangeAdminPw(int userID, string oldPassword, string newPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ComOpp_AdminUser set ");
            strSql.Append("Password=@NewPassword");
            strSql.Append(" where ID=@ID and Password=@Password ");
            int result = SqlHelper.ExecuteNonQuery(_con, CommandType.Text, strSql.ToString(), new SqlParameter("@ID", userID), new SqlParameter("@Password", oldPassword), new SqlParameter("@NewPassword", newPassword));
            if (result < 1)
                return false;
            return true;
        }

        #endregion

        #region 账户组

        public override void AddPowerGroup(PowerGroupInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_PowerGroup(
                [GroupName]
                ,[GroupPower]
                ,[CorporationID]
                ,[CanviewGroupIds]
                ,[Sort]
            )VALUES(
                @GroupName
                ,@GroupPower
                ,@CorporationID
                ,@CanviewGroupIds
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_PowerGroup WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@GroupName",entity.GroupName),
                new SqlParameter("@GroupPower",entity.GroupPower),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@CanviewGroupIds",entity.CanviewGroupIds)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<PowerGroupInfo> GetPowerGroupList()
        {
            List<PowerGroupInfo> list = new List<PowerGroupInfo>();
            string sql = "SELECT * FROM ViewPowerGroup";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulatePowerGroupInfo(reader));
                }
            }

            return list;
        }

        public override void UpdatePowerGroup(PowerGroupInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_PowerGroup set
                GroupName = @GroupName
                ,GroupPower = @GroupPower
                ,CanviewGroupIds = @CanviewGroupIds
                ,LastUpdateTime = @LastUpdateTime
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@GroupName", entity.GroupName),
			    new SqlParameter("@GroupPower", entity.GroupPower),
			    new SqlParameter("@CanviewGroupIds", entity.CanviewGroupIds),
			    new SqlParameter("@LastUpdateTime", entity.LastUpdateTime)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeletePowerGroup(string ids)
        {
            string sql = "UPDATE ComOpp_PowerGroup SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #endregion

        #region 日志

        /// <summary>
        /// 写入日志信息
        /// </summary>
        /// <param name="log"></param>
        public override void WriteEventLogEntry(EventLogEntry log)
        {
            try
            {
                string sql = "ComOpp_AddEvent";
                SqlParameter[] parameters = 
                    {
                        new SqlParameter("@Uniquekey", log.Uniquekey),
                        new SqlParameter("@EventType", log.EventType),
                        new SqlParameter("@EventID",log.EventID),
                        new SqlParameter("@Message",log.Message),
                        new SqlParameter("@Category",log.Category),
                        new SqlParameter("@MachineName",log.MachineName),
                        new SqlParameter("@ApplicationName",log.ApplicationName),
                        new SqlParameter("@ApplicationID",log.ApplicationID),
                        new SqlParameter("@AppType",log.ApplicationType),
                        new SqlParameter("@EntryID",log.EntryID),
                        new SqlParameter("@PCount",log.PCount),
                        new SqlParameter("@LastUpdateTime",log.LastUpdateTime)
                    };
                SqlHelper.ExecuteNonQuery(_con, CommandType.StoredProcedure, sql, parameters);
            }
            catch { }
        }

        /// <summary>
        /// 根据时间清除日志
        /// </summary>
        /// <param name="dt"></param>
        public override void ClearEventLog(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public override List<EventLogEntry> GetEventLogs(int pageindex, int pagesize, EventLogQuery query, out int total)
        {
            List<EventLogEntry> eventlist = new List<EventLogEntry>();
            SqlParameter p;
            if (pageindex != -1)
            {
                using (IDataReader reader = CommonPageSql.GetDataReaderByPager(_con, pageindex, pagesize, query, out p))
                {
                    while (reader.Read())
                    {
                        eventlist.Add(PopulateEventLogEntry(reader));
                    }
                }
                total = int.Parse(p.Value.ToString());
            }

            else
            {
                using (IDataReader reader = CommonSelectSql.SelectGetReader(_con, pagesize, query))
                {
                    while (reader.Read())
                    {
                        eventlist.Add(PopulateEventLogEntry(reader));
                    }
                }
                total = eventlist.Count();
            }
            return eventlist;
        }
        #endregion

        #region 公用方法

        public override void MoveTop(string tablename, int id,string query)
        {
            string sql = @"
            UPDATE " + tablename + @" SET
                [Sort] = 0 
            WHERE [ID] = @ID;
            UPDATE " + tablename + @" SET
                [Sort] = T.[Sort]
            FROM " + tablename + @" AS S,(SELECT [ID],ROW_NUMBER() OVER(ORDER BY [Sort]) AS Sort
                FROM " + tablename + @"
               " + query + @" 
                ) T
            WHERE S.[ID] = T.[ID]
            ";

            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql,new SqlParameter("@ID",id));
        }

        public override void MoveDown(string tablename, int id, string query)
        {
            string sql = @"
            IF EXISTS(SELECT [ID] FROM " + tablename + @" WHERE [Sort] > (SELECT [Sort] FROM " + tablename + @" WHERE [ID] = @ID)" + (string.IsNullOrEmpty(query) ? string.Empty : (" AND " + query.ToUpper().Replace("WHERE",string.Empty))) + @")
            BEGIN
                UPDATE " + tablename + @" SET 
                    [Sort] = (SELECT TOP 1 [Sort] FROM " + tablename + @" WHERE [Sort] > (SELECT [Sort] FROM " + tablename + @" WHERE [ID] = @ID) " + (string.IsNullOrEmpty(query) ? string.Empty : (" AND " + query.ToUpper().Replace("WHERE", string.Empty))) + @")
                WHERE [ID] = @ID
                UPDATE " + tablename + @" SET 
                    [Sort] = [Sort] - 1
                WHERE [ID] <> @ID AND [Sort] = (SELECT [Sort] FROM " + tablename + @" WHERE [ID] = @ID) " + (string.IsNullOrEmpty(query) ? string.Empty : (" AND " + query.ToUpper().Replace("WHERE", string.Empty))) + @"
            END
            ";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new SqlParameter("@ID", id));
        }

        public override void MoveUp(string tablename, int id, string query)
        {
            string sql = @"
            IF EXISTS(SELECT [ID] FROM " + tablename + @" WHERE [Sort] < (SELECT [Sort] FROM " + tablename + @" WHERE [ID] = @ID)" + (string.IsNullOrEmpty(query) ? string.Empty : (" AND " + query.ToUpper().Replace("WHERE", string.Empty))) + @")
            BEGIN
                UPDATE " + tablename + @" SET 
                    [Sort] = (SELECT TOP 1 [Sort] FROM " + tablename + @" WHERE [Sort] < (SELECT [Sort] FROM " + tablename + @" WHERE [ID] = @ID) " + (string.IsNullOrEmpty(query) ? string.Empty : (" AND " + query.ToUpper().Replace("WHERE", string.Empty))) + @" ORDER BY [Sort] DESC)
                WHERE [ID] = @ID
                UPDATE " + tablename + @" SET 
                    [Sort] = [Sort] + 1
                WHERE [ID] <> @ID AND [Sort] = (SELECT [Sort] FROM " + tablename + @" WHERE [ID] = @ID) " + (string.IsNullOrEmpty(query) ? string.Empty : (" AND " + query.ToUpper().Replace("WHERE", string.Empty))) + @"
            END
            ";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new SqlParameter("@ID", id));
        }

        #endregion

        #region 系统设置

        #region 公司管理

        public override void AddCorporation(CorporationInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_Corporation(
                [Name]
                ,[Sort]
                ,[PropertyNames]
                ,[PropertyValues]
            )VALUES(
                @Name
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_Corporation)
                ,@PropertyNames
                ,@PropertyValues
            )";
            SerializerData data = entity.GetSerializerData();
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
				new SqlParameter("@PropertyNames", data.Keys),
				new SqlParameter("@PropertyValues", data.Values)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<CorporationInfo> GetCorporationList()
        {
            List<CorporationInfo> list = new List<CorporationInfo>();
            string sql = "SELECT * FROM ComOpp_Corporation";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCorporationInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateCorporation(CorporationInfo entity)
        {
            SerializerData data = entity.GetSerializerData();
            string sql = @"
            UPDATE ComOpp_Corporation set
                Name = @Name
                ,PropertyNames=@PropertyNames
                ,PropertyValues=@PropertyValues
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name),
			    new SqlParameter("@PropertyNames", data.Keys),
			    new SqlParameter("@PropertyValues", data.Values)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteCorporation(string ids)
        {
            string sql = "DELETE FROM ComOpp_Corporation WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 模块管理

        public override void AddModule(ModuleInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_Module(
                [ModuleName]
                ,[ParentName]
                ,[Sort]
            )VALUES(
                @ModuleName
                ,@ParentName
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_Module)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@ModuleName",entity.ModuleName),
                new SqlParameter("@ParentName",entity.ParentName)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<ModuleInfo> GetModuleList()
        {
            List<ModuleInfo> list = new List<ModuleInfo>();
            string sql = "SELECT * FROM ComOpp_Module";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateModuleInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateModule(ModuleInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_Module set
                ModuleName = @ModuleName
                ,ParentName = @ParentName
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@ModuleName", entity.ModuleName),
			    new SqlParameter("@ParentName", entity.ParentName)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteModule(string ids)
        {
            string sql = "DELETE FROM ComOpp_Module WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 品牌管理

        public override int AddCarBrand(CarBrandInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_CarBrand(
                [Name]
                ,[NameIndex]
            )VALUES(
                @Name
                ,@NameIndex
            );SELECT @@IDENTITY";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@NameIndex",entity.NameIndex)
            };
            return DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
        }

        public override List<CarBrandInfo> GetCarBrandList()
        {
            List<CarBrandInfo> list = new List<CarBrandInfo>();
            string sql = "SELECT * FROM ComOpp_CarBrand";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCarBrandInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateCarBrand(CarBrandInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_CarBrand set
                Name = @Name
                ,NameIndex = @NameIndex
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name),
			    new SqlParameter("@NameIndex", entity.NameIndex)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteCarBrand(string ids)
        {
            string sql = "DELETE FROM ComOpp_CarBrand WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 车系管理

        public override int AddCarSeries(CarSeriesInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_CarSeries(
                [Name]
                ,[BrandID]
            )VALUES(
                @Name
                ,@BrandID
            );SELECT @@IDENTITY";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@BrandID",entity.BrandID)
            };
            return DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
        }

        public override List<CarSeriesInfo> GetCarSeriesList()
        {
            List<CarSeriesInfo> list = new List<CarSeriesInfo>();
            string sql = "SELECT * FROM ViewCarSeries";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCarSeriesInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateCarSeries(CarSeriesInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_CarSeries set
                Name = @Name
                ,BrandID = @BrandID
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name),
			    new SqlParameter("@BrandID", entity.BrandID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteCarSeries(string ids)
        {
            string sql = "DELETE FROM ComOpp_CarSeries WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 车型管理

        public override int AddCarModel(CarModelInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_CarModel(
                [Name]
                ,[SeriesID]
            )VALUES(
                @Name
                ,@SeriesID
            );SELECT @@IDENTITY";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@SeriesID",entity.SeriesID)
            };
            return DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
        }

        public override List<CarModelInfo> GetCarModelList()
        {
            List<CarModelInfo> list = new List<CarModelInfo>();
            string sql = "SELECT * FROM ViewCarModel";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCarModelInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateCarModel(CarModelInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_CarModel set
                Name = @Name
                ,SeriesID = @SeriesID
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name),
			    new SqlParameter("@SeriesID", entity.SeriesID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteCarModel(string ids)
        {
            string sql = "DELETE FROM ComOpp_CarModel WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 省市区

        public override List<ProvinceInfo> GetProvinceList()
        {
            List<ProvinceInfo> list = new List<ProvinceInfo>();
            string sql = "SELECT * FROM dbo.ComOpp_Province";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateProvinceInfo(reader));
                }
            }

            return list;
        }

        public override List<CityInfo> GetCityList()
        {
            List<CityInfo> list = new List<CityInfo>();
            string sql = "SELECT * FROM dbo.ComOpp_City";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCityInfo(reader));
                }
            }

            return list;
        }

        public override List<DistrictInfo> GetDistrictList()
        {
            List<DistrictInfo> list = new List<DistrictInfo>();
            string sql = "SELECT * FROM dbo.ComOpp_District";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateDistrictInfo(reader));
                }
            }

            return list;
        }

        #endregion

        #endregion

        #region 基础设置

        #region 客户等级管理

        public override void AddCustomerLevel(CustomerLevelInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_CustomerLevel(
                [Name]
                ,[CorporationID]
                ,[Drtday]
                ,[Alarmday]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@Drtday
                ,@Alarmday
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_CustomerLevel WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@Drtday",entity.Drtday),
                new SqlParameter("@Alarmday",entity.Alarmday)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<CustomerLevelInfo> GetCustomerLevelList()
        {
            List<CustomerLevelInfo> list = new List<CustomerLevelInfo>();
            string sql = "SELECT * FROM ComOpp_CustomerLevel WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCustomerLevelInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateCustomerLevel(CustomerLevelInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_CustomerLevel set
                Name = @Name
                ,Drtday = @Drtday
                ,Alarmday = @Alarmday
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name),
			    new SqlParameter("@Drtday", entity.Drtday),
			    new SqlParameter("@Alarmday", entity.Alarmday)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteCustomerLevel(string ids)
        {
            string sql = "UPDATE ComOpp_CustomerLevel SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 信息类型管理

        public override void AddInfoType(InfoTypeInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_InfoType(
                [Name]
                ,[CorporationID]
                ,[Locked]
                ,[Lockday]
                ,[Locklevel]
                ,[LocklevelName]
                ,[DataLevel]
                ,[ParentID]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@Locked
                ,@Lockday
                ,@Locklevel
                ,@LocklevelName
                ,@DataLevel
                ,@ParentID
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_InfoType WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@Locked",entity.Locked),
                new SqlParameter("@Lockday",entity.Lockday),
                new SqlParameter("@Locklevel",entity.Locklevel),
                new SqlParameter("@LocklevelName",entity.LocklevelName),
                new SqlParameter("@DataLevel",entity.DataLevel),
                new SqlParameter("@ParentID",entity.ParentID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<InfoTypeInfo> GetInfoTypeList()
        {
            List<InfoTypeInfo> list = new List<InfoTypeInfo>();
            string sql = "SELECT * FROM ViewInfoType WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateInfoTypeInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateInfoType(InfoTypeInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_InfoType set
                Name = @Name
                ,Locked = @Locked
                ,Lockday = @Lockday
                ,Locklevel = @Locklevel
                ,LocklevelName = @LocklevelName
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name),
			    new SqlParameter("@Locked", entity.Locked),
			    new SqlParameter("@Lockday", entity.Lockday),
			    new SqlParameter("@Locklevel", entity.Locklevel),
			    new SqlParameter("@LocklevelName", entity.LocklevelName)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteInfoType(string ids)
        {
            string sql = "UPDATE ComOpp_InfoType SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 信息来源管理

        public override void AddInfoSource(InfoSourceInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_InfoSource(
                [Name]
                ,[CorporationID]
                ,[DataLevel]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@DataLevel
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_InfoSource WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@DataLevel",entity.DataLevel)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<InfoSourceInfo> GetInfoSourceList()
        {
            List<InfoSourceInfo> list = new List<InfoSourceInfo>();
            string sql = "SELECT * FROM ComOpp_InfoSource WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateInfoSourceInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateInfoSource(InfoSourceInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_InfoSource set
                Name = @Name
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteInfoSource(string ids)
        {
            string sql = "UPDATE ComOpp_InfoSource SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 追踪方式管理

        public override void AddConnectWay(ConnectWayInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_ConnectWay(
                [Name]
                ,[CorporationID]
                ,[DataLevel]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@DataLevel
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_ConnectWay WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@DataLevel",entity.DataLevel)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<ConnectWayInfo> GetConnectWayList()
        {
            List<ConnectWayInfo> list = new List<ConnectWayInfo>();
            string sql = "SELECT * FROM ComOpp_ConnectWay WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateConnectWayInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateConnectWay(ConnectWayInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_ConnectWay set
                Name = @Name
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteConnectWay(string ids)
        {
            string sql = "UPDATE ComOpp_ConnectWay SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 放弃原因管理

        public override void AddGiveupCause(GiveupCauseInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_GiveupCause(
                [Name]
                ,[CorporationID]
                ,[DataLevel]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_GiveupCause WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@DataLevel",entity.DataLevel)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<GiveupCauseInfo> GetGiveupCauseList()
        {
            List<GiveupCauseInfo> list = new List<GiveupCauseInfo>();
            string sql = "SELECT * FROM ComOpp_GiveupCause WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateGiveupCauseInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateGiveupCause(GiveupCauseInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_GiveupCause set
                Name = @Name
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteGiveupCause(string ids)
        {
            string sql = "UPDATE ComOpp_GiveupCause SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 支付方式管理

        public override void AddPaymentWay(PaymentWayInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_PaymentWay(
                [Name]
                ,[CorporationID]
                ,[DataLevel]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@DataLevel
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_PaymentWay WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@DataLevel",entity.DataLevel)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<PaymentWayInfo> GetPaymentWayList()
        {
            List<PaymentWayInfo> list = new List<PaymentWayInfo>();
            string sql = "SELECT * FROM ComOpp_PaymentWay WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulatePaymentWayInfo(reader));
                }
            }

            return list;
        }

        public override void UpdatePaymentWay(PaymentWayInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_PaymentWay set
                Name = @Name
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeletePaymentWay(string ids)
        {
            string sql = "UPDATE ComOpp_PaymentWay SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 拟购时间管理

        public override void AddIbuytime(IbuytimeInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_Ibuytime(
                [Name]
                ,[CorporationID]
                ,[DataLevel]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@DataLevel
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_Ibuytime WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@DataLevel",entity.DataLevel)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<IbuytimeInfo> GetIbuytimeList()
        {
            List<IbuytimeInfo> list = new List<IbuytimeInfo>();
            string sql = "SELECT * FROM ComOpp_Ibuytime WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateIbuytimeInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateIbuytime(IbuytimeInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_Ibuytime set
                Name = @Name
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteIbuytime(string ids)
        {
            string sql = "UPDATE ComOpp_Ibuytime SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 线索标签管理

        public override void AddTracktag(TracktagInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_Tracktag(
                [Name]
                ,[CorporationID]
                ,[DataLevel]
                ,[Sort]
            )VALUES(
                @Name
                ,@CorporationID
                ,@DataLevel
                ,(SELECT ISNULL(MAX([Sort]),0) + 1 FROM ComOpp_Tracktag WHERE [CorporationID] = @CorporationID)
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@CorporationID",entity.CorporationID),
                new SqlParameter("@DataLevel",entity.DataLevel)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<TracktagInfo> GetTracktagList()
        {
            List<TracktagInfo> list = new List<TracktagInfo>();
            string sql = "SELECT * FROM ComOpp_Tracktag WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateTracktagInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateTracktag(TracktagInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_Tracktag set
                Name = @Name
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Name", entity.Name)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteTracktag(string ids)
        {
            string sql = "UPDATE ComOpp_Tracktag SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #endregion

        #region 话术管理

        public override void AddTalk(TalkInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_Talk(
                [Title]
                ,[Content]
                ,[Tag]
                ,[Realname]
                ,[PublicUserID]
                ,[CorporationID]
            )VALUES(
                @Title
                ,@Content
                ,@Tag
                ,@Realname
                ,@PublicUserID
                ,@CorporationID
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Title",entity.Title),
                new SqlParameter("@Content",entity.Content),
                new SqlParameter("@Tag",entity.Tag),
                new SqlParameter("@Realname",entity.Realname),
                new SqlParameter("@PublicUserID",entity.PublicUserID),
                new SqlParameter("@CorporationID",entity.CorporationID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<TalkInfo> GetTalkList()
        {
            List<TalkInfo> list = new List<TalkInfo>();
            string sql = "SELECT * FROM ComOpp_Talk WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateTalkInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateTalk(TalkInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_Talk set
                Title = @Title
                ,Content = @Content
                ,Tag = @Tag
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Title", entity.Title),
			    new SqlParameter("@Content", entity.Content),
			    new SqlParameter("@Tag", entity.Tag)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteTalk(string ids)
        {
            string sql = "UPDATE ComOpp_Talk SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 公告通知管理

        public override void AddNotice(NoticeInfo entity)
        {
            string sql = @"INSERT INTO ComOpp_Notice(
                [Title]
                ,[Content]
                ,[DataLevel]
                ,[Realname]
                ,[PublicUserID]
                ,[CorporationID]
            )VALUES(
                @Title
                ,@Content
                ,@DataLevel
                ,@Realname
                ,@PublicUserID
                ,@CorporationID
            )";
            SqlParameter[] p = 
            {
                new SqlParameter("@Title",entity.Title),
                new SqlParameter("@Content",entity.Content),
                new SqlParameter("@DataLevel",entity.DataLevel),
                new SqlParameter("@Realname",entity.Realname),
                new SqlParameter("@PublicUserID",entity.PublicUserID),
                new SqlParameter("@CorporationID",entity.CorporationID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override List<NoticeInfo> GetNoticeList()
        {
            List<NoticeInfo> list = new List<NoticeInfo>();
            string sql = "SELECT * FROM ComOpp_Notice WHERE [State] = 1";

            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateNoticeInfo(reader));
                }
            }

            return list;
        }

        public override void UpdateNotice(NoticeInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_Notice set
                Title = @Title
                ,Content = @Content
            WHERE ID=@ID";
            SqlParameter[] parameters = 
            {
			    new SqlParameter("@ID", entity.ID),
			    new SqlParameter("@Title", entity.Title),
			    new SqlParameter("@Content", entity.Content)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, parameters);
        }

        public override void DeleteNotice(string ids)
        {
            string sql = "UPDATE ComOpp_Notice SET [State] = 0 WHERE ID IN (" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql);
        }

        #endregion

        #region 客户线索管理

        public override int AddCustomer(CustomerInfo entity)
        {
            string sql = @"
                INSERT INTO ComOpp_Customer(
                    [Name],[Phone],[BackupPhone],[ProvinceID],[CityID],[DistrictID],[Address],[WeixinAccount],[InfoTypeID],[InfoSourceID],[PaymentWayID],[IbuyCarBrandID]
                    ,[IbuyCarSeriesID],[IbuyCarModelID],[IbuyTimeID],[QuotedpriceInfo],[PromotionInfo],[RemarkInfo],[OwnerID],[Owner],[OwnerPowerGroupID],[CustomerSex],[TracktagID]
                    ,[Tracktag],[CustomerStatus],[CreateTime],[CreateUserID],[CreateUser],[LastUpdateUserID],[LastUpdateUser],[LastUpdateTime],[CorporationID],[PostTime],[SystemRemark]
                )VALUES(
                    @Name,@Phone,@BackupPhone,@ProvinceID,@CityID,@DistrictID,@Address,@WeixinAccount,@InfoTypeID,@InfoSourceID,@PaymentWayID,@IbuyCarBrandID
                    ,@IbuyCarSeriesID,@IbuyCarModelID,@IbuyTimeID,@QuotedpriceInfo,@PromotionInfo,@RemarkInfo,@OwnerID,@Owner,@OwnerPowerGroupID,@CustomerSex,@TracktagID
                    ,@Tracktag,@CustomerStatus,@CreateTime,@CreateUserID,@CreateUser,@LastUpdateUserID,@LastUpdateUser,@LastUpdateTime,@CorporationID,@PostTime,@SystemRemark
                );
                SELECT @@IDENTITY;
            ";
            SqlParameter[] p = 
            {
                new SqlParameter("Name",entity.Name),
                new SqlParameter("Phone",entity.Phone),
                new SqlParameter("BackupPhone",entity.BackupPhone),
                new SqlParameter("ProvinceID",entity.ProvinceID),
                new SqlParameter("CityID",entity.CityID),
                new SqlParameter("DistrictID",entity.DistrictID),
                new SqlParameter("Address",entity.Address),
                new SqlParameter("WeixinAccount",entity.WeixinAccount),
                new SqlParameter("InfoTypeID",entity.InfoTypeID),
                new SqlParameter("InfoSourceID",entity.InfoSourceID),
                new SqlParameter("PaymentWayID",entity.PaymentWayID),
                new SqlParameter("IbuyCarBrandID",entity.IbuyCarBrandID),
                new SqlParameter("IbuyCarSeriesID",entity.IbuyCarSeriesID),
                new SqlParameter("IbuyCarModelID",entity.IbuyCarModelID),
                new SqlParameter("IbuyTimeID",entity.IbuyTimeID),
                new SqlParameter("QuotedpriceInfo",entity.QuotedpriceInfo),
                new SqlParameter("PromotionInfo",entity.PromotionInfo),
                new SqlParameter("RemarkInfo",entity.RemarkInfo),
                new SqlParameter("OwnerID",entity.OwnerID),
                new SqlParameter("Owner",entity.Owner),
                new SqlParameter("OwnerPowerGroupID",entity.OwnerPowerGroupID),
                new SqlParameter("CustomerSex",entity.CustomerSex),
                new SqlParameter("TracktagID",entity.TracktagID),
                new SqlParameter("Tracktag",entity.Tracktag),
                new SqlParameter("CustomerStatus",entity.CustomerStatus),
                new SqlParameter("CreateTime",entity.CreateTime),
                new SqlParameter("CreateUserID",entity.CreateUserID),
                new SqlParameter("CreateUser",entity.CreateUser),
                new SqlParameter("LastUpdateUserID",entity.LastUpdateUserID),
                new SqlParameter("LastUpdateUser",entity.LastUpdateUser),
                new SqlParameter("LastUpdateTime",entity.LastUpdateTime),
                new SqlParameter("CorporationID",entity.CorporationID),
                new SqlParameter("PostTime",entity.PostTime),
                new SqlParameter("SystemRemark",entity.SystemRemark)
            };
            entity.ID = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
            return entity.ID;
        }

        public override int UpdateCustomer(CustomerInfo entity)
        {
            string sql = @"
                UPDATE ComOpp_Customer SET
                [Name] = @Name
                ,[BackupPhone] = @BackupPhone
                ,[ProvinceID] = @ProvinceID
                ,[CityID] = @CityID
                ,[DistrictID] = @DistrictID
                ,[Address] = @Address
                ,[WeixinAccount] = @WeixinAccount
                ,[InfoTypeID] = @InfoTypeID
                ,[InfoSourceID] = @InfoSourceID
                ,[PaymentWayID] = @PaymentWayID
                ,[IbuyCarBrandID] = @IbuyCarBrandID
                ,[IbuyCarSeriesID] = @IbuyCarSeriesID
                ,[IbuyCarModelID] = @IbuyCarModelID
                ,[IbuyTimeID] = @IbuyTimeID
                ,[QuotedpriceInfo] = @QuotedpriceInfo
                ,[PromotionInfo] = @PromotionInfo
                ,[RemarkInfo] = @RemarkInfo
                ,[OwnerID] = @OwnerID
                ,[Owner] = @Owner
                ,[OwnerPowerGroupID] = @OwnerPowerGroupID
                ,[CustomerSex] = @CustomerSex
                ,[TracktagID] = @TracktagID
                ,[Tracktag] = @Tracktag
                ,[CustomerStatusSource] = @CustomerStatusSource
                ,[CustomerStatus] = @CustomerStatus
                ,[ReservationTime] = @ReservationTime
                ,[VisitTime] = @VisitTime
                ,[LeaveTime] = @LeaveTime
                ,[VisitDuration] = @VisitDuration
                ,[VisitNumber] = @VisitNumber
                ,[IsVisit] = @IsVisit
                ,[LastUpdateUserID] = @LastUpdateUserID
                ,[LastUpdateUser] = @LastUpdateUser
                ,[LastUpdateTime] = @LastUpdateTime
                ,[SbuyCarBrandID] = @SbuyCarBrandID
                ,[SbuyCarSeriesID] = @SbuyCarSeriesID
                ,[SbuyCarModelID] = @SbuyCarModelID
                ,[OrderNumber] = @OrderNumber
                ,[KnockdownPrice] = @KnockdownPrice
                ,[PlaceOrderTime] = @PlaceOrderTime
                ,[PicupcarTime] = @PicupcarTime
                ,[GiveupCauseID] = @GiveupCauseID
                ,[SystemRemark] = @SystemRemark
                ,[CheckStatus] = @CheckStatus
                ,[LurkStatus] = @LurkStatus
                ,[DelState] = @DelState
                ,[PostTime] = @PostTime
            WHERE [ID] = @ID
            ";

            SqlParameter[] p = 
            {
                new SqlParameter("@ID",entity.ID),
                new SqlParameter("@Name",entity.Name),
                new SqlParameter("@BackupPhone",entity.BackupPhone),
                new SqlParameter("@ProvinceID",entity.ProvinceID),
                new SqlParameter("@CityID",entity.CityID),
                new SqlParameter("@DistrictID",entity.DistrictID),
                new SqlParameter("@Address",entity.Address),
                new SqlParameter("@WeixinAccount",entity.WeixinAccount),
                new SqlParameter("@InfoTypeID",entity.InfoTypeID),
                new SqlParameter("@InfoSourceID",entity.InfoSourceID),
                new SqlParameter("@PaymentWayID",entity.PaymentWayID),
                new SqlParameter("@IbuyCarBrandID",entity.IbuyCarBrandID),
                new SqlParameter("@IbuyCarSeriesID",entity.IbuyCarSeriesID),
                new SqlParameter("@IbuyCarModelID",entity.IbuyCarModelID),
                new SqlParameter("@IbuyTimeID",entity.IbuyTimeID),
                new SqlParameter("@QuotedpriceInfo",entity.QuotedpriceInfo),
                new SqlParameter("@PromotionInfo",entity.PromotionInfo),
                new SqlParameter("@RemarkInfo",entity.RemarkInfo),
                new SqlParameter("@OwnerID",entity.OwnerID),
                new SqlParameter("@Owner",entity.Owner),
                new SqlParameter("@OwnerPowerGroupID",entity.OwnerPowerGroupID),
                new SqlParameter("@CustomerSex",entity.CustomerSex),
                new SqlParameter("@TracktagID",entity.TracktagID),
                new SqlParameter("@Tracktag",entity.Tracktag),
                new SqlParameter("@CustomerStatusSource",entity.CustomerStatusSource),
                new SqlParameter("@CustomerStatus",entity.CustomerStatus),
                new SqlParameter("@ReservationTime",entity.ReservationTime),
                new SqlParameter("@VisitTime",entity.VisitTime),
                new SqlParameter("@LeaveTime",entity.LeaveTime),
                new SqlParameter("@VisitDuration",entity.VisitDuration),
                new SqlParameter("@VisitNumber",entity.VisitNumber),
                new SqlParameter("@IsVisit",entity.IsVisit),
                new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                new SqlParameter("@LastUpdateUser",entity.LastUpdateUser),
                new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                new SqlParameter("@SbuyCarBrandID",entity.SbuyCarBrandID),
                new SqlParameter("@SbuyCarSeriesID",entity.SbuyCarSeriesID),
                new SqlParameter("@SbuyCarModelID",entity.SbuyCarModelID),
                new SqlParameter("@OrderNumber",entity.OrderNumber),
                new SqlParameter("@KnockdownPrice",entity.KnockdownPrice),
                new SqlParameter("@PlaceOrderTime",entity.PlaceOrderTime),
                new SqlParameter("@PicupcarTime",entity.PicupcarTime),
                new SqlParameter("@GiveupCauseID",entity.GiveupCauseID),
                new SqlParameter("@SystemRemark",entity.SystemRemark),
                new SqlParameter("@CheckStatus",entity.CheckStatus),
                new SqlParameter("@LurkStatus",entity.LurkStatus),
                new SqlParameter("@DelState",entity.DelState),
                new SqlParameter("@PostTime",entity.PostTime)
            };
            return DataConvert.SafeInt(SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p));
        }

        public override void DeleteCustomer(string ids, int corpid)
        {
            string sql = "UPDATE ComOpp_Customer SET [DelState] = 1 WHERE [CorporationID] = @CorporationID AND [ID] IN(" + ids + ")";
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, new SqlParameter("@CorporationID",corpid));
        }

        public override int UpdateCustomerLastConnect(CustomerInfo entity)
        {
            string sql = @"
            UPDATE ComOpp_Customer  SET 
                [LastConnectTime] = @LastConnectTime
                ,[LastConnectDetail] = @LastConnectDetail
                ,[LastConnectUserID] = @LastConnectUserID
                ,[LastConnectUser] = @LastConnectUser
                ,[LastConnectwayID] = @LastConnectwayID
                ,[LastCustomerLevelID] = @LastCustomerLevelID
                ,[LastUpdateTime] = @LastUpdateTime
                ,[LastUpdateUserID] = @LastUpdateUserID
                ,[LastUpdateUser] = @LastUpdateUser
                ,[ConnectTimes] = @ConnectTimes
            WHERE [ID] = @ID
            ";
            SqlParameter[] p = 
            {
                new SqlParameter("@ID",entity.ID),
                new SqlParameter("@LastConnectTime",entity.LastConnectTime),
                new SqlParameter("@LastConnectDetail",entity.LastConnectDetail),
                new SqlParameter("@LastConnectUserID",entity.LastConnectUserID),
                new SqlParameter("@LastConnectUser",entity.LastConnectUser),
                new SqlParameter("@LastConnectwayID",entity.LastConnectwayID),
                new SqlParameter("@LastCustomerLevelID",entity.LastCustomerLevelID),
                new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                new SqlParameter("@LastUpdateUser",entity.LastUpdateUser),
                new SqlParameter("@ConnectTimes",entity.ConnectTimes)
            };
            return DataConvert.SafeInt(SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p));
        }

        public override void UpdateCustomerPhoneVest(CustomerInfo entity)
        {
            string sql = "UPDATE ComOpp_Customer SET [PhoneVest] = @PhoneVest WHERE [ID] = @ID";
            SqlParameter[] p = 
            { 
                new SqlParameter("@PhoneVest",entity.PhoneVest),
                new SqlParameter("@ID",entity.ID)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override void SetCustomerLevel(int id, int level)
        {
            string sql = "UPDATE ComOpp_Customer SET [CustomerLevelID] = @CustomerLevelID WHERE [ID] = @ID";
            SqlParameter[] p = 
            { 
                new SqlParameter("@CustomerLevelID",level),
                new SqlParameter("@ID",id)
            };
            SqlHelper.ExecuteNonQuery(_con, CommandType.Text, sql, p);
        }

        public override CustomerInfo GetCustomerByPhone(string phone)
        {
            CustomerInfo result = null;
            string sql = "SELECT * FROM ComOpp_Customer WHERE [Phone] = @Phone";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql,new SqlParameter("@Phone",phone)))
            {
                if (reader.Read())
                {
                    result = PopulateCustomerInfo(reader);
                }
            }
            return result;
        }

        public override CustomerInfo GetCustomerByID(int id)
        {
            CustomerInfo result = null;
            string sql = "SELECT * FROM ComOpp_Customer WHERE [ID] = @ID";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@ID", id)))
            {
                if (reader.Read())
                {
                    result = PopulateCustomerInfo(reader);
                }
            }
            return result;
        }

        public override List<CustomerInfo> GetCustomerListForPhoneVest()
        {
            List<CustomerInfo> list = new List<CustomerInfo>();

            string sql = "SELECT * FROM ComOpp_Customer WHERE [PhoneVest] = '' AND [DelState] = 0";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCustomerInfo(reader));
                }
            }

            return list;
        }

        public override List<CustomerInfo> GetCustomerListByCorporation(int cid)
        {
            List<CustomerInfo> list = new List<CustomerInfo>();

            string sql = "SELECT * FROM ComOpp_Customer WHERE [CorporationID] = @CorporationID AND [DelState] = 0";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@CorporationID",cid)))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCustomerInfo(reader));
                }
            }

            return list;
        }

        #endregion

        #region 客户线索流转记录

        public override int AddCustomerMoveRecord(CustomerMoveRecordInfo entity)
        {
            string sql = @"
                INSERT INTO ComOpp_CustomerMoveRecord(
                    [CustomerID],[CustomerStatus],[CustomerStatusSource],[OwnerID],[Owner],[LastUpdateUserID],[LastUpdateUser],[CreateTime],[SystemMsg]
                )VALUES(
                    @CustomerID,@CustomerStatus,@CustomerStatusSource,@OwnerID,@Owner,@LastUpdateUserID,@LastUpdateUser,@CreateTime,@SystemMsg
                );
                SELECT @@IDENTITY;
            ";
            SqlParameter[] p = 
            {
                new SqlParameter("CustomerID",entity.CustomerID),
                new SqlParameter("CustomerStatus",entity.CustomerStatus),
                new SqlParameter("CustomerStatusSource",entity.CustomerStatusSource),
                new SqlParameter("OwnerID",entity.OwnerID),
                new SqlParameter("Owner",entity.Owner),
                new SqlParameter("LastUpdateUserID",entity.LastUpdateUserID),
                new SqlParameter("LastUpdateUser",entity.LastUpdateUser),
                new SqlParameter("CreateTime",entity.CreateTime),
                new SqlParameter("SystemMsg",entity.SystemMsg)
            };
            entity.ID = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
            return entity.ID;
        }

        public override List<CustomerMoveRecordInfo> GetCustomerMoveRecordListByCustomerID(int cid)
        {
            List<CustomerMoveRecordInfo> list = new List<CustomerMoveRecordInfo>();

            string sql = "SELECT * FROM ComOpp_CustomerMoveRecord WHERE [CustomerID] = @CustomerID";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@CustomerID", cid)))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCustomerMoveRecordInfo(reader));
                }
            }

            return list;
        }

        public override CustomerMoveRecordInfo GetCustomerMoveRecordByID(int id)
        {
            CustomerMoveRecordInfo result = null;
            string sql = "SELECT * FROM ComOpp_CustomerMoveRecord WHERE [ID] = @ID";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@ID", id)))
            {
                if (reader.Read())
                {
                    result = PopulateCustomerMoveRecordInfo(reader);
                }
            }
            return result;
        }

        #endregion

        #region 客户跟踪记录

        public override int AddCustomerConnectRecord(CustomerConnectRecordInfo entity)
        {
            string sql = @"
                INSERT INTO ComOpp_CustomerConnectRecord(
                    [CustomerID],[CustomerName],[CustomerPhone],[ConnectUserID],[ConnectUser],[FollowTime],[ConnectwayID],[CustomerLevelID],[ConnectDetail],[CreateTime],[CorporationID]
                )VALUES(
                    @CustomerID,@CustomerName,@CustomerPhone,@ConnectUserID,@ConnectUser,@FollowTime,@ConnectwayID,@CustomerLevelID,@ConnectDetail,@CreateTime,@CorporationID
                );
                SELECT @@IDENTITY;
            ";
            SqlParameter[] p = 
            {
                new SqlParameter("CustomerID",entity.CustomerID),
                new SqlParameter("CustomerName",entity.CustomerName),
                new SqlParameter("CustomerPhone",entity.CustomerPhone),
                new SqlParameter("ConnectUserID",entity.ConnectUserID),
                new SqlParameter("ConnectUser",entity.ConnectUser),
                new SqlParameter("FollowTime",entity.FollowTime),
                new SqlParameter("ConnectwayID",entity.ConnectwayID),
                new SqlParameter("CustomerLevelID",entity.CustomerLevelID),
                new SqlParameter("ConnectDetail",entity.ConnectDetail),
                new SqlParameter("CreateTime",entity.CreateTime),
                new SqlParameter("CorporationID",entity.CorporationID)
            };
            entity.ID = DataConvert.SafeInt(SqlHelper.ExecuteScalar(_con, CommandType.Text, sql, p));
            return entity.ID;
        }

        public override List<CustomerConnectRecordInfo> GetCustomerConnectRecordListByCustomerID(int cid)
        {
            List<CustomerConnectRecordInfo> list = new List<CustomerConnectRecordInfo>();

            string sql = "SELECT * FROM ComOpp_CustomerConnectRecord WHERE [CustomerID] = @CustomerID";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@CustomerID", cid)))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCustomerConnectRecordInfo(reader));
                }
            }

            return list;
        }

        public override CustomerConnectRecordInfo GetCustomerConnectRecordByID(int id)
        {
            CustomerConnectRecordInfo result = null;
            string sql = "SELECT * FROM ComOpp_CustomerConnectRecord WHERE [ID] = @ID";
            using (IDataReader reader = SqlHelper.ExecuteReader(_con, CommandType.Text, sql, new SqlParameter("@ID", id)))
            {
                if (reader.Read())
                {
                    result = PopulateCustomerConnectRecordInfo(reader);
                }
            }
            return result;
        }

        public override List<CustomerConnectRecordInfo> GetCustomerConnectRecordList(CustomerConnectRecordQuery query, int pageindex, int pagesize, ref int recordcount)
        {
            List<CustomerConnectRecordInfo> list = new List<CustomerConnectRecordInfo>();

            SqlParameter p;

            using (IDataReader reader = CommonPageSql.GetDataReaderByPager(_con, pageindex, pagesize, query, out p))
            {
                while (reader.Read())
                {
                    list.Add(PopulateCustomerConnectRecordInfo(reader));
                }
            }
            recordcount = int.Parse(p.Value.ToString());

            return list;
        }

        #endregion
    }
}
