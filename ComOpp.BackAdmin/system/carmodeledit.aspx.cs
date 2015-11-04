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
    public partial class carmodeledit : AdminBase
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

        public CarModelInfo CurrentCarModel { get; set; }

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
                CurrentCarModel = Cars.Instance.GetCarModelModel(id, true);

                txtName.Value = CurrentCarModel.Name;
                SetSelectedByValue(ddlCarBrand, CurrentCarModel.BrandID.ToString());
            }

            ddlCarSeries.DataSource = Cars.Instance.GetCarSeriesListByBrandID(CurrentCarModel == null ? DataConvert.SafeInt(ddlCarBrand.Items[0].Value) : CurrentCarModel.BrandID, true);
            ddlCarSeries.DataTextField = "Name";
            ddlCarSeries.DataValueField = "ID";
            ddlCarSeries.DataBind();

            if (id > 0)
            {
                SetSelectedByValue(ddlCarSeries, CurrentCarModel.SeriesID.ToString());
                hdnCarSeries.Value = CurrentCarModel.SeriesID.ToString();
            }
            else
                hdnCarSeries.Value = ddlCarSeries.Items[0].Value;
        }

        private void FillData(CarModelInfo entity)
        {
            entity.Name = txtName.Value;
            entity.SeriesID = DataConvert.SafeInt(hdnCarSeries.Value);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CarModelInfo entity = new CarModelInfo();
            int id = GetInt("id");
            if (id > 0) entity = Cars.Instance.GetCarModelModel(id, true);
            FillData(entity);

            if (id > 0) Cars.Instance.UpdateCarModel(entity);
            else Cars.Instance.AddCarModel(entity);

            Cars.Instance.ReloadCarModelListCache();
            Response.Redirect("carmodelmg.aspx");
        }
    }
}