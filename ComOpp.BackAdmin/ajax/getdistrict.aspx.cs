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
    public partial class getdistrict : AdminBase
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
                List<ProvinceInfo> list = Districts.Instance.GetProvinceList(true);
                Response.Clear();
                Response.Write(Serializer.SerializeJson(list));
                Response.End();
            }
            else if (t == 2 && pid > 0)
            {
                List<CityInfo> list = Districts.Instance.GetCityListByPID(pid,true);
                Response.Clear();
                Response.Write(Serializer.SerializeJson(list));
                Response.End();
            }
            else if (t == 3 && pid > 0)
            {
                List<DistrictInfo> list = Districts.Instance.GetDistrictListByCID(pid, true);
                Response.Clear();
                Response.Write(Serializer.SerializeJson(list));
                Response.End();
            }

        }
    }
}