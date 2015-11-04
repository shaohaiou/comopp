using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Text.RegularExpressions;

namespace ComOpp.BackAdmin.chance
{
    public partial class chancelist : AdminBase
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
                int page = GetInt("page", 1);
                int rows = GetInt("rows", 100);
                int corpid = GetInt("corpid", 0);
                int state = GetInt("state", 0);
                int issearch = GetInt("issearch", 0);
                int lurkstate = GetInt("lurkstate", 0);
                int checkstate = GetInt("checkstate", 0);
                string action = GetString("action");

                string customername = GetString("uname");
                string customerphone = GetString("phone");
                string weixinaccount = GetString("weixin");
                string ordernum = GetString("ordernum");
                int ibuyseriesid = GetInt("series");
                int customerlevelid = GetInt("customerlevel");
                int infotypeid = GetInt("infotype");
                int infosourceid = GetInt("infosource");
                int connectwayid = GetInt("connectway");
                int giveupcauseid = GetInt("giveupcause");
                int paymentwayid = GetInt("paymentway");
                int ibuytime = GetInt("ibuytime");
                int tracktagid = GetInt("tracktag");
                int followalarm = GetInt("followalarm");
                string isvisit = GetString("arrive");
                int sbuyseriesid = GetInt("iseries");
                int owneruid = GetInt("owneruid");
                int mpuid = GetInt("mpuid");
                int dccuid = GetInt("dccuid");
                int exuid = GetInt("exuid");
                int dsuid = GetInt("dsuid");
                int archivemove = GetInt("archivemove");
                string sotime = GetString("sotime");
                string starttime = GetString("starttime");
                string endtime = GetString("endtime");

