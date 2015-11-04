using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.common
{
    public partial class customerleveledit : AdminBase
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

        public CustomerLevelInfo CurrentCustomerLevel { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentCustomerLevel = CustomerLevels.Instance.GetModel(id);
                if (CurrentCustomerLevel == null)
                    WriteMessage("/message.aspx", "系统提示", "无效客户等级！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentCustomerLevel.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentCustomerLevel.Name;
                    txtDrtday.Value = CurrentCustomerLevel.Drtday.ToString();
                    txtAlarmday.Value = CurrentCustomerLevel.Alarmday.ToString();
                }
            }
        }

        private void FillData(CustomerLevelInfo entity)
        {
            entity.Name = txtName.Value;
            entity.Drtday = DataConvert.SafeInt(txtDrtday.Value);
            entity.Alarmday = DataConvert.SafeInt(txtAlarmday.Value);
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CustomerLevelInfo entity = new CustomerLevelInfo();
            int id = GetInt("id");
            if (id > 0) entity = CustomerLevels.Instance.GetModel(id, true);
            FillData(entity);

            if (id > 0) CustomerLevels.Instance.Update(entity);
            else CustomerLevels.Instance.Add(entity);

            CustomerLevels.Instance.ReloadCustomerLevelListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "customerlevelmg.aspx" : FromUrl);
        }
    }
}