using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.system
{
    public partial class corporationmg : AdminBase
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
                    Corporations.Instance.Delete(GetString("ids"));
                else if (GetString("action") == "moveup" && GetInt("id") > 0)
                    Corporations.Instance.MoveUp(GetInt("id"));
                else if (GetString("action") == "movedown" && GetInt("id") > 0)
                    Corporations.Instance.MoveDown(GetInt("id"));
                else if (GetString("action") == "movetop" && GetInt("id") > 0)
                    Corporations.Instance.MoveTop(GetInt("id"));
                
                Corporations.Instance.ReloadCorporationListCache();
                Response.Clear();
                Response.Write("{\"state\":1}");
                Response.End();
            }
        }
    }
}