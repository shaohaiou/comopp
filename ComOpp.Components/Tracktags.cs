using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Tracktags: BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static Tracktags _instance = null;
        public static Tracktags Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Tracktags("ComOpp_Tracktag");
                    }
                }
                return _instance;
            }
        }

        private Tracktags(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<TracktagInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetTracktagList();

            string key = GlobalKey.TRACKTAG_LIST;
            List<TracktagInfo> list = MangaCache.Get(key) as List<TracktagInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetTracktagList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<TracktagInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<TracktagInfo> list = GetList(fromCache);
            List<TracktagInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadTracktagListCache()
        {
            string key = GlobalKey.TRACKTAG_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public TracktagInfo GetModel(int id, bool fromCache = false)
        {
            TracktagInfo entity = null;

            List<TracktagInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            TracktagInfo entity = GetModel(id,true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(TracktagInfo entity)
        {
            CommonDataProvider.Instance().AddTracktag(entity);
        }

        public void Update(TracktagInfo entity)
        {
            CommonDataProvider.Instance().UpdateTracktag(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteTracktag(ids);
        }
    }
}
