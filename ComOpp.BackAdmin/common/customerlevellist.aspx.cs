﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.common
{
    public partial class customerlevellist : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "基础数据"))
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

                List<CustomerLevelInfo> list = CustomerLevels.Instance.GetList(true);
                if (corpid > 0)
                    list = list.FindAll(l => l.CorporationID == corpid);
                list = list.OrderBy(l => l.CorporationID).ThenBy(l => l.Sort).ToList();
                int total = list.Count;
                int maxpage = list.Count / rows + (list.Count % rows == 0 ? 0 : 1);

                list = list.Skip((page - 1) * rows).Take(rows).ToList<CustomerLevelInfo>();

                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}