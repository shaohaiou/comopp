using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.system
{
    public partial class logdetail : AdminBase
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

        private EventLogEntry currentlogentry = null;
        protected EventLogEntry CurrentLogEntry
        {
            get
            {
                if (currentlogentry == null)
                {
                    int id = GetInt("id");
                    EventLogs.GetModel(id);
                }

                return currentlogentry;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}