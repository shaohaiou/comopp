using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.chance
{
    public partial class archivemerge : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "综合查询"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        private CorporationInfo currentcorporation = null;
        public CorporationInfo CurrentCorporation
        {
            get
            {
                if (currentcorporation == null)
                {
                    if (GetInt("corpid") > 0)
                        currentcorporation = Corporations.Instance.GetModel(GetInt("corpid"));
                    else if (Corporation != null)
                        currentcorporation = Corporation;
                }

                return currentcorporation;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCorporationSearch.DataSource = Corporations.Instance.GetList(true);
                ddlCorporationSearch.DataTextField = "Name";
                ddlCorporationSearch.DataValueField = "ID";
                ddlCorporationSearch.DataBind();
                ddlCorporationSearch.Items.Insert(0, new ListItem("集团公池", "0"));

                SetSelectedByValue(ddlCorporationSearch, GetString("corpid"));
            }
            else
                Response.Redirect("chancemg.aspx?corpid=" + ddlCorporationSearch.SelectedValue);
        }
    }
}