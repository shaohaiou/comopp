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
    public partial class infosourceedit : AdminBase
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

        public InfoSourceInfo CurrentInfoSource { get; set; }

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
                CurrentInfoSource = InfoSources.Instance.GetModel(id);
                if (CurrentInfoSource == null)
                    WriteMessage("/message.aspx", "系统提示", "无效信息来源！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentInfoSource.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentInfoSource.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentInfoSource.Name;
                }
            }
        }

        private void FillData(InfoSourceInfo entity)
        {
            entity.Name = txtName.Value;
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            InfoSourceInfo entity = new InfoSourceInfo();
            int id = GetInt("id");

            if (id > 0)
            {
                entity = InfoSources.Instance.GetModel(id, true);
                FillData(entity);
                InfoSources.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                InfoSources.Instance.Add(entity);
            }

            InfoSources.Instance.ReloadInfoSourceListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "infotypemg.aspx" : FromUrl);
        }
    }
}