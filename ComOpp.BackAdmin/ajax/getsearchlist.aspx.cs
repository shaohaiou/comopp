using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.ajax
{
    public partial class getsearchlist : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int corpid = GetInt("corpid", 0);
                string action = GetString("action");

                if (action == "userlist")
                {
                    List<AdminInfo> list = Admins.Instance.GetAllAdmins();
                    list = list.FindAll(l => l.CorporationID == corpid);
                    if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                    {
                        if (CurrentPowerGroup != null && !string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds))
                        {
                            string[] powers = CurrentPowerGroup.CanviewGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            list = list.FindAll(l => l.ID == AdminID || powers.Contains(l.PowerGroupID.ToString()));
                        }
                        else
                            list = list.FindAll(l => l.ID == AdminID);
                    }

                    list = list.OrderBy(l => (int)l.UserRole).ThenBy(l => l.UserName).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "connectway")
                {
                    List<ConnectWayInfo> list = ConnectWays.Instance.GetListByCorpid(corpid, true);
                    list = list.OrderBy(l => l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "customerlevel")
                {
                    List<CustomerLevelInfo> list = CustomerLevels.Instance.GetListByCorpid(corpid, true);
                    list = list.OrderBy(l => l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "series" || action == "iseries")
                {
                    CorporationInfo corporation = Corporations.Instance.GetModel(corpid);
                    if (corporation != null)
                    {
                        List<CarSeriesInfo> list = Cars.Instance.GetCarSeriesListByBrandID(corporation.BrandID);
                        list = list.OrderBy(l => l.Name).ToList();

                        Response.Write(Serializer.SerializeJson(list));
                    }
                }
                else if (action == "infotype")
                {
                    List<InfoTypeInfo> list = InfoTypes.Instance.GetListByCorpid(corpid);
                    list = list.OrderBy(l=>l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "infosource")
                {
                    List<InfoSourceInfo> list = InfoSources.Instance.GetListByCorpid(corpid);
                    list = list.OrderBy(l=>l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "giveupcause")
                {
                    List<GiveupCauseInfo> list = GiveupCauses.Instance.GetListByCorpid(corpid);
                    list = list.OrderBy(l=>l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "paymentway")
                {
                    List<PaymentWayInfo> list = PaymentWays.Instance.GetListByCorpid(corpid);
                    list = list.OrderBy(l=>l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "ibuytime")
                {
                    List<IbuytimeInfo> list = Ibuytimes.Instance.GetListByCorpid(corpid);
                    list = list.OrderBy(l=>l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "tracktag")
                {
                    List<TracktagInfo> list = Tracktags.Instance.GetListByCorpid(corpid);
                    list = list.OrderBy(l=>l.Sort).ToList();

                    Response.Write(Serializer.SerializeJson(list));
                }
                else if (action == "followalarm")
                {
                    Response.Write("[{\"id\":\"0\",\"name\":\"正常\"},{\"id\":\"1\",\"name\":\"正常(24小时内超时)\"},{\"id\":\"2\",\"name\":\"追踪超时\"}]");
                }
                else if (action == "arrive")
                {
                    Response.Write("[{\"id\":\"0\",\"name\":\"未到店\"},{\"id\":\"1\",\"name\":\"已到店\"}]");
                }
                else if (action == "state")
                {
                    string datalist = GetString("datalist");
                    if (datalist == "list")
                    {
                        Response.Write("[{\"id\":\"" + (int)CustomerStatus.导入_集客 + "\",\"name\":\"导入|集客\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.清洗_邀约 + "\",\"name\":\"清洗|邀约\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.到店_洽谈 + "\",\"name\":\"到店|洽谈\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.追踪_促成 + "\",\"name\":\"追踪|促成\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.预订_成交 + "\",\"name\":\"预订|成交\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.提车_回访 + "\",\"name\":\"提车|回访\"}"
                            + "]");
                    }
                    else if (datalist == "potential")
                    {
                        Response.Write("[{\"id\":\"" + (int)CustomerStatus.潜客_转出 + "\",\"name\":\"转出\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.潜客_战败 + "\",\"name\":\"战败\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.其他潜客 + "\",\"name\":\"其他潜客\"}"
                            + "]");
                    }
                    else
                    {
                        Response.Write("[{\"id\":\"" + (int)CustomerStatus.导入_集客 + "\",\"name\":\"导入|集客\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.清洗_邀约 + "\",\"name\":\"清洗|邀约\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.到店_洽谈 + "\",\"name\":\"到店|洽谈\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.追踪_促成 + "\",\"name\":\"追踪|促成\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.预订_成交 + "\",\"name\":\"预订|成交\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.提车_回访 + "\",\"name\":\"提车|回访\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.潜客_转出 + "\",\"name\":\"转出\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.潜客_战败 + "\",\"name\":\"战败\"}"
                            + ",{\"id\":\"" + (int)CustomerStatus.其他潜客 + "\",\"name\":\"其他潜客\"}"
                            + "]");
                    }
                }
                else if (action == "archivemove")
                {
                    Response.Write("[{\"id\":\"1\",\"name\":\"可主动转出\"},{\"id\":\"2\",\"name\":\"7日内系统将强制转出\"}]");
                }
            }
        }
    }
}