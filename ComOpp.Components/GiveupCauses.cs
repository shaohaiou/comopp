using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class GiveupCauses : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static GiveupCauses _instance = null;
        public static GiveupCauses Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new GiveupCauses("ComOpp_GiveupCause");
                    }
                }
                return _instance;
            }
        }

        private GiveupCauses(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<GiveupCauseInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetGiveupCauseList();

            string key = GlobalKey.GIVEUPCAUSE_LIST;
            List<GiveupCauseInfo> list = MangaCache.Get(key) as List<GiveupCauseInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetGiveupCauseList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<GiveupCauseInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<GiveupCauseInfo> list = GetList(fromCache);
            List<GiveupCauseInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadGiveupCauseListCache()
        {
            string key = GlobalKey.GIVEUPCAUSE_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public string GetName(int id)
        {
            GiveupCauseInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public GiveupCauseInfo GetModel(int id, bool fromCache = false)
        {
            GiveupCauseInfo entity = null;

            List<GiveupCauseInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public void Add(GiveupCauseInfo entity)
        {
            CommonDataProvider.Instance().AddGiveupCause(entity);
        }

        public void Update(GiveupCauseInfo entity)
        {
            CommonDataProvider.Instance().UpdateGiveupCause(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteGiveupCause(ids);
        }
    }
}
