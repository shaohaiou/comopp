using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using ComOpp.Tools.Web;
using ComOpp.Tools;

namespace ComOpp.Components
{
    public class Customers
    {
        #region 单例
        private static object sync_creater = new object();

        private static Customers _instance;
        public static Customers Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Customers();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 更新号码归属作业

        public void PhoneVestUpdate()
        {
            string url = "http://v.showji.com/Locating/showji.com20150416273007.aspx?m={0}&output=json";
            Regex rprovince = new Regex("\"Province\":\"([\\s\\S]*?)\"");
            Regex rcity = new Regex("\"City\":\"([\\s\\S]*?)\"");
            List<CustomerInfo> list = GetCustomerListForPhoneVest();
            WebClient wc = new WebClient();
            foreach (CustomerInfo c in list)
            {
                try
                {
                    string request = Http.GetPage(string.Format(url, c.Phone));
                    if (rprovince.IsMatch(request) && rcity.IsMatch(request))
                    {
                        string province = rprovince.Match(request).Groups[1].Value;
                        string city = rcity.Match(request).Groups[1].Value;
                        if (!string.IsNullOrEmpty(province) && !string.IsNullOrEmpty(city))
                        {
                            c.PhoneVest = province + " " + city;
                            UpdateCustomerPhoneVest(c);
                            RefreshCustomerCache(c);
                            Thread.Sleep(5000);
                        }
                    }
                }
                catch { }
            }
        }

        #endregion

        /// <summary>
        /// 更新客户号码归属地
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateCustomerPhoneVest(CustomerInfo entity)
        {
            CommonDataProvider.Instance().UpdateCustomerPhoneVest(entity);
        }

        /// <summary>
        /// 更新客户线索缓存
        /// </summary>
        /// <param name="entity"></param>
        public void RefreshCustomerCache(CustomerInfo entity)
        {
            string key = GlobalKey.CUSTOMER_LIST + "_corp" + entity.CorporationID;
            List<CustomerInfo> list = MangaCache.Get(key) as List<CustomerInfo>;
            entity = GetCustomerByID(entity.ID);
            if (list != null && list.Exists(c => c.ID == entity.ID))
            {
                list[list.FindIndex(c => c.ID == entity.ID)] = entity;
            }
            else if (list == null)
            {
                GetCustomerListByCorporation(entity.CorporationID, true);
            }
            else
                list.Add(entity);
        }

        /// <summary>
        /// 更新指定公司的客户线索缓存
        /// </summary>
        /// <param name="corpid"></param>
        public void ReloadCustomerListByCorporationCache(int corpid)
        {
            string key = GlobalKey.CUSTOMER_LIST + "_corp" + corpid;
            MangaCache.Remove(key);
            GetCustomerListByCorporation(corpid, true);
        }

        public int Add(CustomerInfo entity)
        {
            int id = CommonDataProvider.Instance().AddCustomer(entity);

            if (id > 0)
            {
                RefreshCustomerCache(entity);

                CustomerMoveRecordInfo minfo = new CustomerMoveRecordInfo()
                {
                    CustomerID = id,
                    CustomerStatus = entity.CustomerStatus,
                    OwnerID = entity.OwnerID,
                    Owner = entity.Owner,
                    LastUpdateUserID = entity.LastUpdateUserID,
                    LastUpdateUser = entity.LastUpdateUser,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    SystemMsg = entity.SystemRemark
                };

                CustomerMoveRecords.Instance.Add(minfo);
            }

            return id;
        }

        public int Update(CustomerInfo entity)
        {
            int result = 0;
            CustomerInfo entityold = GetCustomerByID(entity.ID);

            if (entity.CustomerStatus != entityold.CustomerStatus || entity.OwnerID != entityold.OwnerID)
            {
                CustomerMoveRecordInfo minfo = new CustomerMoveRecordInfo()
                {
                    CustomerID = entity.ID,
                    CustomerStatus = entity.CustomerStatus,
                    CustomerStatusSource = entityold.CustomerStatus,
                    OwnerID = entity.OwnerID,
                    Owner = entity.Owner,
                    LastUpdateUserID = entity.LastUpdateUserID,
                    LastUpdateUser = entity.LastUpdateUser,
                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                };
                if (entity.CustomerStatus == (int)CustomerStatus.潜客_战败)
                    minfo.SystemMsg = DateTime.Today.ToString("yyyy年MM月dd日") + "战败";
                else if (entity.CustomerStatus == (int)CustomerStatus.潜客_转出)
                    minfo.SystemMsg = DateTime.Today.ToString("yyyy年MM月dd日") + "由" + entityold.CustomerStatusName + "转出";
                else if (entity.CustomerStatus != entityold.CustomerStatus)
                    minfo.SystemMsg = DateTime.Today.ToString("yyyy年MM月dd日") + "由" + entityold.CustomerStatusName + "转入";
                else if (entity.OwnerID != entityold.OwnerID)
                    minfo.SystemMsg = DateTime.Today.ToString("yyyy年MM月dd日") + "变更线索所有人";

                CustomerMoveRecords.Instance.Add(minfo);

                if (entity.CustomerStatus != (int)CustomerStatus.潜客_战败 && entity.CustomerStatus != (int)CustomerStatus.潜客_转出)
                    entity.SystemRemark = minfo.SystemMsg;
                if (entity.OwnerID != entityold.OwnerID)
                {
                    AdminInfo owner = Admins.Instance.GetAdmin(entity.OwnerID);
                    entity.OwnerPowerGroupID = owner.PowerGroupID;
                }
            }

            result = CommonDataProvider.Instance().UpdateCustomer(entity);

            if (result > 0)
            {
                RefreshCustomerCache(entity);
            }

            return result;
        }

