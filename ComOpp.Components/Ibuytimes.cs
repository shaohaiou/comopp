using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Ibuytimes: BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static Ibuytimes _instance = null;
        public static Ibuytimes Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Ibuytimes("ComOpp_Ibuytime");
                    }
                }
                return _instance;
            }
        }

        private Ibuytimes(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<IbuytimeInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetIbuytimeList();

            string key = GlobalKey.IBUYTIME_LIST;
            List<IbuytimeInfo> list = MangaCache.Get(key) as List<IbuytimeInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetIbuytimeList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<IbuytimeInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<IbuytimeInfo> list = GetList(fromCache);
            List<IbuytimeInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadIbuytimeListCache()
        {
            string key = GlobalKey.IBUYTIME_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public IbuytimeInfo GetModel(int id, bool fromCache = false)
        {
            IbuytimeInfo entity = null;

            List<IbuytimeInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            IbuytimeInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(IbuytimeInfo entity)
        {
            CommonDataProvider.Instance().AddIbuytime(entity);
        }

        public void Update(IbuytimeInfo entity)
        {
            CommonDataProvider.Instance().UpdateIbuytime(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteIbuytime(ids);
        }
    }
}
