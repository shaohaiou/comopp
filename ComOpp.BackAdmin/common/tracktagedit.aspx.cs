using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.common
{
    public partial class tracktagedit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "基础数据"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        public TracktagInfo CurrentTracktag { get; set; }

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
                CurrentTracktag = Tracktags.Instance.GetModel(id);
                if (CurrentTracktag == null)
                    WriteMessage("/message.aspx", "系统提示", "无效线索标签！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentTracktag.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentTracktag.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentTracktag.Name;
                }
            }
        }

        private void FillData(TracktagInfo entity)
        {
            entity.Name = txtName.Value;
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            TracktagInfo entity = new TracktagInfo();
            int id = GetInt("id");
            if (id > 0)
            {
                entity = Tracktags.Instance.GetModel(id, true);
                FillData(entity);
                Tracktags.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                Tracktags.Instance.Add(entity);
            }

            Tracktags.Instance.ReloadTracktagListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "tracktagmg.aspx" : FromUrl);
        }
    }
}