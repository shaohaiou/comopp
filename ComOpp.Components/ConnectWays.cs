using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class ConnectWays: BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static ConnectWays _instance = null;
        public static ConnectWays Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new ConnectWays("ComOpp_ConnectWay");
                    }
                }
                return _instance;
            }
        }

        private ConnectWays(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<ConnectWayInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetConnectWayList();

            string key = GlobalKey.CONNECTWAY_LIST;
            List<ConnectWayInfo> list = MangaCache.Get(key) as List<ConnectWayInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetConnectWayList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<ConnectWayInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<ConnectWayInfo> list = GetList(fromCache);
            List<ConnectWayInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadConnectWayListCache()
        {
            string key = GlobalKey.CONNECTWAY_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public ConnectWayInfo GetModel(int id, bool fromCache = false)
        {
            ConnectWayInfo entity = null;

            List<ConnectWayInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            ConnectWayInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(ConnectWayInfo entity)
        {
            CommonDataProvider.Instance().AddConnectWay(entity);
        }

        public void Update(ConnectWayInfo entity)
        {
            CommonDataProvider.Instance().UpdateConnectWay(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteConnectWay(ids);
        }
    }
}
