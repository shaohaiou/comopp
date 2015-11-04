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
    public partial class giveupcauseedit : AdminBase
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

        public GiveupCauseInfo CurrentGiveupCause { get; set; }

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
                CurrentGiveupCause = GiveupCauses.Instance.GetModel(id);
                if (CurrentGiveupCause == null)
                    WriteMessage("/message.aspx", "系统提示", "无效放弃原因！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentGiveupCause.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentGiveupCause.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentGiveupCause.Name;
                }
            }
        }

        private void FillData(GiveupCauseInfo entity)
        {
            entity.Name = txtName.Value;
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GiveupCauseInfo entity = new GiveupCauseInfo();
            int id = GetInt("id");
            if (id > 0)
            {
                entity = GiveupCauses.Instance.GetModel(id, true);
                FillData(entity);
                GiveupCauses.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                GiveupCauses.Instance.Add(entity);
            }

            GiveupCauses.Instance.ReloadGiveupCauseListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "giveupcausemg.aspx" : FromUrl);
        }
    }
}