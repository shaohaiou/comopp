using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.user
{
    public partial class usermg : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "账户管理"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        private int? powergroupcount;
        protected int PowerGroupCount
        {
            get
            {
                if (!powergroupcount.HasValue)
                {
                    List<PowerGroupInfo> plist = PowerGroups.Instance.GetList(true);
                    if (!Admin.Administrator)
                        plist = plist.FindAll(l => l.CorporationID == Admin.CorporationID);
                    powergroupcount = plist.Count;
                }
                return powergroupcount.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = GetString("action");
            if (!string.IsNullOrEmpty(action))
            {
                if (GetString("action") == "del")
                {
                    int id = GetInt("id");
                    AdminInfo admin = null;
                    if (id > 0)
                        admin = Admins.Instance.GetAdmin(id);
                    if (admin != null)
                    {
                        if (!Admin.Administrator)
                        {
                            WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                            return;
                        }
                        Admins.Instance.DeleteAdmin(id);

                        Response.Clear();
                        Response.Write("{\"state\":1}");
                        Response.End();
                    }
                }
            }
            if (!IsPostBack)
            {
                ddlCorporationSearch.DataSource = Corporations.Instance.GetList(true);
                ddlCorporationSearch.DataTextField = "Name";
                ddlCorporationSearch.DataValueField = "ID";
                ddlCorporationSearch.DataBind();

                if (GetInt("corpid") > 0)
                    SetSelectedByValue(ddlCorporationSearch, GetString("corpid"));

                if (!Admin.Administrator)
                {
                    List<PowerGroupInfo> plist = PowerGroups.Instance.GetList(true);
                    plist = plist.FindAll(l => l.CorporationID == Admin.CorporationID);
                    ddlPowerGroupSearch.DataSource = plist;
                    ddlPowerGroupSearch.DataTextField = "GroupName";
                    ddlPowerGroupSearch.DataValueField = "ID";
                    ddlPowerGroupSearch.DataBind();
                    ddlPowerGroupSearch.Items.Insert(0, new ListItem("-权限组-", "0"));
                }
            }
        }
    }
}