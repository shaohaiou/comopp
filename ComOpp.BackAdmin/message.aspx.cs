using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin
{
    public partial class message : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        public string ReUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ReUrl = System.Web.HttpUtility.UrlDecode(HXContext.Current.ReturnUrl);
                lTitle.Text = HXContext.Current.MessageTitle;
                lContent.Text = HXContext.Current.Message;
                hyreturn.NavigateUrl = System.Web.HttpUtility.UrlDecode(HXContext.Current.ReturnUrl);
            }
        }
    }
}