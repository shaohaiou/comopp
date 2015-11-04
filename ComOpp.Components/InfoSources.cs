using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class InfoSources : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static InfoSources _instance = null;
        public static InfoSources Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new InfoSources("ComOpp_InfoSource");
                    }
                }
                return _instance;
            }
        }

        private InfoSources(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<InfoSourceInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetInfoSourceList();

            string key = GlobalKey.INFOSOURCE_LIST;
            List<InfoSourceInfo> list = MangaCache.Get(key) as List<InfoSourceInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetInfoSourceList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<InfoSourceInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<InfoSourceInfo> list = GetList(fromCache);
            List<InfoSourceInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadInfoSourceListCache()
        {
            string key = GlobalKey.INFOSOURCE_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public InfoSourceInfo GetModel(int id, bool fromCache = false)
        {
            InfoSourceInfo entity = null;

            List<InfoSourceInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            InfoSourceInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(InfoSourceInfo entity)
        {
            CommonDataProvider.Instance().AddInfoSource(entity);
        }

        public void Update(InfoSourceInfo entity)
        {
            CommonDataProvider.Instance().UpdateInfoSource(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteInfoSource(ids);
        }
    }
}
