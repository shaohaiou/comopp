using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Text.RegularExpressions;

namespace ComOpp.BackAdmin.analyse
{
    public partial class archive : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "报表分析"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        private int CurrentCorporationID
        {
            get
            {
                return Admin.Administrator ? (GetInt("corpid") > 0 ? GetInt("corpid") : DataConvert.SafeInt(ddlCorporation.SelectedValue)) : Admin.CorporationID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
                LoadData();
            }
            else
            {
                Response.Redirect("archive.aspx?corpid=" + ddlCorporation.SelectedValue);
            }
        }

        private void BindControler()
        {
            ddlCorporation.DataSource = Corporations.Instance.GetList(true);
            ddlCorporation.DataTextField = "Name";
            ddlCorporation.DataValueField = "ID";
            ddlCorporation.DataBind();

            if (GetInt("corpid") > 0)
                SetSelectedByValue(ddlCorporation, GetString("corpid"));

            List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
            listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
            listuser = listuser.OrderBy(l => (int)l.UserRole).ThenBy(l => l.UserName).ToList();
            rptUser.DataSource = listuser;
            rptUser.DataBind();

            List<PowerGroupInfo> plist = PowerGroups.Instance.GetList(true);
            plist = plist.FindAll(l => l.CorporationID == CurrentCorporationID);
            rptGroup.DataSource = plist;
            rptGroup.DataBind();

            CorporationInfo corp = Corporations.Instance.GetModel(CurrentCorporationID, true);
            rptSeries.DataSource = Cars.Instance.GetCarSeriesListByBrandID(corp.BrandID);
            rptSeries.DataBind();

            rptInfoSource.DataSource = InfoSources.Instance.GetListByCorpid(CurrentCorporationID, true);
            rptInfoSource.DataBind();
        }

        private void LoadData()
        {
            if (GetInt("json") == 1)
            {
                Regex rtime = new Regex(@"[- :]");
                float starttimevalue = 0;
                float endtimevalue = 0;
                string starttime = GetString("starttime");
                string endtime = GetString("endtime");
                if (!string.IsNullOrEmpty(starttime))
                    starttimevalue = DataConvert.SafeFloat(DataConvert.SafeDate(starttime).ToString("yyyyMMddHHmm"));
                if (!string.IsNullOrEmpty(endtime))
                    endtimevalue = DataConvert.SafeFloat(DataConvert.SafeDate(endtime).AddDays(1).ToString("yyyyMMddHHmm"));
                string result = "{\"0\":false,\"unit\":\"%\"}";
                if (GetString("active") == "track" || string.IsNullOrEmpty(GetString("active")))
                {
                    #region 线索新增量

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (!string.IsNullOrEmpty(uids))
                    {
                        string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => uid.Contains(l.CreateUserID.ToString()));
                    }
                    if (list.Count > 0)
                    {
                        string total = string.Empty;
                        string user = string.Empty;
                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.CreateUserID == admin.ID).Count.ToString();
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            foreach (string groupid in groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.CreateUserID == admin.ID).Count.ToString();
                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.CreateUserID == admin.ID).Count.ToString();
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                        }


                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":" + list.Count
                        + ",\"total\":[" + total
                        + "],\"user\":[" + user
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "brandseries")
                {
                    #region 线索拟购车系

                    string series = GetString("series[]");
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID,true);
                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (!string.IsNullOrEmpty(series))
                    {
                        string[] serie = series.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => series.Contains(l.IbuyCarSeriesID.ToString()));
                    }
                    if (list.Count > 0)
                    {
                        CorporationInfo corp = Corporations.Instance.GetModel(CurrentCorporationID, true);
                        List<CarSeriesInfo> slist = Cars.Instance.GetCarSeriesListByBrandID(corp.BrandID, true);

                        string total = string.Empty;
                        string seriesstr = string.Empty;
                        if (!string.IsNullOrEmpty(series))
                        {
                            string[] serie = series.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (CarSeriesInfo sinfo in slist.FindAll(s => serie.Contains(s.ID.ToString())))
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.IbuyCarSeriesID == sinfo.ID).Count.ToString();
                                seriesstr += (string.IsNullOrEmpty(seriesstr) ? string.Empty : ",") + string.Format("\"{0}\"", sinfo.Name);
                            }
                        }
                        else
                        {
                            foreach (CarSeriesInfo sinfo in slist)
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.IbuyCarSeriesID == sinfo.ID).Count.ToString();
                                seriesstr += (string.IsNullOrEmpty(seriesstr) ? string.Empty : ",") + string.Format("\"{0}\"", sinfo.Name);
                            }
                        }

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":" + list.Count
                        + ",\"total\":[" + total
                        + "],\"series\":[" + seriesstr
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "purge")
                {
                    #region 意向客户新增量

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约 && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (!string.IsNullOrEmpty(uids))
                    {
                        string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => uid.Contains(l.CreateUserID.ToString()));
                    }
                    if (list.Count > 0)
                    {
                        string total = string.Empty;
                        string user = string.Empty;
                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.CreateUserID == admin.ID).Count.ToString();
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            foreach (string groupid in groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.CreateUserID == admin.ID).Count.ToString();
                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.CreateUserID == admin.ID).Count.ToString();
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                        }

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":" + list.Count
                        + ",\"total\":[" + total
                        + "],\"user\":[" + user
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "infosource")
                {
                    #region 线索信息来源

                    string infosources = GetString("infosource[]");
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (!string.IsNullOrEmpty(infosources))
                    {
                        string[] infosource = infosources.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => infosource.Contains(l.InfoSourceID.ToString()));
                    }
                    if (list.Count > 0)
                    {
                        List<InfoSourceInfo> islist = InfoSources.Instance.GetListByCorpid(CurrentCorporationID, true);

                        string total = string.Empty;
                        string infosource = string.Empty;
                        if (!string.IsNullOrEmpty(infosources))
                        {
                            string[] infosourcelist = infosources.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (InfoSourceInfo sinfo in islist.FindAll(s => infosourcelist.Contains(s.ID.ToString())))
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.InfoSourceID == sinfo.ID).Count.ToString();
                                infosource += (string.IsNullOrEmpty(infosource) ? string.Empty : ",") + string.Format("\"{0}\"", sinfo.Name);
                            }
                        }
                        else
                        {
                            foreach (InfoSourceInfo sinfo in islist)
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.InfoSourceID == sinfo.ID).Count.ToString();
                                infosource += (string.IsNullOrEmpty(infosource) ? string.Empty : ",") + string.Format("\"{0}\"", sinfo.Name);
                            }
                        }

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":" + list.Count
                        + ",\"total\":[" + total
                        + "],\"infosource\":[" + infosource
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
            }
        }
    }
}