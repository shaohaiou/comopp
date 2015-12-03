using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.chance
{
    public partial class followlist : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "追踪流水"))
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
                int total = 0;
                int corpid = GetInt("corpid", 0);
                string customername = GetString("uname");
                string customerphone = GetString("phone");
                int connectuserid = GetInt("uid",0);
                int connectwayid = GetInt("connectway",0);
                int customerlevelid = GetInt("customerlevel",0);
                string starttime = GetString("starttime");
                string endtime = GetString("endtime");

                CustomerConnectRecordQuery query = new CustomerConnectRecordQuery();
                query.CorporationID = corpid;
                if (!string.IsNullOrEmpty(customername))
                    query.CustomerName = customername;
                if (!string.IsNullOrEmpty(customerphone))
                    query.CustomerPhone = customerphone;
                if(connectuserid > 0)
                    query.ConnectUserID = connectuserid;
                if (connectwayid > 0)
                    query.ConnectwayID = connectwayid;
                if (customerlevelid > 0)
                    query.CustomerLevelID = customerlevelid;
                if (!string.IsNullOrEmpty(starttime))
                    query.StartTime = DataConvert.SafeDate(starttime);
                if (!string.IsNullOrEmpty(endtime))
                    query.EndTime = DataConvert.SafeDate(endtime);
                if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                {
                    if (CurrentPowerGroup != null && !string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds))
                    {
                        query.CanviewGroupIds = CurrentPowerGroup.CanviewGroupIds;
                    }
                }

                List<CustomerConnectRecordInfo> list = CustomerConnectRecords.Instance.GetList(query, page, rows, ref total);

                int maxpage = total / rows + (total % rows == 0 ? 0 : 1);

                list = list.Skip((page - 1) * rows).Take(rows).ToList<CustomerConnectRecordInfo>();

                Response.Write("{\"count\":" + list.Count + ",\"total\":" + total + ",\"maxpage\":" + maxpage + ",\"rows\":" + Serializer.SerializeJson(list) + "}");
            }
        }
    }
}