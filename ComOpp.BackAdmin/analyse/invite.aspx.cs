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
    public partial class invite : AdminBase
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
                Response.Redirect("invite.aspx?corpid=" + ddlCorporation.SelectedValue);
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
                if (GetString("active") == "arrive")
                {
                    #region 邀约到店量

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= ((int)CustomerStatus.到店_洽谈) && l.CustomerStatus <= ((int)CustomerStatus.提车_回访) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
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

                        result = "{\"heji\":" + list.Count
                        + ",\"total\":[" + total
                        + "],\"user\":[" + user
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "structure")
                {
                    #region 分时邀约结构比达成率

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => l.IsVisit == 1 && listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string week67 = string.Empty; //周末
                        string week15 = string.Empty; //工作日
                        string pct = string.Empty; //达成率
                        string user = string.Empty;
                        int week67count = 0;
                        int week15count = 0;
                        int pctcount = 0;

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                int w67count = list.FindAll(l => l.CreateUserID == admin.ID && (DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Sunday || DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Saturday)).Count;
                                week67 += (string.IsNullOrEmpty(week67) ? string.Empty : ",") + w67count.ToString();
                                int w15count = list.FindAll(l => l.CreateUserID == admin.ID && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Sunday && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Saturday).Count;
                                week15 += (string.IsNullOrEmpty(week15) ? string.Empty : ",") + w15count.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (w15count == 0 ? "0" : (w67count / w15count).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int w67othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && (DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Sunday || DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Saturday)).Count;
                            int w15othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Sunday && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Saturday).Count;
                            int pctothercount = w15othercount == 0 ? 0 : (w67othercount / w15othercount);
                            week67 += (string.IsNullOrEmpty(week67) ? string.Empty : ",") + w67othercount.ToString();
                            week15 += (string.IsNullOrEmpty(week15) ? string.Empty : ",") + w15othercount.ToString();
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
                                    int w67count = list.FindAll(l => l.CreateUserID == admin.ID && (DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Sunday || DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Saturday)).Count;
                                    week67 += (string.IsNullOrEmpty(week67) ? string.Empty : ",") + w67count.ToString();
                                    int w15count = list.FindAll(l => l.CreateUserID == admin.ID && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Sunday && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Saturday).Count;
                                    week15 += (string.IsNullOrEmpty(week15) ? string.Empty : ",") + w15count.ToString();

                                    pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (w15count == 0 ? "0" : (w67count / w15count).ToString());
                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));
                            int w67othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && (DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Sunday || DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Saturday)).Count;
                            int w15othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Sunday && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Saturday).Count;
                            int pctothercount = w15othercount == 0 ? 0 : (w67othercount / w15othercount);
                            week67 += (string.IsNullOrEmpty(week67) ? string.Empty : ",") + w67othercount.ToString();
                            week15 += (string.IsNullOrEmpty(week15) ? string.Empty : ",") + w15othercount.ToString();
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + pctothercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                int w67count = list.FindAll(l => l.CreateUserID == admin.ID && (DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Sunday || DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Saturday)).Count;
                                week67 += (string.IsNullOrEmpty(week67) ? string.Empty : ",") + w67count.ToString();
                                int w15count = list.FindAll(l => l.CreateUserID == admin.ID && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Sunday && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Saturday).Count;
                                week15 += (string.IsNullOrEmpty(week15) ? string.Empty : ",") + w15count.ToString();

                                pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + (w15count == 0 ? "0" : (w67count / w15count).ToString());
                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            week67 += (string.IsNullOrEmpty(week67) ? string.Empty : ",") + "0";
                            week15 += (string.IsNullOrEmpty(week15) ? string.Empty : ",") + "0";
                            pct += (string.IsNullOrEmpty(pct) ? string.Empty : ",") + "0";
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        week67count = list.FindAll(l => DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Sunday || DataConvert.SafeDate(l.VisitTime).DayOfWeek == DayOfWeek.Saturday).Count;
                        week15count = list.FindAll(l => DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Sunday && DataConvert.SafeDate(l.VisitTime).DayOfWeek != DayOfWeek.Saturday).Count;
                        pctcount = week15count == 0 ? 0 : (week67count / week15count);

                        result = "{\"heji\":{\"pct\":" + pctcount
                        + ",\"week15\":" + week15count
                        + ",\"week67\":" + week67count
                        + "},\"week67\":[" + week67
                        + "],\"week15\":[" + week15
                        + "],\"pct\":[" + pct
                        + "],\"user\":[" + user
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