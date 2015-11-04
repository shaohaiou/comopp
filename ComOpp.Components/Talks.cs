using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Talks
    {
        #region 单例
        private static object sync_creater = new object();

        private static Talks _instance;
        public static Talks Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Talks();
                    }
                }
                return _instance;
            }
        }

        #endregion

        public List<TalkInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetTalkList();

            string key = GlobalKey.TALK_LIST;
            List<TalkInfo> list = MangaCache.Get(key) as List<TalkInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetTalkList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadTalkListCache()
        {
            string key = GlobalKey.TALK_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public List<TalkInfo> GetListBycorpid(int corpid, bool fromCache = false)
        {
            List<TalkInfo> list = GetList(fromCache);

            return list.FindAll(l=>l.CorporationID == corpid);
        }

        public TalkInfo GetModel(int id, bool fromCache = false)
        {
            TalkInfo entity = null;

            List<TalkInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public void Add(TalkInfo entity)
        {
            CommonDataProvider.Instance().AddTalk(entity);
        }

        public void Update(TalkInfo entity)
        {
            CommonDataProvider.Instance().UpdateTalk(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteTalk(ids);
        }
    }
}
