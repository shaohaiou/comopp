using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Modules : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static Modules _instance = null;
        public static Modules Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Modules("ComOpp_Module");
                    }
                }
                return _instance;
            }
        }

        private Modules(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<ModuleInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetModuleList().OrderBy(l => l.Sort).ToList();

            string key = GlobalKey.MODULE_LIST;
            List<ModuleInfo> list = MangaCache.Get(key) as List<ModuleInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetModuleList().OrderBy(l => l.Sort).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadModuleListCache()
        {
            string key = GlobalKey.MODULE_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public ModuleInfo GetModel(int id, bool fromCache = false)
        {
            ModuleInfo entity = null;

            List<ModuleInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public void Add(ModuleInfo entity)
        {
            CommonDataProvider.Instance().AddModule(entity);
        }

        public void Update(ModuleInfo entity)
        {
            CommonDataProvider.Instance().UpdateModule(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeleteModule(ids);
        }
    }
}
