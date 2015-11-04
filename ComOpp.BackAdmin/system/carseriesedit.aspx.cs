using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.system
{
    public partial class carseriesedit : AdminBase
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

        public CarSeriesInfo CurrentCarSeries { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControlor();
                LoadData();
            }
        }

        private void BindControlor()
        {
            ddlCarBrand.DataSource = Cars.Instance.GetCarBrandList(true);
            ddlCarBrand.DataTextField = "Name";
            ddlCarBrand.DataValueField = "ID";
            ddlCarBrand.DataBind();
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentCarSeries = Cars.Instance.GetCarSeriesModel(id, true);

                txtName.Value = CurrentCarSeries.Name;
                SetSelectedByValue(ddlCarBrand, CurrentCarSeries.BrandID.ToString());
            }
        }

        private void FillData(CarSeriesInfo entity)
        {
            entity.Name = txtName.Value;
            entity.BrandID = DataConvert.SafeInt(ddlCarBrand.SelectedValue);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CarSeriesInfo entity = new CarSeriesInfo();
            int id = GetInt("id");
            if (id > 0) entity = Cars.Instance.GetCarSeriesModel(id, true);
            FillData(entity);

            if (id > 0) Cars.Instance.UpdateCarSeries(entity);
            else Cars.Instance.AddCarSeries(entity);

            Cars.Instance.ReloadCarSeriesListCache();
            Response.Redirect("carseriesmg.aspx");
        }
    }
}