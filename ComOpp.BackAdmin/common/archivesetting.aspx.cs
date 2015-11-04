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
    public partial class archivesetting : AdminBase
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

        private CorporationInfo currentcorporation = null;
        public CorporationInfo CurrentCorporation
        {
            get
            {
                if (currentcorporation == null)
                {
                    if (GetInt("corpid") > 0)
                        currentcorporation = Corporations.Instance.GetModel(GetInt("corpid"));
                    else if (Corporation != null)
                        currentcorporation = Corporation;
                    else
                        currentcorporation = Corporations.Instance.GetList(true).First();
                }

                return currentcorporation;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindControler();
                LoadData();
            }
        }

        private void BindControler()
        {
            ddlCorporation.DataSource = Corporations.Instance.GetList(true);
            ddlCorporation.DataTextField = "Name";
            ddlCorporation.DataValueField = "ID";
            ddlCorporation.DataBind();

            SetSelectedByValue(ddlCorporation, CurrentCorporation.ID.ToString());

            cblOffcustomerlevel.DataSource = CustomerLevels.Instance.GetListByCorpid(CurrentCorporation.ID, true).OrderBy(l=>l.Sort);
            cblOffcustomerlevel.DataTextField = "Name";
            cblOffcustomerlevel.DataValueField = "ID";
            cblOffcustomerlevel.DataBind();

            cblForcedoutdaylevel.DataSource = CustomerLevels.Instance.GetListByCorpid(CurrentCorporation.ID, true).OrderBy(l => l.Sort);
            cblForcedoutdaylevel.DataTextField = "Name";
            cblForcedoutdaylevel.DataValueField = "ID";
            cblForcedoutdaylevel.DataBind();

        }

        private void LoadData()
        {
            cbxTrackmove.Checked = CurrentCorporation.Trackmove == 1;
            cbxMovecheck.Checked = CurrentCorporation.Movecheck == 1;
            txtForcedoffday.Value = CurrentCorporation.Forcedoffday.ToString();

            string[] offcustomerlevels = CurrentCorporation.Offcustomerlevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (ListItem item in cblOffcustomerlevel.Items)
            {
                if (offcustomerlevels.Contains(item.Value))
                    item.Selected = true;
            }

            txtVoluntaryoffday.Value = CurrentCorporation.Voluntaryoffday.ToString();
            cbxOffcheck.Checked = CurrentCorporation.Offcheck == 1;
            txtForcedoutday.Value = CurrentCorporation.Forcedoutday.ToString();

            string[] forcedoutdaylevels = CurrentCorporation.Forcedoutdaylevel.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (ListItem item in cblForcedoutdaylevel.Items)
            {
                if (forcedoutdaylevels.Contains(item.Value))
                    item.Selected = true;
            }

            txtVoluntaryoutday.Value = CurrentCorporation.Voluntaryoutday.ToString();
            cbxOutcheck.Checked = CurrentCorporation.Outcheck == 1;
        }

        private void FillData(CorporationInfo entity)
        {
            entity.Trackmove = cbxTrackmove.Checked ? 1 : 0;
            entity.Movecheck = cbxMovecheck.Checked ? 1 : 0;
            entity.Forcedoffday = DataConvert.SafeInt(txtForcedoffday.Value);

            List<string> offcustomerlevels = new List<string>();
            foreach (ListItem item in cblOffcustomerlevel.Items)
            {
                if (item.Selected)
                    offcustomerlevels.Add(item.Value);
            }
            entity.Offcustomerlevel = string.Join(",", offcustomerlevels);

            entity.Voluntaryoffday = DataConvert.SafeInt(txtVoluntaryoffday.Value);
            entity.Offcheck = cbxOffcheck.Checked ? 1 : 0;
            entity.Forcedoutday = DataConvert.SafeInt(txtForcedoutday.Value);

            List<string> forcedoutdaylevels = new List<string>();
            foreach (ListItem item in cblForcedoutdaylevel.Items)
            {
                if (item.Selected)
                    forcedoutdaylevels.Add(item.Value);
            }
            entity.Forcedoutdaylevel = string.Join(",", forcedoutdaylevels);

            entity.Voluntaryoutday = DataConvert.SafeInt(txtVoluntaryoutday.Value);
            entity.Outcheck = cbxOutcheck.Checked ? 1 : 0;
        }

        protected void ddlCorporation_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("archivesetting.aspx?corpid=" + ddlCorporation.SelectedValue);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CorporationInfo entity = CurrentCorporation;
            FillData(entity);

            Corporations.Instance.Update(entity);

            Corporations.Instance.ReloadCorporationListCache();

            WriteSuccessMessage("系统提示", "数据保存成功", CurrentUrl);
        }
    }
}