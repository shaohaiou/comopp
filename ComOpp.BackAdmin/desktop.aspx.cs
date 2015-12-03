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

        protected int corpid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
                LoadData();
            }
        }

        private void BindControler()
        {
            ddlCorporation.DataSource = Corporations.Instance.GetList(true);
            ddlCorporation.DataTextField = "Name";
            ddlCorporation.DataValueField = "ID";
            ddlCorporation.DataBind();
        }

        private void LoadData()
        {
            corpid = Corporation == null ? 0 : Corporation.ID;
            if (Admin.Administrator) corpid = DataConvert.SafeInt(ddlCorporation.SelectedValue);
            CorporationInfo corpinfo = Corporations.Instance.GetModel(corpid,true);

            if (corpinfo != null)
            {
                List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(corpinfo.ID, true);
                list = list.FindAll(l => l.LurkStatus == 0);
                list = list.FindAll(l => l.CheckStatus == 0);
                if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                {
                    if (CurrentPowerGroup != null && !string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds))
                    {
                        string[] powers = CurrentPowerGroup.CanviewGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        list = list.FindAll(l => l.OwnerID == AdminID || powers.Contains(l.OwnerPowerGroupID.ToString()));
                    }
                    else
                        list = list.FindAll(l => l.OwnerID == AdminID);
                }

                List<CustomerInfo> listVoluntary = new List<CustomerInfo>();
                //主动转出
                if (corpinfo.Trackmove == 1)
                    listVoluntary.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.导入_集客));
                if (corpinfo.Voluntaryoffday > 0)
                    listVoluntary.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corpinfo.Voluntaryoffday - 1)));
                if (corpinfo.Voluntaryoutday > 0)
                    listVoluntary.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corpinfo.Voluntaryoutday - 1)));
                VoluntaryNum = listVoluntary.Count;

                List<CustomerInfo> listForced = new List<CustomerInfo>();
                //7天内强制转出
                if (corpinfo.Forcedoffday > 0)
                {
                    string[] offcustomerlevel = corpinfo.Offcustomerlevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    listForced.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.清洗_邀约 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corpinfo.Forcedoffday - 7) && !offcustomerlevel.Contains(l.LastCustomerLevelID.ToString())));
                }
                if (corpinfo.Forcedoutday > 0)
                {
                    string[] forcedoutdaylevel = corpinfo.Forcedoutdaylevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    listForced.AddRange(list.FindAll(l => l.CustomerStatus == (int)CustomerStatus.追踪_促成 && DateTime.Today.Subtract(DataConvert.SafeDate(l.PostTime)).TotalDays >= (corpinfo.Forcedoutday - 7) && !forcedoutdaylevel.Contains(l.LastCustomerLevelID.ToString())));
                }
                ForcedNum = listForced.Count;

                ConnecttimeoutNum = list.FindAll(l => l.ConnectAlarm == "2").Count;
                ConnecttimeoutingNum = list.FindAll(l => l.ConnectAlarm == "1").Count;
            }
        }

        protected void ddlCorporation_SelectedIndexChanged(object sender,EventArgs e)
        {
            LoadData();
        }
    }
}