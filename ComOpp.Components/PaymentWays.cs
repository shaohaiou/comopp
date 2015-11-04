using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class PaymentWays : BllBase
    {
        #region 单例

        private static object sync_creater = new object();

        private static PaymentWays _instance = null;
        public static PaymentWays Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync_creater)
                    {
                        if (_instance == null)
                            _instance = new PaymentWays("ComOpp_PaymentWay");
                    }
                }
                return _instance;
            }
        }

        private PaymentWays(string tablename)
            : base(tablename)
        { }

        #endregion

        public List<PaymentWayInfo> GetList(bool fromCache = false)
        {
            if (!fromCache)
                return CommonDataProvider.Instance().GetPaymentWayList();

            string key = GlobalKey.PAYMENTWAY_LIST;
            List<PaymentWayInfo> list = MangaCache.Get(key) as List<PaymentWayInfo>;
            if (list == null)
            {
                list = CommonDataProvider.Instance().GetPaymentWayList();
                MangaCache.Max(key, list);
            }
            return list;
        }

        public List<PaymentWayInfo> GetListByCorpid(int corpid, bool fromCache = false)
        {
            List<PaymentWayInfo> list = GetList(fromCache);
            List<PaymentWayInfo> listcorp = list.FindAll(l => l.CorporationID == corpid || l.DataLevel == 0);

            return listcorp.OrderBy(l => l.DataLevel).ThenBy(l => l.Sort).ToList();
        }

        public void ReloadPaymentWayListCache()
        {
            string key = GlobalKey.PAYMENTWAY_LIST;
            MangaCache.Remove(key);
            GetList(true);
        }

        public PaymentWayInfo GetModel(int id, bool fromCache = false)
        {
            PaymentWayInfo entity = null;

            List<PaymentWayInfo> list = GetList(fromCache);
            entity = list.Find(c => c.ID == id);

            return entity;
        }

        public string GetName(int id)
        {
            PaymentWayInfo entity = GetModel(id, true);
            if (entity == null) return string.Empty;
            return entity.Name;
        }

        public void Add(PaymentWayInfo entity)
        {
            CommonDataProvider.Instance().AddPaymentWay(entity);
        }

        public void Update(PaymentWayInfo entity)
        {
            CommonDataProvider.Instance().UpdatePaymentWay(entity);
        }

        public void Delete(string ids)
        {
            CommonDataProvider.Instance().DeletePaymentWay(ids);
        }
    }
}
