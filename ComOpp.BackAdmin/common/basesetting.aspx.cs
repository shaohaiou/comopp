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
    public partial class basesetting : AdminBase
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
            if (Admin.Administrator)
            {
                ddlCorporation.DataSource = Corporations.Instance.GetList(true);
                ddlCorporation.DataTextField = "Name";
                ddlCorporation.DataValueField = "ID";
                ddlCorporation.DataBind();
            }
            ddlCarBrand.DataSource = Cars.Instance.GetCarBrandList(true);
            ddlCarBrand.DataTextField = "BindName";
            ddlCarBrand.DataValueField = "ID";
            ddlCarBrand.DataBind();

            ddlProvince.DataSource = Districts.Instance.GetProvinceList(true);
            ddlProvince.DataTextField = "Name";
            ddlProvince.DataValueField = "ID";
            ddlProvince.DataBind();
        }

        private void LoadData()
        {
            if (Corporation != null)
            {
                SetSelectedByValue(ddlCarBrand, Corporation.BrandID.ToString());
                SetSelectedByValue(ddlProvince, Corporation.ProvinceID.ToString());
                ddlProvince_SelectedIndexChanged(null, null);
                SetSelectedByValue(ddlCity, Corporation.CityID.ToString());
                ddlCity_SelectedIndexChanged(null, null);
                SetSelectedByValue(ddlDistrict, Corporation.DistrictID.ToString());

                SetSelectedByValue(rblIsProcess, Corporation.IsProcess.ToString());
            }
            else if (Admin.Administrator)
            {
                ddlCorporation_SelectedIndexChanged(null, null);
            }
        }

        private void FillData(CorporationInfo entity)
        {
            entity.BrandID = DataConvert.SafeInt(ddlCarBrand.SelectedValue);
            entity.ProvinceID = DataConvert.SafeInt(ddlProvince.SelectedValue);
            entity.CityID = DataConvert.SafeInt(ddlCity.SelectedValue);
            entity.DistrictID = DataConvert.SafeInt(ddlDistrict.SelectedValue);

            entity.IsProcess = DataConvert.SafeInt(rblIsProcess.SelectedValue);
        }
        
        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCity.DataSource = Districts.Instance.GetCityListByPID(DataConvert.SafeInt(ddlProvince.SelectedValue), true);
            ddlCity.DataTextField = "Name";
            ddlCity.DataValueField = "ID";
            ddlCity.DataBind();

            ddlCity_SelectedIndexChanged(null, null);
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDistrict.DataSource = Districts.Instance.GetDistrictListByCID(DataConvert.SafeInt(ddlCity.SelectedValue), true);
            ddlDistrict.DataTextField = "Name";
            ddlDistrict.DataValueField = "ID";
            ddlDistrict.DataBind();
        }

        protected void ddlCorporation_SelectedIndexChanged(object sender, EventArgs e)
        {
            int corpid = DataConvert.SafeInt(ddlCorporation.SelectedValue);
            CorporationInfo corpinfo = Corporations.Instance.GetModel(corpid, true);
            if (corpinfo.ProvinceID >= 0)
            {
                SetSelectedByValue(ddlCarBrand, corpinfo.BrandID.ToString());

                SetSelectedByValue(ddlProvince, corpinfo.ProvinceID.ToString());
                ddlProvince_SelectedIndexChanged(null, null);
                SetSelectedByValue(ddlCity, corpinfo.CityID.ToString());
                ddlCity_SelectedIndexChanged(null, null);
                SetSelectedByValue(ddlDistrict, corpinfo.DistrictID.ToString());

                SetSelectedByValue(rblIsProcess, corpinfo.IsProcess.ToString());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CorporationInfo entity = new CorporationInfo();
            if (Corporation != null) entity = Corporation;
            else entity = Corporations.Instance.GetModel(DataConvert.SafeInt(ddlCorporation.SelectedValue), true);
            FillData(entity);

            Corporations.Instance.Update(entity);
            Corporations.Instance.ReloadCorporationListCache();
            if (Corporation != null) Corporation = entity;

            WriteMessage("/message.aspx", "系统提示", "数据保存成功！", "", "/common/basesetting.aspx");
        }
    }
}