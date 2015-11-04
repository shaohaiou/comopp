using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Text;

namespace ComOpp.BackAdmin.center
{
    public partial class safe : AdminBase
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

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool result = Admins.Instance.ChangePassword(HXContext.Current.AdminUserID, EncryptString.MD5(txtOldpassword.Value), EncryptString.MD5(txtPassword.Value));
                if (!result)
                {
                    WriteMessage("/message.aspx", "系统提示", "原密码错误！", "", "/center/safe.aspx");
                }
                else
                {
                    AdminInfo admin = Admins.Instance.GetAdmin(AdminID);
                    admin.PasswordText = txtPassword.Value;
                    Admins.Instance.UpdateAdmin(admin);
                    WriteMessage("/message.aspx", "系统提示", "密码修改成功,请使用新密码登录！", "", "/logout.aspx");
                }
            }
        }
    }
}