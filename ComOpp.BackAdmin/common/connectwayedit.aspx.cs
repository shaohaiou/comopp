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
    public partial class connectwayedit : AdminBase
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

        public ConnectWayInfo CurrentConnectWay { get; set; }

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
                CurrentConnectWay = ConnectWays.Instance.GetModel(id);
                if (CurrentConnectWay == null)
                    WriteMessage("/message.aspx", "系统提示", "无效追踪方式！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentConnectWay.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentConnectWay.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentConnectWay.Name;
                }
            }
        }

        private void FillData(ConnectWayInfo entity)
        {
            entity.Name = txtName.Value;
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ConnectWayInfo entity = new ConnectWayInfo();
            int id = GetInt("id");

            if (id > 0)
            {
                entity = ConnectWays.Instance.GetModel(id, true);
                FillData(entity);
                ConnectWays.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                ConnectWays.Instance.Add(entity);
            }

            ConnectWays.Instance.ReloadConnectWayListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "connectwaymg.aspx" : FromUrl);
        }
    }
}