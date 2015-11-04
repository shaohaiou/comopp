using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.common
{
    public partial class talkpage : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        private TalkInfo currenttalk = null;
        protected TalkInfo CurrentTalk
        {
            get
            {
                if (currenttalk == null)
                {
                    int id = GetInt("id");
                    if (id > 0)
                        currenttalk = Talks.Instance.GetModel(id,true);
                }

                return currenttalk;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}