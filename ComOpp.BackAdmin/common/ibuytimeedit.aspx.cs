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
    public partial class ibuytimeedit : AdminBase
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

        public IbuytimeInfo CurrentIbuytime { get; set; }

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
                CurrentIbuytime = Ibuytimes.Instance.GetModel(id);
                if (CurrentIbuytime == null)
                    WriteMessage("/message.aspx", "系统提示", "无效拟购时间！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentIbuytime.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentIbuytime.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentIbuytime.Name;
                }
            }
        }

        private void FillData(IbuytimeInfo entity)
        {
            entity.Name = txtName.Value;
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            IbuytimeInfo entity = new IbuytimeInfo();
            int id = GetInt("id");
            if (id > 0)
            {
                entity = Ibuytimes.Instance.GetModel(id, true);
                FillData(entity);
                Ibuytimes.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                Ibuytimes.Instance.Add(entity);
            }

            Ibuytimes.Instance.ReloadIbuytimeListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "ibuytimemg.aspx" : FromUrl);
        }
    }
}