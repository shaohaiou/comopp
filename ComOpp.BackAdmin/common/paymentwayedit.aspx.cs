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
    public partial class paymentwayedit : AdminBase
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

        public PaymentWayInfo CurrentPaymentWay { get; set; }

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
                CurrentPaymentWay = PaymentWays.Instance.GetModel(id);
                if (CurrentPaymentWay == null)
                    WriteMessage("/message.aspx", "系统提示", "无效支付方式！", "", "/index.aspx");
                else
                {
                    if (!Admin.Administrator && Admin.CorporationID != CurrentPaymentWay.CorporationID)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
                    if (!Admin.Administrator && CurrentPaymentWay.DataLevel == 0)
                        WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");

                    txtName.Value = CurrentPaymentWay.Name;
                }
            }
        }

        private void FillData(PaymentWayInfo entity)
        {
            entity.Name = txtName.Value;
            entity.CorporationID = GetInt("corpid");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            PaymentWayInfo entity = new PaymentWayInfo();
            int id = GetInt("id");
            if (id > 0)
            {
                entity = PaymentWays.Instance.GetModel(id, true);
                FillData(entity);
                PaymentWays.Instance.Update(entity);
            }
            else
            {
                FillData(entity);
                entity.DataLevel = GetInt("corpid") == 0 ? 0 : 1;
                PaymentWays.Instance.Add(entity);
            }

            PaymentWays.Instance.ReloadPaymentWayListCache();

            Response.Redirect(string.IsNullOrEmpty(FromUrl) ? "paymentwaymg.aspx" : FromUrl);
        }
    }
}