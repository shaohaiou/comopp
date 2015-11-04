using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class CustomerConnectRecords
    {
        #region 单例
        private static object sync_creater = new object();

        private static CustomerConnectRecords _instance;
        public static CustomerConnectRecords Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new CustomerConnectRecords();
                    }
                }
                return _instance;
            }
        }

        #endregion

        public int Add(CustomerConnectRecordInfo entity)
        {
            int id = CommonDataProvider.Instance().AddCustomerConnectRecord(entity);
            if (id > 0)
            {
                RefreshCustomerConnectRecordCache(entity);
            }

            return id;
        }

        /// <summary>
        /// 更新客户线索流转记录缓存
        /// </summary>
        /// <param name="entity"></param>
        public void RefreshCustomerConnectRecordCache(CustomerConnectRecordInfo entity)
        {
            string key = GlobalKey.CUSTOMERCONNECTRECORD_LIST + "_c" + entity.CustomerID;
            List<CustomerConnectRecordInfo> list = MangaCache.Get(key) as List<CustomerConnectRecordInfo>;
            entity = GetCustomerConnectRecordByID(entity.ID);
            if (list != null && list.Exists(c => c.ID == entity.ID))
            {
                list[list.FindIndex(c => c.ID == entity.ID)] = entity;
            }
            else if (list == null)
            {
                GetListByCustomerID(entity.CustomerID,true);
            }
            else
                list.Add(entity);
        }

        public List<CustomerConnectRecordInfo> GetListByCustomerID(int cid, bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCustomerConnectRecordListByCustomerID(cid);

            string key = GlobalKey.CUSTOMERCONNECTRECORD_LIST + "_c" + cid;
            List<CustomerConnectRecordInfo> list = MangaCache.Get(key) as List<CustomerConnectRecordInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCustomerConnectRecordListByCustomerID(cid);
                MangaCache.Max(key, list);
            }

            return list;
        }

        public List<CustomerConnectRecordInfo> GetList(CustomerConnectRecordQuery query,int pageindex,int pagesize,ref int recordcount)
        {
            return CommonDataProvider.Instance().GetCustomerConnectRecordList(query, pageindex, pagesize,ref recordcount);
        }

        public CustomerConnectRecordInfo GetCustomerConnectRecordByID(int id)
        {
            return CommonDataProvider.Instance().GetCustomerConnectRecordByID(id);
        }
    }
}
