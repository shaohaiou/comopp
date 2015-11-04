using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class PowerGroups : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static PowerGroups _instance = null;
        public static PowerGroups Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new PowerGroups("ComOpp_PowerGroup");
                    }
                }
                return _instance;
            }
        }

        private PowerGroups(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<PowerGroupInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetPowerGroupList().OrderBy(l => l.Sort).ToList();

            string key = GlobalKey.POWERGROUP_LIST;
            List<PowerGroupInfo> list = MangaCache.Get(key) as List<PowerGroupInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetPowerGroupList().OrderBy(l => l.Sort).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadPowerGroupListCache()
        {
            string key = GlobalKey.POWERGROUP_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public List<PowerGroupInfo> GetListByCorporation(int corpid, bool fromCache = false)
        {
            List<PowerGroupInfo> list = GetList(fromCache);
            return list.FindAll(l=>l.CorporationID == corpid);
        }

        public PowerGroupInfo GetModel(int id, bool fromCache = false)
        {
            PowerGroupInfo entity = null;

            List<PowerGroupInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public void Add(PowerGroupInfo entity)
        {
            CommonDataProvider.Instance().AddPowerGroup(entity);
        }

        public void Update(PowerGroupInfo entity)
        {
            CommonDataProvider.Instance().UpdatePowerGroup(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeletePowerGroup(ids);
        }
    }
}
