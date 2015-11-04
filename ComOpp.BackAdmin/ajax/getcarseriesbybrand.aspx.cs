using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.ajax
{
    public partial class getcarseriesbybrand : AdminBase
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
                rptData.DataSource = Cars.Instance.GetCarSeriesListByBrandID(id);
                rptData.DataBind();
            }
        }
    }
}