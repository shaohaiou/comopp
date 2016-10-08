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
    public partial class finality : AdminBase
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
                Response.Redirect("finality.aspx?corpid=" + ddlCorporation.SelectedValue);
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
                if (GetString("active") == "turnover")
                {
                    #region 成交量

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= ((int)CustomerStatus.预订_成交) && l.CustomerStatus <= ((int)CustomerStatus.提车_回访) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
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
                else if (GetString("active") == "turnoverrate")
                {
                    #region 到店成交率

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string arrive = string.Empty; //到店
                        string turnover = string.Empty; //成交
                        string pct = string.Empty; //到店成交率
                        string user = string.Empty;
                        int arrivecount = 0;
                        int turnovercount = 0;
                        int pctcount = 0;

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                int acount = list.FindAll(l => l.CreateUserID == admin.ID && l.IsVisit == 1).Count;
                                arrive += (string.IsNullOrEmpty(arrive) ? string.Empty : ",") + acount.ToString();
                                int tcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + tcount.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (acount == 0 ? "0" : (tcount / acount).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int arriveothercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.IsVisit == 1).Count;
                            int turnoverothercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                            int pctothercount = arriveothercount == 0 ? 0 : (turnoverothercount / arriveothercount);
                            arrive += (string.IsNullOrEmpty(arrive) ? string.Empty : ",") + arriveothercount.ToString();
                            turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + turnoverothercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            string[] groupidlist = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string groupid in groupidlist)
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    int acount = list.FindAll(l => l.CreateUserID == admin.ID && l.IsVisit == 1).Count;
                                    arrive += (string.IsNullOrEmpty(arrive) ? string.Empty : ",") + acount.ToString();
                                    int tcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                    turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + tcount.ToString();

                                    pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (acount == 0 ? "0" : (tcount / acount).ToString());
                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));
                            int arriveothercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.IsVisit == 1).Count;
                            int turnoverothercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                            int pctothercount = arriveothercount == 0 ? 0 : (turnoverothercount / arriveothercount);
                            arrive += (string.IsNullOrEmpty(arrive) ? string.Empty : ",") + arriveothercount.ToString();
                            turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + turnoverothercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                int acount = list.FindAll(l => l.CreateUserID == admin.ID && l.IsVisit == 1).Count;
                                arrive += (string.IsNullOrEmpty(arrive) ? string.Empty : ",") + acount.ToString();
                                int tcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + tcount.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (acount == 0 ? "0" : (tcount / acount).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            arrive += (string.IsNullOrEmpty(arrive) ? string.Empty : ",") + "0";
                            turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + "0";
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + "0";
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        arrivecount = list.FindAll(l => l.IsVisit == 1).Count;
                        turnovercount = list.FindAll(l => l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                        pctcount = arrivecount == 0 ? 0 : (turnovercount / arrivecount);

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":{\"pct\":" + pctcount
                        + ",\"arrive\":" + arrivecount
                        + ",\"turnover\":" + turnovercount
                        + "},\"arrive\":[" + arrive
                        + "],\"turnover\":[" + turnover
                        + "],\"pct\":[" + pct
                        + "],\"user\":[" + user
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "purgerate")
                {
                    #region 意向客户成交率

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string purge = string.Empty; //意向客户
                        string turnover = string.Empty; //成交
                        string pct = string.Empty; //到店成交率
                        string user = string.Empty;
                        int purgecount = 0;
                        int turnovercount = 0;
                        int pctcount = 0;

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                int pcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                                purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + pcount.ToString();
                                int tcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + tcount.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (pcount == 0 ? "0" : (tcount / pcount).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int purgeothercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                            int turnoverothercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                            int pctothercount = purgeothercount == 0 ? 0 : (turnoverothercount / purgeothercount);
                            purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + purgeothercount.ToString();
                            turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + turnoverothercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            string[] groupidlist = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string groupid in groupidlist)
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    int pcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                                    purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + pcount.ToString();
                                    int tcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                    turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + tcount.ToString();

                                    pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (pcount == 0 ? "0" : (tcount / pcount).ToString());
                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));
                            int purgeothercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                            int turnoverothercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                            int pctothercount = purgeothercount == 0 ? 0 : (turnoverothercount / purgeothercount);
                            purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + purgeothercount.ToString();
                            turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + turnoverothercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                int pcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                                purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + pcount.ToString();
                                int tcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + tcount.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (pcount == 0 ? "0" : (tcount / pcount).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + "0";
                            turnover += (string.IsNullOrEmpty(turnover) ? string.Empty : ",") + "0";
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + "0";
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        purgecount = list.FindAll(l => l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                        turnovercount = list.FindAll(l => l.CustomerStatus >= (int)CustomerStatus.预订_成交 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                        pctcount = purgecount == 0 ? 0 : (turnovercount / purgecount);

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":{\"pct\":" + pctcount
                        + ",\"purge\":" + purgecount
                        + ",\"turnover\":" + turnovercount
                        + "},\"purge\":[" + purge
                        + "],\"turnover\":[" + turnover
                        + "],\"pct\":[" + pct
                        + "],\"user\":[" + user
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "defeatrate")
                {
                    #region 战败率

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string purge = string.Empty; //意向客户
                        string failure = string.Empty; //战败数
                        string pct = string.Empty; //到店成交率
                        string user = string.Empty;
                        int purgecount = 0;
                        int failurecount = 0;
                        int pcfcount = 0;

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                int pcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                                purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + pcount.ToString();
                                int fcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.潜客_战败).Count;
                                failure += (string.IsNullOrEmpty(failure) ? string.Empty : ",") + fcount.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (pcount == 0 ? "0" : (fcount / pcount).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int purgeothercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                            int failureothercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus >= (int)CustomerStatus.潜客_战败).Count;
                            int pctothercount = purgeothercount == 0 ? 0 : (failureothercount / purgeothercount);
                            purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + purgeothercount.ToString();
                            failure += (string.IsNullOrEmpty(failure) ? string.Empty : ",") + failureothercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            string[] groupidlist = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string groupid in groupidlist)
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    int pcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                                    purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + pcount.ToString();
                                    int fcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.潜客_战败).Count;
                                    failure += (string.IsNullOrEmpty(failure) ? string.Empty : ",") + fcount.ToString();

                                    pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (pcount == 0 ? "0" : (fcount / pcount).ToString());
                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));
                            int purgeothercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                            int failureothercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.潜客_战败).Count;
                            int pctothercount = purgeothercount == 0 ? 0 : (failureothercount / purgeothercount);
                            purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + purgeothercount.ToString();
                            failure += (string.IsNullOrEmpty(failure) ? string.Empty : ",") + failureothercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                int pcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                                purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + pcount.ToString();
                                int fcount = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.潜客_战败).Count;
                                failure += (string.IsNullOrEmpty(failure) ? string.Empty : ",") + fcount.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (pcount == 0 ? "0" : (fcount / pcount).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            purge += (string.IsNullOrEmpty(purge) ? string.Empty : ",") + "0";
                            failure += (string.IsNullOrEmpty(failure) ? string.Empty : ",") + "0";
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + "0";
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        purgecount = list.FindAll(l => l.CustomerStatus >= (int)CustomerStatus.清洗_邀约).Count;
                        failurecount = list.FindAll(l => l.CustomerStatus >= (int)CustomerStatus.潜客_战败).Count;
                        pcfcount = purgecount == 0 ? 0 : (failurecount / purgecount);

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + "\",\"heji\":{\"pct\":" + pcfcount
                        + ",\"purge\":" + purgecount
                        + ",\"failure\":" + failurecount
                        + "},\"purge\":[" + purge
                        + "],\"failure\":[" + failure
                        + "],\"pct\":[" + pct
                        + "],\"user\":[" + user
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "defeatreason")
                {
                    #region 战败原因

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<GiveupCauseInfo> glist = GiveupCauses.Instance.GetListByCorpid(CurrentCorporationID, true);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (!string.IsNullOrEmpty(uids))
                    {
                        string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => uid.Contains(l.CreateUserID.ToString()));
                    }
                    if (list.Count > 0)
                    {
                        string total = string.Empty;
                        string giveupcause = string.Empty;
                        if (!string.IsNullOrEmpty(groupids))
                        {
                            List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                            listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                            string[] groupid = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            listuser = listuser.FindAll(l=>groupid.Contains(l.PowerGroupID.ToString()));

                            foreach (GiveupCauseInfo ginfo in glist)
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.GiveupCauseID == ginfo.ID && listuser.Exists(u=>u.ID == l.CreateUserID)).Count.ToString();
                                giveupcause += (string.IsNullOrEmpty(giveupcause) ? string.Empty : ",") + string.Format("\"{0}\"", ginfo.Name);
                            }
                        }
                        else
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (GiveupCauseInfo ginfo in glist)
                            {
                                total += (string.IsNullOrEmpty(total) ? string.Empty : ",") + list.FindAll(l => l.GiveupCauseID == ginfo.ID).Count.ToString();
                                giveupcause += (string.IsNullOrEmpty(giveupcause) ? string.Empty : ",") + string.Format("\"{0}\"", ginfo.Name);
                            }
                        }

                        result = "{\"starttime\":\"" + starttime
                        + "\",\"endtime\":\"" + endtime
                        + ",\"total\":[" + total
                        + "],\"giveupcause\":[" + giveupcause
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