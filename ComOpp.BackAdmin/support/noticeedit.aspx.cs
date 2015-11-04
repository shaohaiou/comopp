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
    public partial class noticeedit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "公告通知管理"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        public NoticeInfo CurrentNotice { get; set; }

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
                CurrentNotice = Notices.Instance.GetModel(id);
                if (CurrentNotice == null)
                    WriteMessage("/message.aspx", "系统提示", "无效公告通知！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentNotice.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentNotice.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtTitle.Value = CurrentNotice.Title;
                }
            }
        }

        private void FillData(NoticeInfo entity)
        {
            entity.Title = txtTitle.Value;
            entity.Content = GetString("txtContent");
            entity.CorporationID = GetInt("corpid");
            entity.Realname = Admin.Realname;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            NoticeInfo entity = new NoticeInfo();
            int id = GetInt("id");
            if (id > 0)
            {
                entity = Notices.Instance.GetModel(id, true);
                FillData(entity);
                Notices.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                Notices.Instance.Add(entity);
            }

            Notices.Instance.ReloadNoticeListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "noticemg.aspx" : FromUrl);
        }
    }
}