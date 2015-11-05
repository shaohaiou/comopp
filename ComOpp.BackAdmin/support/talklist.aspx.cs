using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.support
{
    public partial class talklist : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "话术管理"))
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
                int corpid = GetInt("corpid", 0);
                string keywords = GetString("keywords");
                string starttime = GetString("starttime");
                string endtime = GetString("endtime");

                List<TalkInfo> list = Talks.Instance.GetList(true);
                if (corpid > 0)
                    list = list.FindAll(l => l.CorporationID == corpid);
                if (!string.IsNullOrEmpty(keywords))
                    list = list.FindAll(l=>l.Title.ToLower().Contains(keywords.ToLower()) || l.Tag.ToLower().Contains(keywords.ToLower()));
                if (!string.IsNullOrEmpty(starttime))
                {
                    DateTime timestart = DataConvert.SafeDate(starttime);
                    list = list.FindAll(l=>l.AddTime >= timestart);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    DateTime timeend = DataConvert.SafeDate(endtime);
                    list = list.FindAll(l => l.AddTime <= timeend);
                }
                list = list.OrderByDescending(l => l.ID).ToList();
                int total = list.Count;
                int maxpage = list.Count / rows + (list.Count % rows == 0 ? 0 : 1);

                list = list.Skip((page - 1) * rows).Take(rows).ToList<TalkInfo>();

                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}