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
    public partial class loginrecordmg : AdminBase
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

        private CorporationInfo currentcorporation = null;
        public CorporationInfo CurrentCorporation
        {
            get
            {
                if (currentcorporation == null)
                {
                    if (GetInt("corpid") > 0)
                        currentcorporation = Corporations.Instance.GetModel(GetInt("corpid"));
                    else if (Corporation != null)
                        currentcorporation = Corporation;
                }

                return currentcorporation;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCorporationSearch.DataSource = Corporations.Instance.GetList(true);
                ddlCorporationSearch.DataTextField = "Name";
                ddlCorporationSearch.DataValueField = "ID";
                ddlCorporationSearch.DataBind();

                SetSelectedByValue(ddlCorporationSearch, GetString("corpid"));

                //List<AdminInfo> list = Admins.Instance.GetAllAdmins();
                //list = list.FindAll(l => l.CorporationID == DataConvert.SafeInt(ddlCorporationSearch.SelectedValue));
                //if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员)
                //{
                //    if (CurrentPowerGroup != null && !string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds))
                //    {
                //        string[] powers = CurrentPowerGroup.CanviewGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                //        list = list.FindAll(l => l.ID == AdminID || powers.Contains(l.PowerGroupID.ToString()));
                //    }
                //    else
                //        list = list.FindAll(l => l.ID == AdminID);
                //}

                //list = list.OrderBy(l => (int)l.UserRole).ThenBy(l => l.UserName).ToList();
                //rptData.DataSource = list;
                //rptData.DataBind();
            }
        }
    }
}