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
    public partial class loglist : AdminBase
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
            if (!IsPostBack)
            {
                int page = GetInt("page", 1);
                int rows = GetInt("rows", 100);
                int type = GetInt("type",-1);
                string startime = GetString("starttime");
                string endtime = GetString("endtime");
                
                int total = 0;

                EventLogQuery query = new EventLogQuery();
                if(type >= 0)
                    query.EventType = type;
                if(!string.IsNullOrEmpty(startime))
                    query.StartTime = DateTime.Parse(startime);
                if(!string.IsNullOrEmpty(endtime))
                    query.EndTime = DateTime.Parse(endtime);

                List<EventLogEntry> list = EventLogs.GetList(page, rows, query, out total);

                int maxpage = total / rows + (total % rows == 0 ? 0 : 1);


                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}