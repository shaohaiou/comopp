using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComOpp.Tools.Web;
using ComOpp.Tools;
using Newtonsoft.Json;

namespace ComOpp.Components
{
    public class Cars
    {
        #region 单例
        private static object sync_creater = new object();

        private static Cars _instance;
        public static Cars Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new Cars();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 品牌管理

        public List<CarBrandInfo> GetCarBrandList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCarBrandList().OrderBy(l => l.NameIndex).ThenBy(l => l.Name).ToList();

            string key = GlobalKey.CARBRAND_LIST;
            List<CarBrandInfo> list = MangaCache.Get(key) as List<CarBrandInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCarBrandList().OrderBy(l => l.NameIndex).ThenBy(l => l.Name).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadCarBrandListCache()
        {
            string key = GlobalKey.CARBRAND_LIST;
            MangaCache.Remove(key);
            GetCarBrandList(true);
        }

        public CarBrandInfo GetCarBrandModel(int id, bool fromCache = false)
        {
            CarBrandInfo entity = null;

            List<CarBrandInfo> list = GetCarBrandList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetCarBrandName(int id)
        {
            CarBrandInfo entity = GetCarBrandModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public int AddCarBrand(CarBrandInfo entity)
        {
            return CommonDataProvider.Instance().AddCarBrand(entity);
        }

        public void UpdateCarBrand(CarBrandInfo entity)
        {
            CommonDataProvider.Instance().UpdateCarBrand(entity);
        }

        public void DeleteCarBrand(string ids)
        {
            CommonDataProvider.Instance().DeleteCarBrand(ids);
        }

        #endregion

        #region 车系管理

        public List<CarSeriesInfo> GetCarSeriesList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCarSeriesList().OrderBy(l => l.BrandNameIndex).ThenBy(l => l.Name).ToList();

            string key = GlobalKey.CARSERIES_LIST;
            List<CarSeriesInfo> list = MangaCache.Get(key) as List<CarSeriesInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCarSeriesList().OrderBy(l => l.BrandNameIndex).ThenBy(l => l.Name).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<CarSeriesInfo> GetCarSeriesListByBrandID(int brandid, bool fromCache = false)
        {
            List<CarSeriesInfo> list = GetCarSeriesList(fromCache);
            return list.FindAll(l => l.BrandID == brandid);
        }

        public void ReloadCarSeriesListCache()
        {
            string key = GlobalKey.CARSERIES_LIST;
            MangaCache.Remove(key);
            GetCarSeriesList(true);
        }

        public CarSeriesInfo GetCarSeriesModel(int id, bool fromCache = false)
        {
            CarSeriesInfo entity = null;

            List<CarSeriesInfo> list = GetCarSeriesList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetCarSeriesName(int id)
        {
            CarSeriesInfo entity = GetCarSeriesModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public int AddCarSeries(CarSeriesInfo entity)
        {
            return CommonDataProvider.Instance().AddCarSeries(entity);
        }

        public void UpdateCarSeries(CarSeriesInfo entity)
        {
            CommonDataProvider.Instance().UpdateCarSeries(entity);
        }

        public void DeleteCarSeries(string ids)
        {
            CommonDataProvider.Instance().DeleteCarSeries(ids);
        }

        #endregion

        #region 车型管理

        public List<CarModelInfo> GetCarModelList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetCarModelList().OrderBy(l => l.BrandNameIndex).ThenBy(l => l.SeriesName).ThenBy(l => l.Name).ToList();

            string key = GlobalKey.CARMODEL_LIST;
            List<CarModelInfo> list = MangaCache.Get(key) as List<CarModelInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetCarModelList().OrderBy(l => l.BrandNameIndex).ThenBy(l => l.SeriesName).ThenBy(l => l.Name).ToList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public void ReloadCarModelListCache()
        {
            string key = GlobalKey.CARMODEL_LIST;
            MangaCache.Remove(key);
            GetCarModelList(true);
        }

        public List<CarModelInfo> GetCarModelListBySeriesID(int sid, bool fromCache = false)
        {
            List<CarModelInfo> list = GetCarModelList(fromCache);
            return list.FindAll(l => l.SeriesID == sid);
        }

        public CarModelInfo GetCarModelModel(int id, bool fromCache = false)
        {
            CarModelInfo entity = null;

            List<CarModelInfo> list = GetCarModelList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetCarModelName(int id)
        {
            CarModelInfo entity = GetCarModelModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public int AddCarModel(CarModelInfo entity)
        {
            return CommonDataProvider.Instance().AddCarModel(entity);
        }

        public void UpdateCarModel(CarModelInfo entity)
        {
            CommonDataProvider.Instance().UpdateCarModel(entity);
        }

        public void DeleteCarModel(string ids)
        {
            CommonDataProvider.Instance().DeleteCarModel(ids);
        }

        #endregion


        #region 数据采集

        private static bool iscollected = false;

        [Serializable]
        private class JsonModel
        {
            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("fletter")]
            public string fletter { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }
        }

        public void CollectData()
        {
            //每个月5号，凌晨3点5分，采集数据
            if (DateTime.Now.Day == 5 && DateTime.Now.Hour == 3 && DateTime.Now.Minute == 5)
            {
                if (!iscollected)
                {
                    iscollected = true;
                    try
                    {
                        EventLogs.JobLog("开始作业-车型数据采集");
                        bool hasnewdata = false;
                        string urlBrand = "http://sales.new4s.com/ajax/brand/1/";
                        string urlSeries = "http://sales.new4s.com/ajax/brand/3/{0}/";
                        string urlModel = "http://sales.new4s.com/ajax/brand/5/{0}/";
                        List<CarBrandInfo> listBrand = GetCarBrandList(true);

                        string strBrand = Http.GetPage(urlBrand, 3);
                        List<JsonModel> listJsonBrand = Serializer.DeserializeJson<List<JsonModel>>(strBrand);
                        if (listBrand == null) listBrand = new List<CarBrandInfo>();
                        foreach (JsonModel jsonbrand in listJsonBrand)
                        {
                            int brandid = 0;
                            string brandname = jsonbrand.name.Replace(jsonbrand.fletter + "-",string.Empty);
                            if (!listBrand.Exists(l => l.Name == brandname))
                            {
                                brandid = AddCarBrand(new CarBrandInfo()
                                {
                                    Name = brandname,
                                    NameIndex = jsonbrand.fletter
                                });
                            }
                            else
                                brandid = listBrand.Find(l => l.Name == brandname).ID;

                            if (brandid > 0)
                            {
                                List<CarSeriesInfo> listSeries = GetCarSeriesListByBrandID(brandid, true);
                                if (listSeries == null) listSeries = new List<CarSeriesInfo>();
                                string strSeries = Http.GetPage(string.Format(urlSeries, jsonbrand.id), 3);
                                List<JsonModel> listJsonSeries = Serializer.DeserializeJson<List<JsonModel>>(strSeries);
                                foreach (JsonModel jsonseries in listJsonSeries)
                                {
                                    int seriesid = 0;
                                    string seriesname = jsonseries.name;
                                    if (!listSeries.Exists(l => l.Name == seriesname))
                                    {
                                        seriesid = AddCarSeries(new CarSeriesInfo()
                                        {
                                            Name = seriesname,
                                            BrandID = brandid
                                        });
                                    }
                                    else
                                        seriesid = listSeries.Find(l => l.Name == seriesname).ID;

                                    if (seriesid > 0)
                                    {
                                        List<CarModelInfo> listModel = GetCarModelListBySeriesID(seriesid, true);
                                        if (listModel == null) listModel = new List<CarModelInfo>();
                                        string strModel = Http.GetPage(string.Format(urlModel, jsonseries.id), 3);
                                        List<JsonModel> listJsonModel = Serializer.DeserializeJson<List<JsonModel>>(strModel);
                                        foreach(JsonModel jsonmodel in listJsonModel)
                                        {
                                            int modelid = 0;
                                            string modelname = jsonmodel.name;
                                            if (!listModel.Exists(l => l.Name == modelname))
                                            {
                                                modelid = AddCarModel(new CarModelInfo()
                                                {
                                                    Name = modelname,
                                                    SeriesID = seriesid
                                                });
                                                hasnewdata = true;
                                            }
                                            else
                                                modelid = listModel.Find(l => l.Name == modelname).ID;

                                            if (modelid == 0)
                                                throw new Exception("新增车型 " + modelname + " 出错");
                                        }
                                    }
                                    else
                                        throw new Exception("新增车系 " + seriesname + " 出错");
                                }
                            }
                            else
                                throw new Exception("新增品牌 " + brandname + " 出错");
                        }

                        if (hasnewdata)
                        {
                            ReloadCarBrandListCache();
                            ReloadCarSeriesListCache();
                            ReloadCarModelListCache();
                        }
                        EventLogs.JobLog("完成作业-车型数据采集");
                    }
                    catch (Exception ex)
                    {
                        EventLogs.JobError("作业发生错误-车型数据采集", EventLogs.EVENTID_JOB_ERROR, 0, ex);
                        ExpLog.Write(ex);
                    }
                }
            }
            else
                iscollected = false;
        }

        #endregion
    }
}
