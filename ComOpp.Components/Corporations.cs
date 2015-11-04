using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Corporations : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static Corporations _instance = null;
        public static Corporations Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Corporations("ComOpp_Corporation");
                    }
                }
                return _instance;
            }
        }

        private Corporations(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<CorporationInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCorporationList().OrderBy(l => l.Sort).ToList();

            string key = GlobalKey.CORPORATION_LIST;
            List<CorporationInfo> list = MangaCache.Get(key) as List<CorporationInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCorporationList().OrderBy(l => l.Sort).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadCorporationListCache()
        {
            string key = GlobalKey.CORPORATION_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public CorporationInfo GetModel(int id, bool fromCache = false)
        {
            CorporationInfo entity = null;

            List<CorporationInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public void Add(CorporationInfo entity)
        {
            CommonDataProvider.Instance().AddCorporation(entity);
        }

        public void Update(CorporationInfo entity)
        {
            CommonDataProvider.Instance().UpdateCorporation(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteCorporation(ids);
        }
    }
}
