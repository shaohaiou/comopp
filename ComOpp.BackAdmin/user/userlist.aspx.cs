using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.user
{
    public partial class userlist : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "账户管理"))
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
                int state = GetInt("state");
                int powergroup = GetInt("powergroup");

                List<AdminInfo> list = Admins.Instance.GetUsers();
                list = list.FindAll(l => l.CorporationID == corpid);
                if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                {
                    if (CurrentPowerGroup != null && !string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds))
                    {
                        string[] powers = CurrentPowerGroup.CanviewGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => l.ID == AdminID || powers.Contains(l.PowerGroupID.ToString()));
                    }
                    else
                        list = list.FindAll(l => l.ID == AdminID);
                }
                if (state > 0)
                    list = list.FindAll(l => l.State == state.ToString());
                if (powergroup > 0)
                    list = list.FindAll(l => l.PowerGroupID == powergroup);
                if (!string.IsNullOrEmpty(keywords))
                    list = list.FindAll(l => l.UserName.ToLower().Contains(keywords.ToLower()) || l.Realname.ToLower().Contains(keywords.ToLower()) || l.QQ.ToLower().Contains(keywords.ToLower()));

                list = list.OrderBy(l => l.CorporationID).ThenBy(l => l.PowerGroupID).ThenBy(l => l.ID).ToList();

                int total = list.Count;
                int maxpage = list.Count / rows + (list.Count % rows == 0 ? 0 : 1);

                list = list.Skip((page - 1) * rows).Take(rows).ToList<AdminInfo>();

                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}