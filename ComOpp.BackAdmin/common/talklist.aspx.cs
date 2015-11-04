using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using System.Text;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.common
{
    public partial class talklist : AdminBase
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

        protected string GetTalklist()
        {
            StringBuilder strb = new StringBuilder();

            if (Corporation != null)
            {
                string keywords = GetString("keywords");
                List<TalkInfo> list = Talks.Instance.GetListBycorpid(Corporation.ID);
                if (!string.IsNullOrEmpty(keywords))
                    list = list.FindAll(l=>l.Content.Contains(keywords) || l.Tag.Contains(keywords));
                foreach (TalkInfo t in list)
                {
                    strb.AppendLine("<div class=\"ask\"><a href=\"common/talkpage.aspx?id=" + t.ID + "\">" + t.Title + "</a>");
                    strb.AppendLine("<p>" + StrHelper.GetFuzzyChar(StrHelper.GetHtml(t.Content),50) + "</p></div>");
                }
            }

            return strb.ToString();
        }
    }
}