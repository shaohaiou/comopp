using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.system
{
    public partial class moduleedit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator)
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        public ModuleInfo CurrentModule { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }
        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentModule = Modules.Instance.GetModel(id, true);

                txtModuleName.Value = CurrentModule.ModuleName;
                txtParentName.Value = CurrentModule.ParentName;
            }
        }

        private void FillData(ModuleInfo entity)
        {
            entity.ModuleName = txtModuleName.Value;
            entity.ParentName = txtParentName.Value;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ModuleInfo entity = new ModuleInfo();
            int id = GetInt("id");
            if (id > 0) entity = Modules.Instance.GetModel(id, true);
            FillData(entity);

            if (id > 0) Modules.Instance.Update(entity);
            else Modules.Instance.Add(entity);

            Modules.Instance.ReloadModuleListCache();
            Response.Redirect("modulemg.aspx");
        }
    }
}