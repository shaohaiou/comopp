using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Tools;
using ComOpp.Components;
using ComOpp.Tools.Web;

namespace ComOpp.BackAdmin
{
    public partial class login : AdminBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HXContext.Current.AdminCheck)
            {
                Response.Clear();
                Response.Write(string.Format("你已经登录，请返回<a href='{0}'>首页</a>", "index.aspx"));
                Response.End();
            }
        }

        /// <summary>
        /// 登录页面不需要验证
        /// </summary>
        protected override void Check()
        {

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string userName = StrHelper.Trim(txtUserName.Value);
                string password = StrHelper.Trim(txtUserPwd.Value);

                ///用户名，密码，验证码不允许为空
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    int id = Admins.Instance.ValiUser(userName, EncryptString.MD5(password));//验证用户

                    if (id > 0)
                    {
                        AdminInfo admin = Admins.Instance.GetAdmin(id);
                        if (admin.State == "1")
                        {
                            admin.LastLoginIP = WebHelper.GetClientsIP();
                            admin.LastLoginTime = DateTime.Now;
                            Admins.Instance.UpdateAdmin(admin);
                            Session[GlobalKey.SESSION_ADMIN] = admin;
                            ManageCookies.CreateCookie(GlobalKey.SESSION_ADMIN, id.ToString(), true, DateTime.Today.AddDays(1), HXContext.Current.CookieDomain);
                            Response.Redirect("index.aspx");
                        }
                        else
                            lblMsg.Text = "用户状态异常，请联系管理员";
                    }
                    else
                        lblMsg.Text = "用户名或密码错误";
                    Session[GlobalKey.SESSION_ADMIN] = null;
                }
            }
        }
    }
}