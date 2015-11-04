using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.common
{
    public partial class giveupcausemg : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "基础数据"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = GetString("action");
            if (!string.IsNullOrEmpty(action))
            {
                int id = GetInt("id");
                int corpid = 0;
                if (id > 0)
                    corpid = GiveupCauses.Instance.GetModel(id, true).CorporationID;
                if (GetString("action") == "del" && !string.IsNullOrEmpty(GetString("ids")))
                    GiveupCauses.Instance.Delete(GetString("ids"));
                else if (GetString("action") == "moveup" && id > 0)
                    GiveupCauses.Instance.MoveUp(id, " WHERE [CorporationID] = " + corpid + " AND State = 1");
                else if (GetString("action") == "movedown" && id > 0)
                    GiveupCauses.Instance.MoveDown(id, " WHERE [CorporationID] = " + corpid + " AND State = 1");
                else if (GetString("action") == "movetop" && id > 0)
                    GiveupCauses.Instance.MoveTop(id, " WHERE [CorporationID] = " + corpid + " AND State = 1");

                GiveupCauses.Instance.ReloadGiveupCauseListCache();
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
                ddlCorporationSearch.Items.Insert(0, new ListItem("系统数据", "0"));

                SetSelectedByValue(ddlCorporationSearch, GetString("corpid"));
            }
            else
                Response.Redirect("giveupcausemg.aspx?corpid=" + ddlCorporationSearch.SelectedValue);
        }
    }
}