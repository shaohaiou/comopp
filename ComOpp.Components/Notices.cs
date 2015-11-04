using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Notices
    {
        #region 单例
        private static object sync_creater = new object();

        private static Notices _instance;
        public static Notices Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Notices();
                    }
                }
                return _instance;
            }
        }

        #endregion

        public List<NoticeInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetNoticeList();

            string key = GlobalKey.NOTICE_LIST;
            List<NoticeInfo> list = MangaCache.Get(key) as List<NoticeInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetNoticeList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<NoticeInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<NoticeInfo> list = GetList(fromCache);
            List<NoticeInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp;
        }

        public void ReloadNoticeListCache()
        {
            string key = GlobalKey.NOTICE_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public NoticeInfo GetModel(int id, bool fromCache = false)
        {
            NoticeInfo entity = null;

            List<NoticeInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public void Add(NoticeInfo entity)
        {
            CommonDataProvider.Instance().AddNotice(entity);
        }

        public void Update(NoticeInfo entity)
        {
            CommonDataProvider.Instance().UpdateNotice(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteNotice(ids);
        }
    }
}
