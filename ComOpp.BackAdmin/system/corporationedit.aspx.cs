using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.system
{
    public partial class corporationedit : AdminBase
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

        public CorporationInfo CurrentCorporation { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentCorporation = Corporations.Instance.GetModel(id, true);

                txtName.Value = CurrentCorporation.Name;
            }
        }

        private void FillData(CorporationInfo entity)
        {
            entity.Name = txtName.Value;
        }

        protected void btnSubmit_Click(object sender,EventArgs e)
        {
            CorporationInfo entity = new CorporationInfo();
            int id = GetInt("id");
            if (id > 0) entity = Corporations.Instance.GetModel(id, true);
            FillData(entity);

            if (id > 0) Corporations.Instance.Update(entity);
            else Corporations.Instance.Add(entity);

            Corporations.Instance.ReloadCorporationListCache();
            Response.Redirect("corporationmg.aspx");
        }
    }
}