                List<CustomerInfo> list;
                if (Corporation != null && Corporation.ID != corpid)
                    list = new List<CustomerInfo>();
                else
                {
                    list = Customers.Instance.GetCustomerListByCorporation(corpid, true);
                    if (issearch == 0)
                    {
                        list = list.FindAll(l => l.LurkStatus == lurkstate);
                        list = list.FindAll(l => l.CheckStatus == checkstate);
                    }
                    if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                    {
                        if (CurrentPowerGroup != null && !string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds))
                        {
                            string[] powers = CurrentPowerGroup.CanviewGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            list = list.FindAll(l => l.OwnerPowerGroupID == Admin.PowerGroupID || powers.Contains(l.OwnerPowerGroupID.ToString()));
                        }
                        else
                            list = list.FindAll(l => l.OwnerID == AdminID);
                    }
                    if (state > 0)
                        list = list.FindAll(l => l.CustomerStatus == state);
                    if (!string.IsNullOrEmpty(action))
                    {
                        if (action == "intent")
                            list = list.FindAll(l=>l.CustomerStatusSource == (int)CustomerStatus.追踪_促成);
                        else if(action == "potential")
                            list = list.FindAll(l => l.CustomerStatusSource == (int)CustomerStatus.清洗_邀约 || l.CustomerStatusSource == (int)CustomerStatus.导入_集客);
                    }
                    if (!string.IsNullOrEmpty(customername))
                        list = list.FindAll(l => l.Name == customername);
                    if (!string.IsNullOrEmpty(customerphone))
                        list = list.FindAll(l => l.Phone == customerphone);
                    if (!string.IsNullOrEmpty(weixinaccount))
                        list = list.FindAll(l => l.WeixinAccount == weixinaccount);
                    if (!string.IsNullOrEmpty(ordernum))
                        list = list.FindAll(l => l.OrderNumber == ordernum);
                    if(ibuyseriesid > 0)
                        list = list.FindAll(l => l.IbuyCarSeriesID == ibuyseriesid);
                    if (customerlevelid > 0)
                        list = list.FindAll(l => l.LastCustomerLevelID == customerlevelid);
                    if (infotypeid > 0)
                        list = list.FindAll(l => l.InfoTypeID == infotypeid);
                    if (infosourceid > 0)
                        list = list.FindAll(l => l.InfoSourceID == infosourceid);
                    if (connectwayid > 0)
                        list = list.FindAll(l => l.LastConnectwayID == connectwayid);
                    if (giveupcauseid > 0)
                        list = list.FindAll(l => l.GiveupCauseID == giveupcauseid);
                    if (paymentwayid > 0)
                        list = list.FindAll(l => l.PaymentWayID == paymentwayid);
                    if (ibuytime > 0)
                        list = list.FindAll(l => l.IbuyTimeID == ibuytime);
                    if (tracktagid > 0)
                        list = list.FindAll(l => l.TracktagID.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries).Contains(tracktagid.ToString()));
                    if (followalarm > 0)
                        list = list.FindAll(l => l.ConnectAlarm == followalarm.ToString());
                    if (!string.IsNullOrEmpty(isvisit))
                        list = list.FindAll(l => l.IsVisit.ToString() == isvisit);
                    if (sbuyseriesid > 0)
                        list = list.FindAll(l => l.SbuyCarSeriesID == sbuyseriesid);
                    if (owneruid > 0)
                        list = list.FindAll(l => l.OwnerID == owneruid);
                    if (mpuid > 0)
                        list = list.FindAll(l => l.MarketDirectorID == mpuid);
                    if (dccuid > 0)
                        list = list.FindAll(l => l.DCCDirectorID == dccuid);
                    if (exuid > 0)
                        list = list.FindAll(l => l.ExhibitionDirectorID == exuid);
                    if (dsuid > 0)
                        list = list.FindAll(l => l.DirectorID == dsuid);
                    if (!string.IsNullOrEmpty(sotime))
                    {
                        Regex rtime = new Regex(@"[- :]");
                        float starttimevalue = 0;
                        float endtimevalue = 0;
                        if (!string.IsNullOrEmpty(starttime))
                            starttimevalue = DataConvert.SafeFloat(DataConvert.SafeDate(starttime).ToString("yyyyMMddHHmm"));
                        if (!string.IsNullOrEmpty(endtime))
                            endtimevalue = DataConvert.SafeFloat(DataConvert.SafeDate(endtime).ToString("yyyyMMddHHmm"));
                        switch (sotime)
                        {
                            case "posttime":
                                if (starttimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.PostTime, string.Empty)) >= starttimevalue);
                                if (endtimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.PostTime, string.Empty)) <= endtimevalue);
                                break;
                            case "invitetime":
                                if (starttimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.ReservationTime, string.Empty)) >= starttimevalue);
                                if (endtimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.ReservationTime, string.Empty)) <= endtimevalue);
                                break;
                            case "dateline":
                                if (starttimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue);
                                if (endtimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                                break;
                            case "arrivetime":
                                if (starttimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.VisitTime, string.Empty)) >= starttimevalue);
                                if (endtimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.VisitTime, string.Empty)) <= endtimevalue);
                                break;
                            case "endtime":
                                if (starttimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.PlaceOrderTime, string.Empty)) >= starttimevalue);
                                if (endtimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.PlaceOrderTime, string.Empty)) <= endtimevalue);
                                break;
                            case "delivertime":
                                if (starttimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.PicupcarTime, string.Empty)) >= starttimevalue);
                                if (endtimevalue > 0)
                                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.PicupcarTime, string.Empty)) <= endtimevalue);
                                break;
                            default:
                                break;
                        }
                            
                    }
                    if (archivemove > 0)
                    {
                        if (state > 0 && state != (int)CustomerStatus.导入_集客 && state != (int)CustomerStatus.清洗_邀约 && state != (int)CustomerStatus.追踪_促成)
                            list = new List<CustomerInfo>();
                        else 
                        {
                            List<CustomerInfo> listresult = new List<CustomerInfo>();
                            CorporationInfo corporation = Corporations.Instance.GetModel(corpid,true);
                            if (corporation != null)
                            {
                                if (archivemove == 1)//主动转出
                                {
                                    if (corporation.Trackmove == 1)
                                        listresult.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.导入_集客));
                                    if (corporation.Voluntaryoffday > 0)
                                        listresult.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corporation.Voluntaryoffday - 1)));
                                    if (corporation.Voluntaryoutday > 0)
                                        listresult.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corporation.Voluntaryoutday - 1)));
                                }
                                else //7天内强制转出
                                {
                                    if (corporation.Forcedoffday > 0)
                                    {
                                        string[] offcustomerlevel = corporation.Offcustomerlevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        listresult.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corporation.Forcedoffday - 1) && !offcustomerlevel.Contains(l.LastCustomerLevelID.ToString())));
                                    }
                                    if (corporation.Forcedoutday > 0)
                                    {

                                        string[] forcedoutdaylevel = corporation.Forcedoutdaylevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        listresult.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corporation.Forcedoutday - 1) && !forcedoutdaylevel.Contains(l.LastCustomerLevelID.ToString())));
                                    }
                                }
                            }
                            list = listresult;
                        }
                    }
                }

                int total = list.Count;
                int maxpage = list.Count / rows + (list.Count % rows == 0 ? 0 : 1);

                list = list.Skip((page - 1) * rows).Take(rows).ToList<CustomerInfo>();

                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}