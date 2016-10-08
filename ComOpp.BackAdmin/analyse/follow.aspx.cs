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
    public partial class follow : AdminBase
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
                Response.Redirect("follow.aspx?corpid=" + ddlCorporation.SelectedValue);
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
                if (GetString("active") == "state")
                {
                    #region 客户状态

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string layer = "\"合计\",\"导入|集客\",\"清洗|邀约\",\"到店|洽谈\",\"追踪|促成\",\"预订|成交\",\"提车|回访\""; //客户状态
                        string ilayer = "\"合计\",\"导入|集客\",\"清洗|邀约\",\"到店|洽谈\",\"追踪|促成\",\"预订|成交\",\"提车|回访\""; //客户状态
                        string layer0 = string.Empty; //合计
                        string layer1 = string.Empty; //状态 导入|集客
                        string layer2 = string.Empty;
                        string layer3 = string.Empty;
                        string layer4 = string.Empty;
                        string layer5 = string.Empty;
                        string layer6 = string.Empty;
                        string user = string.Empty;
                        int layer0count = 0;
                        int layer1count = 0;
                        int layer2count = 0;
                        int layer3count = 0;
                        int layer4count = 0;
                        int layer5count = 0;
                        int layer6count = 0;

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.导入_集客 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();
                                int y1 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.导入_集客).Count;
                                layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1.ToString();
                                int y2 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.清洗_邀约).Count;
                                layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2.ToString();
                                int y3 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.到店_洽谈).Count;
                                layer3 += (string.IsNullOrEmpty(layer3) ? string.Empty : ",") + y3.ToString();
                                int y4 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.追踪_促成).Count;
                                layer4 += (string.IsNullOrEmpty(layer4) ? string.Empty : ",") + y4.ToString();
                                int y5 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.预订_成交).Count;
                                layer5 += (string.IsNullOrEmpty(layer5) ? string.Empty : ",") + y5.ToString();
                                int y6 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.提车_回访).Count;
                                layer6 += (string.IsNullOrEmpty(layer6) ? string.Empty : ",") + y6.ToString();

                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int y0othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus >= (int)CustomerStatus.导入_集客 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                            int y1othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus == (int)CustomerStatus.导入_集客).Count;
                            int y2othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus == (int)CustomerStatus.清洗_邀约).Count;
                            int y3othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus == (int)CustomerStatus.到店_洽谈).Count;
                            int y4othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus == (int)CustomerStatus.追踪_促成).Count;
                            int y5othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus == (int)CustomerStatus.预订_成交).Count;
                            int y6othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.CustomerStatus == (int)CustomerStatus.提车_回访).Count;
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0othercount.ToString();
                            layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1othercount.ToString();
                            layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2othercount.ToString();
                            layer3 += (string.IsNullOrEmpty(layer3) ? string.Empty : ",") + y3othercount.ToString();
                            layer4 += (string.IsNullOrEmpty(layer4) ? string.Empty : ",") + y4othercount.ToString();
                            layer5 += (string.IsNullOrEmpty(layer5) ? string.Empty : ",") + y5othercount.ToString();
                            layer6 += (string.IsNullOrEmpty(layer6) ? string.Empty : ",") + y6othercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            string[] groupidlist = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string groupid in groupidlist)
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.导入_集客 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                    layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();
                                    int y1 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.导入_集客).Count;
                                    layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1.ToString();
                                    int y2 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.清洗_邀约).Count;
                                    layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2.ToString();
                                    int y3 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.到店_洽谈).Count;
                                    layer3 += (string.IsNullOrEmpty(layer3) ? string.Empty : ",") + y3.ToString();
                                    int y4 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.追踪_促成).Count;
                                    layer4 += (string.IsNullOrEmpty(layer4) ? string.Empty : ",") + y4.ToString();
                                    int y5 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.预订_成交).Count;
                                    layer5 += (string.IsNullOrEmpty(layer5) ? string.Empty : ",") + y5.ToString();
                                    int y6 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.提车_回访).Count;
                                    layer6 += (string.IsNullOrEmpty(layer6) ? string.Empty : ",") + y6.ToString();

                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));

                            int y0othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus >= (int)CustomerStatus.导入_集客 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                            int y1othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus == (int)CustomerStatus.导入_集客).Count;
                            int y2othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus == (int)CustomerStatus.清洗_邀约).Count;
                            int y3othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus == (int)CustomerStatus.到店_洽谈).Count;
                            int y4othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus == (int)CustomerStatus.追踪_促成).Count;
                            int y5othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus == (int)CustomerStatus.预订_成交).Count;
                            int y6othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.CustomerStatus == (int)CustomerStatus.提车_回访).Count;
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0othercount.ToString();
                            layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1othercount.ToString();
                            layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2othercount.ToString();
                            layer3 += (string.IsNullOrEmpty(layer3) ? string.Empty : ",") + y3othercount.ToString();
                            layer4 += (string.IsNullOrEmpty(layer4) ? string.Empty : ",") + y4othercount.ToString();
                            layer5 += (string.IsNullOrEmpty(layer5) ? string.Empty : ",") + y5othercount.ToString();
                            layer6 += (string.IsNullOrEmpty(layer6) ? string.Empty : ",") + y6othercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus >= (int)CustomerStatus.导入_集客 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                                layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();
                                int y1 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.导入_集客).Count;
                                layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1.ToString();
                                int y2 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.清洗_邀约).Count;
                                layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2.ToString();
                                int y3 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.到店_洽谈).Count;
                                layer3 += (string.IsNullOrEmpty(layer3) ? string.Empty : ",") + y3.ToString();
                                int y4 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.追踪_促成).Count;
                                layer4 += (string.IsNullOrEmpty(layer4) ? string.Empty : ",") + y4.ToString();
                                int y5 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.预订_成交).Count;
                                layer5 += (string.IsNullOrEmpty(layer5) ? string.Empty : ",") + y5.ToString();
                                int y6 = list.FindAll(l => l.CreateUserID == admin.ID && l.CustomerStatus == (int)CustomerStatus.提车_回访).Count;
                                layer6 += (string.IsNullOrEmpty(layer6) ? string.Empty : ",") + y6.ToString();

                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + "0";
                            layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + "0";
                            layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + "0";
                            layer3 += (string.IsNullOrEmpty(layer3) ? string.Empty : ",") + "0";
                            layer4 += (string.IsNullOrEmpty(layer4) ? string.Empty : ",") + "0";
                            layer5 += (string.IsNullOrEmpty(layer5) ? string.Empty : ",") + "0";
                            layer6 += (string.IsNullOrEmpty(layer6) ? string.Empty : ",") + "0";
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        layer0count = list.FindAll(l => l.CustomerStatus >= (int)CustomerStatus.导入_集客 && l.CustomerStatus <= (int)CustomerStatus.提车_回访).Count;
                        layer1count = list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.导入_集客).Count;
                        layer2count = list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约).Count;
                        layer3count = list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.到店_洽谈).Count;
                        layer4count = list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成).Count;
                        layer5count = list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.预订_成交).Count;
                        layer6count = list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.提车_回访).Count;

                        result = "{\"heji\":{\"layer:0\":" + layer0count
                        + ",\"layer:1\":" + layer1count
                        + ",\"layer:2\":" + layer2count
                        + ",\"layer:3\":" + layer3count
                        + ",\"layer:4\":" + layer4count
                        + ",\"layer:5\":" + layer5count
                        + ",\"layer:6\":" + layer6count
                        + "},\"layer\":[" + layer
                        + "],\"user\":[" + user
                        + "],\"ilayer\":[" + ilayer
                        + "],\"layer:0\":[" + layer0
                        + "],\"layer:1\":[" + layer1
                        + "],\"layer:2\":[" + layer2
                        + "],\"layer:3\":[" + layer3
                        + "],\"layer:4\":[" + layer4
                        + "],\"layer:5\":[" + layer5
                        + "],\"layer:6\":[" + layer6
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion
                }
                else if (GetString("active") == "customerlevel")
                {
                    #region 客户级别

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<CustomerLevelInfo> levellist = CustomerLevels.Instance.GetListByCorpid(CurrentCorporationID, true);
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string layer = "{\"ikey\":" + levellist.Count + ",\"id\":0,\"name\":\"合计\"}," + string.Join(",", levellist.Select(l => "{\"ikey\":" + levellist.IndexOf(l) + ",\"id\":" + l.ID + ",\"name\":\"" + l.Name + "\"}")); //客户等级
                        string ilayer = "\"合计\"," + string.Join(",", levellist.Select(l => "\"" + l.Name + "\""));
                        string layer0 = string.Empty; //合计
                        List<string> layerlist = new List<string>();
                        string user = string.Empty;
                        int layer0count = 0;
                        List<int> layercountlist = new List<int>();

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                for (int i = 0; i < levellist.Count; i++)
                                {
                                    int y = list.FindAll(l => l.CreateUserID == admin.ID && l.LastCustomerLevelID == levellist[i].ID).Count;
                                    if (layerlist.Count > i)
                                    {
                                        layerlist[i] += "," + y;
                                    }
                                    else
                                        layerlist.Add(y.ToString());
                                }
                                int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.LastCustomerLevelID > 0).Count;
                                layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();

                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int y0othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.LastCustomerLevelID > 0).Count;
                            for (int i = 0; i < levellist.Count; i++)
                            {
                                int yother = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.LastCustomerLevelID == levellist[i].ID).Count;
                                if (layerlist.Count > i)
                                    layerlist[i] += "," + yother;
                                else
                                    layerlist.Add(yother.ToString());
                            }
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0othercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            string[] groupidlist = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string groupid in groupidlist)
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    for (int i = 0; i < levellist.Count; i++)
                                    {
                                        int y = list.FindAll(l => l.CreateUserID == admin.ID && l.LastCustomerLevelID == levellist[i].ID).Count;
                                        if (layerlist.Count > i)
                                            layerlist[i] += "," + y;
                                        else
                                            layerlist.Add(y.ToString());
                                    }
                                    int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.LastCustomerLevelID > 0).Count;
                                    layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();

                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));

                            int y0othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.LastCustomerLevelID > 0).Count;
                            for (int i = 0; i < levellist.Count; i++)
                            {
                                int yother = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.LastCustomerLevelID == levellist[i].ID).Count;
                                if (layerlist.Count > i)
                                    layerlist[i] += "," + yother;
                                else
                                    layerlist.Add(yother.ToString());
                            }
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0othercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                for (int i = 0; i < levellist.Count; i++)
                                {
                                    int y = list.FindAll(l => l.CreateUserID == admin.ID && l.LastCustomerLevelID == levellist[i].ID).Count;
                                    if (layerlist.Count > i)
                                        layerlist[i] += "," + y;
                                    else
                                        layerlist.Add(y.ToString());
                                }
                                int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.LastCustomerLevelID > 0).Count;
                                layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();

                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + "0";
                            for (int i = 0; i < levellist.Count; i++)
                            {
                                if (layerlist.Count > i)
                                    layerlist[i] += ",0";
                                else
                                    layerlist.Add("0");
                            }
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        layer0count = list.FindAll(l => l.LastCustomerLevelID > 0).Count;

                        result = "{\"heji\":{\"layer:0\":" + layer0count;
                        for (int i = 0; i < levellist.Count; i++)
                        {
                            result += ",\"layer:" + (i + 1) + "\":" + list.FindAll(l => l.LastCustomerLevelID == levellist[i].ID).Count;
                        }
                        result += "},\"layer\":[" + layer
                        + "],\"user\":[" + user
                        + "],\"ilayer\":[" + ilayer
                        + "],\"layer:0\":[" + layer0;
                        for (int i = 0; i < levellist.Count; i++)
                        {
                            result += "],\"layer:" + (i + 1) + "\":[" + layerlist[i];
                        }
                        result += "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion

                    //Response.Clear();
                    //Response.Write("{\"layer\":[{\"ikey\":0,\"id\":\"751\",\"name\":\"H\"},{\"ikey\":1,\"id\":\"752\",\"name\":\"A\"},{\"ikey\":2,\"id\":\"753\",\"name\":\"B\"},{\"ikey\":3,\"id\":\"754\",\"name\":\"C\"},{\"ikey\":4,\"id\":\"755\",\"name\":\"D\"},{\"ikey\":5,\"id\":\"2633\",\"name\":\"H+\"},{\"ikey\":6,\"id\":\"4453\",\"name\":\"A+\"},{\"ikey\":7,\"id\":\"6523\",\"name\":\"H++\"}],\"user\":[],\"ilayer\":[\"\u5408\u8ba1\",\"H\",\"A\",\"B\",\"C\",\"D\",\"H+\",\"A+\",\"H++\"],\"layer:0\":[0,0,0,54,22,3,4,2,1,2,7,1,4,1,0,0,1,0,0,1,1,3,1,0,0,0],\"heji\":{\"layer:0\":108,\"layer:1\":68,\"layer:2\":4,\"layer:3\":2,\"layer:4\":1,\"layer:5\":0,\"layer:6\":22,\"layer:7\":2,\"layer:8\":9},\"layer:1\":[0,0,0,34,13,1,1,0,0,1,7,1,4,1,0,0,0,0,0,1,1,3,0,0,0,0],\"layer:2\":[0,0,0,1,1,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0],\"layer:3\":[0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"layer:4\":[0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"layer:5\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"layer:6\":[0,0,0,14,3,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0],\"layer:7\":[0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"layer:8\":[0,0,0,3,4,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"unit\":\"%\"}");
                    //Response.End();
                }
                else if (GetString("active") == "overtime")
                {
                    #region 追踪超时率

                    string uids = GetString("uid[]");
                    string groupids = GetString("groupid[]");
                    List<AdminInfo> listuser = Admins.Instance.GetAllAdmins();
                    listuser = listuser.FindAll(l => l.CorporationID == CurrentCorporationID);
                    List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(CurrentCorporationID, true);
                    list = list.FindAll(l => listuser.Exists(u => u.ID == l.CreateUserID) && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) >= starttimevalue && DataConvert.SafeFloat(rtime.Replace(l.CreateTime, string.Empty)) <= endtimevalue);
                    if (list.Count > 0)
                    {
                        string layer = "\"正常\",\"正常(24小时内超时)\",\"追踪超时\"";
                        string ilayer = "\"正常\",\"正常(24小时内超时)\",\"追踪超时\"";
                        string layer0 = string.Empty; //正常
                        string layer1 = string.Empty; //正常(24小时内超时)
                        string layer2 = string.Empty; //追踪超时
                        string user = string.Empty;
                        int layer0count = 0;
                        int layer1count = 0;
                        int layer2count = 0;

                        if (!string.IsNullOrEmpty(uids))
                        {
                            string[] uid = uids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (AdminInfo admin in listuser.FindAll(l => uid.Contains(l.ID.ToString())))
                            {
                                int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "0").Count;
                                layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();
                                int y1 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "1").Count;
                                layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1.ToString();
                                int y2 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "2").Count;
                                layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2.ToString();

                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            int y0othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.ConnectAlarm == "0").Count;
                            int y1othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.ConnectAlarm == "1").Count;
                            int y2othercount = list.FindAll(l => !uid.Contains(l.CreateUserID.ToString()) && l.ConnectAlarm == "2").Count;
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0othercount.ToString();
                            layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1othercount.ToString();
                            layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2othercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else if (!string.IsNullOrEmpty(groupids))
                        {
                            string[] groupidlist = groupids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string groupid in groupidlist)
                            {
                                foreach (AdminInfo admin in listuser.FindAll(l => l.PowerGroupID.ToString() == groupid))
                                {
                                    int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "0").Count;
                                    layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();
                                    int y1 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "1").Count;
                                    layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1.ToString();
                                    int y2 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "2").Count;
                                    layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2.ToString();

                                    user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                                }
                            }
                            listuser = listuser.FindAll(l => !groupidlist.Contains(l.PowerGroupID.ToString()));

                            int y0othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.ConnectAlarm == "0").Count;
                            int y1othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.ConnectAlarm == "1").Count;
                            int y2othercount = list.FindAll(l => !listuser.Exists(u => u.ID == l.CreateUserID) && l.ConnectAlarm == "2").Count;
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0othercount.ToString();
                            layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1othercount.ToString();
                            layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2othercount.ToString();
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        else
                        {
                            foreach (AdminInfo admin in listuser)
                            {
                                int y0 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "0").Count;
                                layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + y0.ToString();
                                int y1 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "1").Count;
                                layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + y1.ToString();
                                int y2 = list.FindAll(l => l.CreateUserID == admin.ID && l.ConnectAlarm == "2").Count;
                                layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + y2.ToString();

                                user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", admin.RealnameAndGroupname);
                            }
                            layer0 += (string.IsNullOrEmpty(layer0) ? string.Empty : ",") + "0";
                            layer1 += (string.IsNullOrEmpty(layer1) ? string.Empty : ",") + "0";
                            layer2 += (string.IsNullOrEmpty(layer2) ? string.Empty : ",") + "0";
                            user += (string.IsNullOrEmpty(user) ? string.Empty : ",") + string.Format("\"{0}\"", "其他");
                        }
                        layer0count = list.FindAll(l => l.ConnectAlarm == "0").Count;
                        layer1count = list.FindAll(l => l.ConnectAlarm == "1").Count;
                        layer2count = list.FindAll(l => l.ConnectAlarm == "2").Count;

                        result = "{\"heji\":{\"layer:0\":" + layer0count
                        + ",\"layer:1\":" + layer1count
                        + ",\"layer:2\":" + layer2count
                        + "},\"layer\":[" + layer
                        + "],\"user\":[" + user
                        + "],\"ilayer\":[" + ilayer
                        + "],\"layer:0\":[" + layer0
                        + "],\"layer:1\":[" + layer1
                        + "],\"layer:2\":[" + layer2
                        + "],\"unit\":\"%\"}";
                    }

                    Response.Clear();
                    Response.Write(result);
                    Response.End();

                    #endregion

                    //Response.Clear();
                    //Response.Write("{\"layer\":[\"\u6b63\u5e38\",\"\u6b63\u5e38(24\u5c0f\u65f6\u5185\u8d85\u65f6)\",\"\u8ffd\u8e2a\u8d85\u65f6\"],\"user\":[\"\u738b\u73b2\u5f66(\u7cfb\u7edf\u7ba1\u7406\u5458)\",\"\u9ec4\u96bd\u7476(\u5e02\u573a\u4e13\u5458)\",\"\u66fe\u82cf\u4e39(\u5e02\u573a\u4e13\u5458)\",\"\u6c88\u84c9\u84c9(IB\u7535\u8bdd\u8425\u9500\u5458)\",\"\u5218\u4f73\u4f73(OB\u7535\u8bdd\u8425\u9500\u5458)\",\"\u6797\u4f1f(\u76f4\u9500\u4e13\u5458)\",\"\u66f9\u9601(\u76f4\u9500\u4e13\u5458)\",\"\u6234\u6653\u6b66dcc(\u76f4\u9500\u4e13\u5458)\",\"\u8521\u6653\u6e05(\u9500\u552e\u987e\u95ee)\",\"\u6bdb\u660e\u73e0(\u9500\u552e\u987e\u95ee)\",\"\u674e\u5a1f(\u9500\u552e\u987e\u95ee)\",\"\u66f9\u9ad8\u7fd4(\u9500\u552e\u987e\u95ee)\",\"\u9648\u6d01(\u9500\u552e\u987e\u95ee)\",\"\u6c88\u950b(\u9500\u552e\u987e\u95ee)\",\"\u9648\u4ec1\u4e49(\u9500\u552e\u987e\u95ee)\",\"\u962e\u5229\u660e(\u9500\u552e\u987e\u95ee)\",\"\u6797\u5cf0(\u9500\u552e\u987e\u95ee)\",\"\u9648\u6e05\u9521(\u9500\u552e\u987e\u95ee)\",\"\u5f20\u9752\u9752(\u9500\u552e\u987e\u95ee)\",\"\u6bdb\u65b9\u6d0b(\u9500\u552e\u987e\u95ee)\",\"\u6c5f\u601d\u601d(\u9500\u552e\u987e\u95ee)\",\"\u91d1\u5fae\u53cc(\u9500\u552e\u987e\u95ee)\",\"\u6234\u6653\u6b66(IDCC\u4e3b\u7ba1)\",\"\u5362\u5b50\u541b(\u5c55\u5385\u7763\u67e5\u4e13\u5458)\",\"\u8521\u6c38\u4e45(\u9500\u552e\u7ecf\u7406)\",\"\u5176\u4ed6\"],\"ilayer\":[\"\u6b63\u5e38\",\"\u6b63\u5e38(24\u5c0f\u65f6\u5185\u8d85\u65f6)\",\"\u8ffd\u8e2a\u8d85\u65f6\"],\"layer:0\":[0,0,0,54,22,3,4,2,3,1,7,1,6,1,0,0,1,0,0,3,3,5,1,0,0,0],\"heji\":{\"layer:0\":117,\"layer:1\":1,\"layer:2\":2},\"layer:1\":[0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"layer:2\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0],\"unit\":\"%\"}");
                    //Response.End();
                }
            }
        }
    }
}