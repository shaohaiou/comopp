using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ComOpp.Components;

namespace ComOpp.BackAdmin
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码

            try
            {
                Start();
            }
            catch { }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码

        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }


        private void Start()
        {
            Jobs.Instance().Start();                        //任务启动
#if DEBUG
            EventLogs.WebLog("网站启动");//写入系统日志信息
#endif

            LoadCache();
        }

        private void LoadCache()
        {
            try
            {
                #region 加载地区缓存

                Districts.Instance.ReloadProvinceListCache();
                Districts.Instance.ReloadCityListCache();
                Districts.Instance.ReloadDistrictListCache();

                #endregion

                #region 加载车型缓存
                
                Cars.Instance.ReloadCarBrandListCache();
                Cars.Instance.ReloadCarSeriesListCache();
                Cars.Instance.ReloadCarModelListCache();

                #endregion

                #region 加载系统设置缓存
                
                Corporations.Instance.ReloadCorporationListCache();
                ComOpp.Components.Modules.Instance.ReloadModuleListCache();
                PowerGroups.Instance.ReloadPowerGroupListCache();

                #endregion

                #region 加载基础设置缓存

                CustomerLevels.Instance.ReloadCustomerLevelListCache();
                InfoSources.Instance.ReloadInfoSourceListCache();
                InfoTypes.Instance.ReloadInfoTypeListCache();
                ConnectWays.Instance.ReloadConnectWayListCache();
                GiveupCauses.Instance.ReloadGiveupCauseListCache();
                PaymentWays.Instance.ReloadPaymentWayListCache();
                Ibuytimes.Instance.ReloadIbuytimeListCache();
                Tracktags.Instance.ReloadTracktagListCache();

                #endregion

                #region 加载用户缓存

                PowerGroups.Instance.ReloadPowerGroupListCache();

                #endregion

                #region 公告通知缓存加载

                Notices.Instance.ReloadNoticeListCache();

                #endregion

                #region 商机缓存加载

                List<CorporationInfo> corplist = Corporations.Instance.GetList(true);
                foreach (CorporationInfo corpinfo in corplist)
                {
                    Customers.Instance.GetCustomerListByCorporation(corpinfo.ID,true);
                }

                #endregion
            }
            catch { }
        }
    }
}
