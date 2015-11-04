using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.support
{
    public partial class talkedit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "话术管理"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        public TalkInfo CurrentTalk { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            int id = GetInt("id");
            if (id > 0)
            {
                CurrentTalk = Talks.Instance.GetModel(id);
                if (CurrentTalk == null)
                    WriteMessage("/message.aspx", "系统提示", "无效话术信息！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentTalk.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtTitle.Value = CurrentTalk.Title;
                    txtTag.Value = CurrentTalk.Tag;
                }
            }
        }

        private void FillData(TalkInfo entity)
        {
            entity.Title = txtTitle.Value;
            entity.Tag = txtTag.Value;
            entity.Content = GetString("txtContent");
            entity.CorporationID = GetInt("corpid");
            entity.Realname = Admin.Realname;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            TalkInfo entity = new TalkInfo();
            int id = GetInt("id");
            if (id > 0) entity = Talks.Instance.GetModel(id, true);
            FillData(entity);

            if (id > 0) Talks.Instance.Update(entity);
            else Talks.Instance.Add(entity);

            Talks.Instance.ReloadTalkListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "talkmg.aspx" : FromUrl);
        }
    }
}