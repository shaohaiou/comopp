using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class InfoTypes : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static InfoTypes _instance = null;
        public static InfoTypes Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new InfoTypes("ComOpp_InfoType");
                    }
                }
                return _instance;
            }
        }

        private InfoTypes(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<InfoTypeInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetInfoTypeList();

            string key = GlobalKey.INFOTYPE_LIST;
            List<InfoTypeInfo> list = MangaCache.Get(key) as List<InfoTypeInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetInfoTypeList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<InfoTypeInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<InfoTypeInfo> list = GetList(fromCache);
            List<InfoTypeInfo> listcorp = list.FindAll(l => l.CorporationID == corpid);
            if (corpid == 0) return listcorp;
            List<InfoTypeInfo> listsys = list.FindAll(l => l.CorporationID == 0);

            if (listsys.Exists(l => !listcorp.Exists(c => c.ParentID == l.ID)))
            {
                foreach (InfoTypeInfo info in listsys.FindAll(l => !listcorp.Exists(c => c.ParentID == l.ID)))
                {
                    InfoTypeInfo newentity = new InfoTypeInfo() 
                    {
                        CorporationID = corpid,
                        DataLevel = 0,
                        Lockday = 0,
                        Locked = 0,
                        Locklevel = string.Empty,
                        LocklevelName = string.Empty,
                        Name = info.Name,
                        ParentID = info.ID
                    };
                    Add(newentity);
                }
                ReloadInfoTypeListCache();
                list = GetList(fromCache);
                listcorp = list.FindAll(l => l.CorporationID == corpid);
            }

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadInfoTypeListCache()
        {
            string key = GlobalKey.INFOTYPE_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public InfoTypeInfo GetModel(int id, bool fromCache = false)
        {
            InfoTypeInfo entity = null;

            List<InfoTypeInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            InfoTypeInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(InfoTypeInfo entity)
        {
            CommonDataProvider.Instance().AddInfoType(entity);
        }

        public void Update(InfoTypeInfo entity)
        {
            CommonDataProvider.Instance().UpdateInfoType(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteInfoType(ids);
        }
    }
}
