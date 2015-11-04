using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.system
{
    public partial class carseriesmg : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator)
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
                    Cars.Instance.DeleteCarSeries(GetString("ids"));

                Cars.Instance.ReloadCarSeriesListCache();
                Response.Clear();
                Response.Write("{\"state\":1}");
                Response.End();
            }

            if (!IsPostBack)
            {
                ddlCarBrandSearch.DataSource = Cars.Instance.GetCarBrandList(true);
                ddlCarBrandSearch.DataTextField = "Name";
                ddlCarBrandSearch.DataValueField = "ID";
                ddlCarBrandSearch.DataBind();
                ddlCarBrandSearch.Items.Insert(0, new ListItem("-车辆品牌-", "0"));
            }
        }
    }
}