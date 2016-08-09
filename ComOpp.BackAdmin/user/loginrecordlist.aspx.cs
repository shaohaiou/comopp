using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Text.RegularExpressions;

namespace ComOpp.BackAdmin.user
{
    public partial class loginrecordlist : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "登录记录"))
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
                int uid = GetInt("uid", 0);
                string starttime = GetString("starttime");
                string endtime = GetString("endtime");
                List<LoginRecordInfo> list = Admins.Instance.GetLoginRecord(uid);
                if (!string.IsNullOrEmpty(starttime))
                    list = list.FindAll(l=>l.LoginTime > DateTime.Parse(starttime));
                if (!string.IsNullOrEmpty(endtime))
                    list = list.FindAll(l => l.LoginTime < DateTime.Parse(endtime));

                list = list.OrderByDescending(l => l.LoginTime).ToList();

                int total = list.Count;
                int maxpage = list.Count / rows + (list.Count % rows == 0 ? 0 : 1);

                list = list.Skip((page - 1) * rows).Take(rows).ToList<LoginRecordInfo>();

                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}