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
    public partial class infotypeedit : AdminBase
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

        public InfoTypeInfo CurrentInfoType { get; set; }

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
                CurrentInfoType = InfoTypes.Instance.GetModel(id);
                if (CurrentInfoType == null)
                    WriteMessage("/message.aspx", "系统提示", "无效信息类型！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentInfoType.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    List<CustomerLevelInfo> cllist = CustomerLevels.Instance.GetListByCorpid(CurrentInfoType.CorporationID, true);
                    cblLocklevel.DataSource = cllist;
                    cblLocklevel.DataTextField = "Name";
                    cblLocklevel.DataValueField = "ID";
                    cblLocklevel.DataBind();
                    foreach (ListItem item in cblLocklevel.Items)
                    {
                        if (CurrentInfoType.Locklevel.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Contains(item.Value))
                            item.Selected = true;
                    }

                    txtName.Value = CurrentInfoType.Name;
                    SetSelectedByValue(rblLocked, CurrentInfoType.Locked.ToString());
                    txtLockday.Value = CurrentInfoType.Lockday.ToString();
                    if (!Admin.Administrator && CurrentInfoType.DataLevel == 0)
                        txtName.Attributes["readonly"] = "true";
                }
            }
            else
            {
                int corpid = GetInt("corpid");

                if (corpid > 0)
                {
                    List<CustomerLevelInfo> cllist = CustomerLevels.Instance.GetListByCorpid(corpid, true);
                    cblLocklevel.DataSource = cllist;
                    cblLocklevel.DataTextField = "Name";
                    cblLocklevel.DataValueField = "ID";
                    cblLocklevel.DataBind();
                }
            }
        }

        private void FillData(InfoTypeInfo entity)
        {
            entity.Name = txtName.Value;
            entity.Locked = DataConvert.SafeInt(rblLocked.SelectedValue);
            entity.Lockday = DataConvert.SafeInt(txtLockday.Value);
            entity.CorporationID = GetInt("corpid");
            List<string> locklevel = new List<string>();
            List<string> locklevelname = new List<string>();
            foreach (ListItem item in cblLocklevel.Items)
            {
                if (item.Selected)
                {
                    locklevel.Add(item.Value);
                    locklevelname.Add(item.Text);
                }
            }
            entity.Locklevel = string.Join("|", locklevel);
            entity.LocklevelName = string.Join(",", locklevelname);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            InfoTypeInfo entity = new InfoTypeInfo();
            int id = GetInt("id");

            if (id > 0)
            {
                entity = InfoTypes.Instance.GetModel(id, true);
                FillData(entity);
                InfoTypes.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                InfoTypes.Instance.Add(entity);
            }

            InfoTypes.Instance.ReloadInfoTypeListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "infotypemg.aspx" : FromUrl);
        }

        protected string GetLocklevelSelecteState(string id)
        {
            if (CurrentInfoType != null)
            {
                if (CurrentInfoType.Locklevel.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Contains(id))
                    return "checked=\"checked\"";
            }
            return string.Empty;
        }
    }
}