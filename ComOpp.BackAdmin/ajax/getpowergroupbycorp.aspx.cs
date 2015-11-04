using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.ajax
{
    public partial class getpowergroupbycorp : AdminBase
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
            int id = GetInt("id");
            if (id > 0)
            {
                List<PowerGroupInfo> plist = PowerGroups.Instance.GetList(true);
                plist = plist.FindAll(l => l.CorporationID == id);
                rptData.DataSource = plist;
                rptData.DataBind();
            }
        }
    }
}