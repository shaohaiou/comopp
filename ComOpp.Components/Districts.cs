using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class Districts
    {
        #region 单例
        private static object sync_creater = new object();

        private static Districts _instance;
        public static Districts Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Districts();
                    }
                }
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// 获取省份列表
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public List<ProvinceInfo> GetProvinceList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetProvinceList();

            string key = GlobalKey.PROVINCE_LIST;
            List<ProvinceInfo> list = MangaCache.Get(key) as List<ProvinceInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetProvinceList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        /// <summary>
        /// 重新加载省份列表缓存
        /// </summary>
        public void ReloadProvinceListCache()
        {
            string key = GlobalKey.PROVINCE_LIST;
            MangaCache.Remove(key);
            GetProvinceList(true);
        }

        public string GetProvinceName(int id)
        {
            List<ProvinceInfo> list = GetProvinceList(true);
            if (list.Exists(p => p.ID == id)) return list.Find(p => p.ID == id).Name;
            return string.Empty;
        }

        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public List<CityInfo> GetCityList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCityList();

            string key = GlobalKey.CITY_LIST;
            List<CityInfo> list = MangaCache.Get(key) as List<CityInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCityList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<CityInfo> GetCityListByPID(int pid,bool fromCache = false)
        {
            List<CityInfo> list = GetCityList(fromCache);
            list = list.FindAll(l=>l.ProvinceID == pid);

            return list;
        }

        /// <summary>
        /// 重新加载城市列表缓存
        /// </summary>
        public void ReloadCityListCache()
        {
            string key = GlobalKey.CITY_LIST;
            MangaCache.Remove(key);
            GetCityList(true);
        }

        public string GetCityName(int id)
        {
            List<CityInfo> list = GetCityList(true);
            if (list.Exists(c => c.ID == id)) return list.Find(c => c.ID == id).Name;
            return string.Empty;
        }

        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public List<DistrictInfo> GetDistrictList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetDistrictList();

            string key = GlobalKey.DISTRICT_LIST;
            List<DistrictInfo> list = MangaCache.Get(key) as List<DistrictInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetDistrictList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<DistrictInfo> GetDistrictListByCID(int cid,bool fromCache = false)
        {
            List<DistrictInfo> list = GetDistrictList(fromCache);
            list = list.FindAll(l=>l.CityID == cid);
            return list;
        }

        /// <summary>
        /// 重新加载地区列表缓存
        /// </summary>
        public void ReloadDistrictListCache()
        {
            string key = GlobalKey.DISTRICT_LIST;
            MangaCache.Remove(key);
            GetDistrictList(true);
        }

        public string GetDistrictName(int id)
        {
            List<DistrictInfo> list = GetDistrictList(true);
            if (list.Exists(d => d.ID == id)) return list.Find(d => d.ID == id).Name;
            return string.Empty;
        }
    }
}
