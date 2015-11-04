using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class CustomerMoveRecords
    {
        #region 单例
        private static object sync_creater = new object();

        private static CustomerMoveRecords _instance;
        public static CustomerMoveRecords Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new CustomerMoveRecords();
                    }
                }
                return _instance;
            }
        }

        #endregion

        public void Add(CustomerMoveRecordInfo entity)
        {
            int id = CommonDataProvider.Instance().AddCustomerMoveRecord(entity);
            if (id > 0)
            {
                RefreshCustomerMoveRecordCache(entity);
            }
        }

        /// <summary>
        /// 更新客户线索流转记录缓存
        /// </summary>
        /// <param name="entity"></param>
        public void RefreshCustomerMoveRecordCache(CustomerMoveRecordInfo entity)
        {
            string key = GlobalKey.CUSTOMERMOVERECORD_LIST + "_c" + entity.CustomerID;
            List<CustomerMoveRecordInfo> list = MangaCache.Get(key) as List<CustomerMoveRecordInfo>;
            entity = GetCustomerMoveRecordByID(entity.ID);
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

        public List<CustomerMoveRecordInfo> GetListByCustomerID(int cid, bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCustomerMoveRecordListByCustomerID(cid);

            string key = GlobalKey.CUSTOMERMOVERECORD_LIST + "_c" + cid;
            List<CustomerMoveRecordInfo> list = MangaCache.Get(key) as List<CustomerMoveRecordInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCustomerMoveRecordListByCustomerID(cid);
                MangaCache.Max(key, list);
            }

            return list;
        }

        public CustomerMoveRecordInfo GetCustomerMoveRecordByID(int id)
        {
            return CommonDataProvider.Instance().GetCustomerMoveRecordByID(id);
        }
    }
}
