using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class CustomerLevels : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static CustomerLevels _instance = null;
        public static CustomerLevels Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new CustomerLevels("ComOpp_CustomerLevel");
                    }
                }
                return _instance;
            }
        }

        private CustomerLevels(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<CustomerLevelInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCustomerLevelList().OrderBy(l => l.Sort).ToList();

            string key = GlobalKey.CUSTOMERLEVEL_LIST;
            List<CustomerLevelInfo> list = MangaCache.Get(key) as List<CustomerLevelInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCustomerLevelList().OrderBy(l => l.Sort).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<CustomerLevelInfo> GetListByCorpid(int corpid,bool fromCache = false)
        {
            List<CustomerLevelInfo> list = GetList(fromCache);
            List<CustomerLevelInfo> listcorp = list.FindAll(l => l.CorporationID == corpid);

            return listcorp.OrderBy(l => l.Sort).ToList();
        }

        public void ReloadCustomerLevelListCache()
        {
            string key = GlobalKey.CUSTOMERLEVEL_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public CustomerLevelInfo GetModel(int id, bool fromCache = false)
        {
            CustomerLevelInfo entity = null;

            List<CustomerLevelInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            CustomerLevelInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(CustomerLevelInfo entity)
        {
            CommonDataProvider.Instance().AddCustomerLevel(entity);
        }

        public void Update(CustomerLevelInfo entity)
        {
            CommonDataProvider.Instance().UpdateCustomerLevel(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteCustomerLevel(ids);
        }
    }
}
