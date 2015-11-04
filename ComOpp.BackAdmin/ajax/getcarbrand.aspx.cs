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
    public partial class getcarbrand : AdminBase
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
            int t = GetInt("t");
            int pid = GetInt("pid");
            if (t == 1)
            {
                List<CarBrandInfo> list = Cars.Instance.GetCarBrandList(true);
                Response.Clear();
                Response.Write(Serializer.SerializeJson(list));
                Response.End();
            }
            else if (t == 2 && pid > 0)
            {
                List<CarSeriesInfo> list = Cars.Instance.GetCarSeriesListByBrandID(pid, true);
                Response.Clear();
                Response.Write(Serializer.SerializeJson(list));
                Response.End();
            }
            else if (t == 3 && pid > 0)
            {
                List<CarModelInfo> list = Cars.Instance.GetCarModelListBySeriesID(pid, true);
                Response.Clear();
                Response.Write(Serializer.SerializeJson(list));
                Response.End();
            }
        }
    }
}