using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Text;

namespace ComOpp.BackAdmin.user
{
    public partial class powergroupedit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "账户组"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        public PowerGroupInfo CurrentPowerGroup { get; set; }

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
                CurrentPowerGroup = PowerGroups.Instance.GetModel(id, true);
                if (CurrentPowerGroup == null)
                    WriteMessage("/message.aspx", "系统提示", "无效账户组！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentPowerGroup.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtGroupName.Value = CurrentPowerGroup.GroupName;
                    hdnGroupPower.Value = CurrentPowerGroup.GroupPower;
                    hdnCanViewGroup.Value = CurrentPowerGroup.CanviewGroupIds;
                }
            }
            rptData.DataSource = Modules.Instance.GetList(true).FindAll(m => m.ParentName == "");
            rptData.DataBind();
        }

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ModuleInfo m = (ModuleInfo)e.Item.DataItem;
                Repeater rptModules = (Repeater)e.Item.FindControl("rptModules");
                rptModules.DataSource = Modules.Instance.GetList(true).FindAll(l => l.ParentName == m.ModuleName);
                rptModules.DataBind();
            }
        }

        private void FillData(PowerGroupInfo entity)
        {
            entity.GroupName = txtGroupName.Value;
            entity.CorporationID = GetInt("corpid");
            entity.LastUpdateTime = DateTime.Now;
            entity.GroupPower = hdnGroupPower.Value;
            entity.CanviewGroupIds = hdnCanViewGroup.Value;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            PowerGroupInfo entity = new PowerGroupInfo();
            int id = GetInt("id");
            if (id > 0) entity = PowerGroups.Instance.GetModel(id, true);
            FillData(entity);

            if (id > 0) PowerGroups.Instance.Update(entity);
            else PowerGroups.Instance.Add(entity);

            PowerGroups.Instance.ReloadPowerGroupListCache();
            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "powergroupmg.aspx" : FromUrl);
        }

        protected string GetPowerStatus(object mn)
        {
            if (CurrentPowerGroup != null && CheckGroupPower(CurrentPowerGroup.GroupPower, mn.ToString()))
            {
                return "checked=\"checked\"";
            }
            return string.Empty;
        }

        protected string GetCanviewPowerGroupHtml(object mn)
        {
            StringBuilder strb = new StringBuilder();
            if (mn.ToString() == "商机管理")
            {
                strb.AppendLine("<div style=\"font-weight:normal\">");
                strb.AppendLine("<label style=\"cursor:pointer\"><input type=\"checkbox\" id=\"cbxcanview\" " + ((CurrentPowerGroup == null || string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds)) ? string.Empty : "checked=\"checked\"") + " />查看其他人的线索</label>");
                strb.AppendLine("<div class=\"archivegroup\" style=\"font-weight:normal;" + ((CurrentPowerGroup == null || string.IsNullOrEmpty(CurrentPowerGroup.CanviewGroupIds)) ? "display:none" : string.Empty) + "\">");
                strb.AppendLine("<label style=\"color:red\">授权查看分组：</label>");

                List<PowerGroupInfo> list = PowerGroups.Instance.GetListByCorporation(CurrentPowerGroup == null ? GetInt("corpid") : CurrentPowerGroup.CorporationID, true);
                string[] currentpowers = CurrentPowerGroup == null ? new string[0] : CurrentPowerGroup.CanviewGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (PowerGroupInfo p in list)
                {
                    strb.AppendLine("<label><input type=\"checkbox\" value=\"" + p.ID + "\" " + (currentpowers.Contains(p.ID.ToString()) ? "checked=\"checked\"" : string.Empty) + " class=\"canviewgroup\" />" + p.GroupName + "</label>");
                }
                strb.AppendLine("</div></div>");
            }
            return strb.ToString();
        }
    }
}