using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.admin
{
    public partial class adminedit : AdminBase
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

        public AdminInfo CurrentAdmin { get; set; }

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
            ddlCorp.DataSource = Corporations.Instance.GetList(true);
            ddlCorp.DataTextField = "Name";
            ddlCorp.DataValueField = "ID";
            ddlCorp.DataBind();
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentAdmin = Admins.Instance.GetAdmin(id);

                txtUserName.Value = CurrentAdmin.UserName;
                txtUserName.Attributes["readonly"] = "true";
                txtPassword.Value = CurrentAdmin.PasswordText;
                txtRealname.Value = CurrentAdmin.Realname;
                txtMobile.Value = CurrentAdmin.Mobile;
                txtQQ.Value = CurrentAdmin.QQ;
                SetSelectedByValue(rblSex,CurrentAdmin.Sex.ToString());
                SetSelectedByValue(rblState, CurrentAdmin.State);
                SetSelectedByValue(ddlCorp, CurrentAdmin.CorporationID.ToString());
            }
        }

        private void FillData(AdminInfo entity)
        {
            entity.UserName = txtUserName.Value;
            entity.UserRole = UserRoleType.系统管理员;
            entity.Password = EncryptString.MD5(txtPassword.Value);
            entity.PasswordText = txtPassword.Value;
            entity.Realname = txtRealname.Value;
            entity.Mobile = txtMobile.Value;
            entity.QQ = txtQQ.Value;
            entity.Sex = DataConvert.SafeInt(rblSex.SelectedValue);
            entity.LastLoginIP = string.Empty;
            if (!entity.Administrator)
            {
                entity.CorporationID = DataConvert.SafeInt(ddlCorp.SelectedValue);
                entity.Corporation = ddlCorp.SelectedItem.Text;
            }
            entity.State = rblState.SelectedValue;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AdminInfo entity = new AdminInfo();
            int id = GetInt("id");
            if (id > 0) entity = Admins.Instance.GetAdmin(id);
            FillData(entity);

            if (id > 0) Admins.Instance.UpdateAdmin(entity);
            else Admins.Instance.AddAdmin(entity);

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "adminmg.aspx" : FromUrl);
        }
    }
}