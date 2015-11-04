using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin
{
    public partial class desktop : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        protected int VoluntaryNum { get; set; }
        protected int ForcedNum { get; set; }
        protected int ConnecttimeoutNum { get; set; }
        protected int ConnecttimeoutingNum { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            if (Corporation != null)
            {
                List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(Corporation.ID, true);
                list = list.FindAll(l => l.LurkStatus == 0);
                list = list.FindAll(l => l.CheckStatus == 0);
                if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                    list = list.FindAll(l => l.OwnerID == AdminID);

                List<CustomerInfo> listVoluntary = new List<CustomerInfo>();
                //主动转出
                if (Corporation.Trackmove == 1)
                    listVoluntary.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.导入_集客));
                if (Corporation.Voluntaryoffday > 0)
                    listVoluntary.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (Corporation.Voluntaryoffday - 1)));
                if (Corporation.Voluntaryoutday > 0)
                    listVoluntary.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (Corporation.Voluntaryoutday - 1)));
                VoluntaryNum = listVoluntary.Count;

                List<CustomerInfo> listForced = new List<CustomerInfo>();
                //7天内强制转出
                if (Corporation.Forcedoffday > 0)
                {
                    string[] offcustomerlevel = Corporation.Offcustomerlevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    listForced.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (Corporation.Forcedoffday - 1) && !offcustomerlevel.Contains(l.LastCustomerLevelID.ToString())));
                }
                if (Corporation.Forcedoutday > 0)
                {
                    string[] forcedoutdaylevel = Corporation.Forcedoutdaylevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    listForced.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (Corporation.Forcedoutday - 1) && !forcedoutdaylevel.Contains(l.LastCustomerLevelID.ToString())));
                }
                ForcedNum = listForced.Count;

                ConnecttimeoutNum = list.FindAll(l => l.ConnectAlarm == "2").Count;
                ConnecttimeoutingNum = list.FindAll(l => l.ConnectAlarm == "1").Count;
            }
        }
    }
}