        public void Delete(string ids, int corpid)
        {
            CommonDataProvider.Instance().DeleteCustomer(ids, corpid);
        }

        public int Move(CustomerInfo entity)
        {
            int result = 0;
            CustomerMoveRecordInfo minfo = new CustomerMoveRecordInfo()
            {
                CustomerID = entity.ID,
                CustomerStatus = entity.CustomerStatus,
                CustomerStatusSource = entity.CustomerStatus,
                OwnerID = entity.OwnerID,
                Owner = entity.Owner,
                LastUpdateUserID = entity.LastUpdateUserID,
                LastUpdateUser = entity.LastUpdateUser,
                CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                SystemMsg = DateTime.Today.ToString("yyyy年MM月dd日") + "线索批量移交"
            };

            CustomerMoveRecords.Instance.Add(minfo);

            entity.SystemRemark = minfo.SystemMsg;

            result = CommonDataProvider.Instance().UpdateCustomer(entity);

            if (result > 0)
            {
                RefreshCustomerCache(entity);
            }

            return result;
        }

        public int UpdateCustomerLastConnect(CustomerInfo entity)
        {
            int result = 0;
            result = CommonDataProvider.Instance().UpdateCustomerLastConnect(entity);

            if (result > 0)
            {
                RefreshCustomerCache(entity);
            }

            return result;
        }


        public CustomerInfo GetCustomerByPhone(string phone)
        {
            return CommonDataProvider.Instance().GetCustomerByPhone(phone);
        }

        public CustomerInfo GetCustomerByID(int id)
        {
            return CommonDataProvider.Instance().GetCustomerByID(id);
        }

        public List<CustomerInfo> GetCustomerListForPhoneVest()
        {
            return CommonDataProvider.Instance().GetCustomerListForPhoneVest();
        }

        public List<CustomerInfo> GetCustomerListByCorporation(int cid, bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCustomerListByCorporation(cid);

            string key = GlobalKey.CUSTOMER_LIST + "_corp" + cid;
            List<CustomerInfo> list = MangaCache.Get(key) as List<CustomerInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCustomerListByCorporation(cid);
                MangaCache.Max(key, list);
            }

            return list;
        }

        #region 线索强制转出

        private static bool hasruncustomerforcedout = false;

        public void CustomerForcedout()
        {
            if (DateTime.Now.Hour == 2 && DateTime.Now.Minute == 1)
            {
                if (!hasruncustomerforcedout)
                {
                    try
                    {
                        List<CorporationInfo> list = Corporations.Instance.GetList(true);
                        foreach (CorporationInfo corpinfo in list)
                        {
                            List<CustomerInfo> listForced = new List<CustomerInfo>();
                            List<CustomerInfo> clist = GetCustomerListByCorporation(corpinfo.ID, true);
                            if (corpinfo.Forcedoffday > 0)
                            {
                                string[] offcustomerlevel = corpinfo.Offcustomerlevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                listForced.AddRange(clist.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corpinfo.Forcedoffday - 1) && !offcustomerlevel.Contains(l.LastCustomerLevelID.ToString())));

                            }
                            if (corpinfo.Forcedoutday > 0)
                            {
                                string[] forcedoutdaylevel = corpinfo.Forcedoutdaylevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                listForced.AddRange(clist.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corpinfo.Forcedoutday - 1) && !forcedoutdaylevel.Contains(l.LastCustomerLevelID.ToString())));
                            }
                            foreach (CustomerInfo entity in listForced)
                            {
                                entity.CustomerStatusSource = entity.CustomerStatus;
                                entity.CustomerStatus = (int)CustomerStatus.潜客_转出;
                                entity.LurkStatus = 1;

                                CustomerMoveRecordInfo minfo = new CustomerMoveRecordInfo()
                                {
                                    CustomerID = entity.ID,
                                    CustomerStatus = entity.CustomerStatus,
                                    CustomerStatusSource = entity.CustomerStatus,
                                    OwnerID = entity.OwnerID,
                                    Owner = entity.Owner,
                                    LastUpdateUserID = entity.LastUpdateUserID,
                                    LastUpdateUser = entity.LastUpdateUser,
                                    CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                    SystemMsg = DateTime.Today.ToString("yyyy年MM月dd日") + "线索强制转出"
                                };

                                CustomerMoveRecords.Instance.Add(minfo);

                                entity.SystemRemark = minfo.SystemMsg;

                                CommonDataProvider.Instance().UpdateCustomer(entity);
                            }
                            if (listForced.Count > 0)
                                ReloadCustomerListByCorporationCache(corpinfo.ID);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExpLog.Write(ex);
                    }
                    finally
                    {
                        hasruncustomerforcedout = true;
                    }
                }
            }
            else
                hasruncustomerforcedout = false;
        }

        #endregion
    }
}
