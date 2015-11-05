using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.user
{
    public partial class useredit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "账户管理"))
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
            if (Admin.Administrator)
            {
                ddlCorporation.DataSource = Corporations.Instance.GetList(true);
                ddlCorporation.DataTextField = "Name";
                ddlCorporation.DataValueField = "ID";
                ddlCorporation.DataBind();
            }
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentAdmin = Admins.Instance.GetAdmin(id);
                if (CurrentAdmin == null)
                    WriteMessage("/message.aspx", "系统提示", "无效账户！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentAdmin.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtUserName.Value = CurrentAdmin.UserName;
                    txtUserName.Attributes["readonly"] = "true";
                    txtPassword.Value = CurrentAdmin.PasswordText;
                    txtRealname.Value = CurrentAdmin.Realname;
                    txtMobile.Value = CurrentAdmin.Mobile;
                    txtQQ.Value = CurrentAdmin.QQ;
                    SetSelectedByValue(rblSex, CurrentAdmin.Sex.ToString());
                    SetSelectedByValue(rblState, CurrentAdmin.State);
                    if (Admin.Administrator)
                    {
                        SetSelectedByValue(ddlCorporation, CurrentAdmin.CorporationID.ToString());
                        CorporationChanged(DataConvert.SafeInt(ddlCorporation.SelectedValue));
                    }
                    else
                    {
                        CorporationChanged(DataConvert.SafeInt(Admin.CorporationID));
                    }
                    SetSelectedByValue(ddlPowerGroupEdit, CurrentAdmin.PowerGroupID.ToString());
                    hdnPowerGroup.Value = CurrentAdmin.PowerGroupID.ToString();
                }
            }
            else
            {
                if (!Admin.Administrator)
                    CorporationChanged(DataConvert.SafeInt(Admin.CorporationID));
                else
                    CorporationChanged(DataConvert.SafeInt(ddlCorporation.SelectedValue));
                hdnPowerGroup.Value = ddlPowerGroupEdit.SelectedValue;
            }
        }

        private void FillData(AdminInfo entity)
        {
            entity.UserName = txtUserName.Value;
            entity.Password = EncryptString.MD5(txtPassword.Value);
            entity.PasswordText = txtPassword.Value;
            entity.Realname = txtRealname.Value;
            entity.Mobile = txtMobile.Value;
            entity.QQ = txtQQ.Value;
            entity.Sex = DataConvert.SafeInt(rblSex.SelectedValue);
            entity.LastLoginIP = string.Empty;
            entity.State = rblState.SelectedValue;
            if (entity.UserRole == UserRoleType.普通用户)
                entity.PowerGroupID = DataConvert.SafeInt(hdnPowerGroup.Value);

            if (Admin.Administrator)
            {
                entity.CorporationID = DataConvert.SafeInt(ddlCorporation.SelectedValue);
                entity.Corporation = ddlCorporation.SelectedItem.Text;
            }
            else
            {
                entity.CorporationID = Admin.CorporationID;
                entity.Corporation = Admin.Corporation;
            }
        }

        private void CorporationChanged(int corpid)
        {
            if (CurrentAdmin == null || CurrentAdmin.UserRole == UserRoleType.普通用户)
            {
                List<PowerGroupInfo> plist = PowerGroups.Instance.GetList(true);
                plist = plist.FindAll(l => l.CorporationID == corpid);
                ddlPowerGroupEdit.DataSource = plist;
                ddlPowerGroupEdit.DataTextField = "GroupName";
                ddlPowerGroupEdit.DataValueField = "ID";
                ddlPowerGroupEdit.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AdminInfo entity = new AdminInfo()
            {
                UserRole = UserRoleType.普通用户
            };
            int id = GetInt("id");
            if (id > 0) entity = Admins.Instance.GetAdmin(id);
            FillData(entity);

            if (id > 0) Admins.Instance.UpdateAdmin(entity);
            else
            {
                Admins.Instance.AddAdmin(entity);
                PowerGroups.Instance.ReloadPowerGroupListCache();
            }

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "adminmg.aspx" : FromUrl);
        }
    }
}