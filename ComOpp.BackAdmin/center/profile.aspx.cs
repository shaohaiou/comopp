using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.center
{
    public partial class profile : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            txtMobile.Value = Admin.Mobile;
            txtQQ.Value = Admin.QQ;
            txtRealname.Value = Admin.Realname;
            SetSelectedByValue(rblSex, Admin.Sex.ToString());
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                AdminInfo admin = Admins.Instance.GetAdmin(AdminID);
                admin.Mobile = txtMobile.Value;
                admin.Realname = txtRealname.Value;
                admin.QQ = txtQQ.Value;
                admin.Sex = DataConvert.SafeInt(rblSex.SelectedValue);

                Admins.Instance.UpdateAdmin(admin);

                Admin = admin;

                WriteMessage("/message.aspx", "系统提示", "数据保存成功！", "", "/center/profile.aspx");
            }
        }
    }
}