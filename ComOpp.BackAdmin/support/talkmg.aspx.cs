using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.support
{
    public partial class talkmg : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "话术管理"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = GetString("action");
            if (!string.IsNullOrEmpty(action))
            {
                if (GetString("action") == "del" && !string.IsNullOrEmpty(GetString("ids")))
                    Talks.Instance.Delete(GetString("ids"));

                Talks.Instance.ReloadTalkListCache();
                Response.Clear();
                Response.Write("{\"state\":1}");
                Response.End();
            }
            if (!IsPostBack)
            {
                ddlCorporationSearch.DataSource = Corporations.Instance.GetList(true);
                ddlCorporationSearch.DataTextField = "Name";
                ddlCorporationSearch.DataValueField = "ID";
                ddlCorporationSearch.DataBind();

                if (GetInt("corpid") > 0)
                    SetSelectedByValue(ddlCorporationSearch, GetString("corpid"));
            }
            else
                Response.Redirect("talkmg.aspx?corpid=" + ddlCorporationSearch.SelectedValue);
        }
    }
}