using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.system
{
    public partial class carbrandedit : AdminBase
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

        public CarBrandInfo CurrentCarBrand { get; set; }

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
                CurrentCarBrand = Cars.Instance.GetCarBrandModel(id, true);

                txtName.Value = CurrentCarBrand.Name;
                txtNameIndex.Value = CurrentCarBrand.NameIndex;
            }
        }

        private void FillData(CarBrandInfo entity)
        {
            entity.Name = txtName.Value;
            entity.NameIndex = txtNameIndex.Value;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CarBrandInfo entity = new CarBrandInfo();
            int id = GetInt("id");
            if (id > 0) entity = Cars.Instance.GetCarBrandModel(id, true);
            FillData(entity);

            if (id > 0) Cars.Instance.UpdateCarBrand(entity);
            else Cars.Instance.AddCarBrand(entity);

            Cars.Instance.ReloadCarBrandListCache();
            Response.Redirect("carbrandmg.aspx");
        }
    }
}