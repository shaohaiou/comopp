﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Data;

namespace ComOpp.BackAdmin.chance
{
    public partial class chancefollowadd : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        protected CustomerInfo CurrentCustomerInfo { get; set; }

        protected string CurrentLevelIntroduce
        {
            get
            {
                if (CurrentCustomerInfo != null && CurrentCustomerInfo.LastCustomerLevelID > 0)
                {
                    CustomerLevelInfo level = CustomerLevels.Instance.GetModel(CurrentCustomerInfo.LastCustomerLevelID, true);
                    if (level != null)
                    {
                        if (level.Alarmday == 0) return "订车客户";
                        if (level.Alarmday <= 3) return "高意向客户";
                        return level.Alarmday + "天内购车";
                    }
                }
                return string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPost())
            {
                BindControlor();
            }
            else
            {
                CommitData();
            }
        }

        private void BindControlor()
        {
            int id = GetInt("id");
            if (id > 0)
                CurrentCustomerInfo = Customers.Instance.GetCustomerByID(id);

            int corpid = CurrentCustomerInfo == null ? Corporation.ID : CurrentCustomerInfo.CorporationID;

            rptConnectway.DataSource = ConnectWays.Instance.GetListByCorpid(corpid, true);
            rptConnectway.DataBind();

            rptCustomerLevel.DataSource = CustomerLevels.Instance.GetListByCorpid(corpid, true);
            rptCustomerLevel.DataBind();

            List<AdminInfo> list = Admins.Instance.GetUsers();
            list = list.OrderBy(l => l.Realname).ToList();
            rptUser.DataSource = list.FindAll(l => l.CorporationID == corpid);
            rptUser.DataBind();
        }

        private void FillData(CustomerConnectRecordInfo entity)
        {
            entity.CorporationID = CurrentCustomerInfo.CorporationID;
            entity.CustomerID = GetInt("id");
            entity.CustomerName = CurrentCustomerInfo.Name;
            entity.CustomerPhone = CurrentCustomerInfo.Phone;
            entity.ConnectUserID = DataConvert.SafeInt(Request["form[uid]"]);
            entity.ConnectUser = Request["form[uname]"];
            entity.FollowTime = Request["form[followtime]"];
            entity.ConnectwayID = DataConvert.SafeInt(Request["form[connectway]"]);
            entity.CustomerLevelID = DataConvert.SafeInt(Request["form[customerlevel]"]);
            entity.ConnectDetail = Request["form[followinfo]"];
            entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private void CommitData()
        {
            int result = 0;

            int id = GetInt("id");
            if (id > 0)
                CurrentCustomerInfo = Customers.Instance.GetCustomerByID(id);

            CustomerConnectRecordInfo entity = new CustomerConnectRecordInfo();
            FillData(entity);
            result = CustomerConnectRecords.Instance.Add(entity);
            if (result > 0)
            {
                CurrentCustomerInfo.LastConnectTime = entity.FollowTime;
                CurrentCustomerInfo.LastConnectDetail = entity.ConnectDetail;
                CurrentCustomerInfo.LastConnectUserID = entity.ConnectUserID;
                CurrentCustomerInfo.LastConnectUser = entity.ConnectUser;
                CurrentCustomerInfo.LastConnectwayID = entity.ConnectwayID;
                CurrentCustomerInfo.LastCustomerLevelID = entity.CustomerLevelID;
                CurrentCustomerInfo.LastUpdateTime = entity.CreateTime;
                CurrentCustomerInfo.LastUpdateUserID = AdminID;
                CurrentCustomerInfo.LastUpdateUser = Admin.Realname;
                CurrentCustomerInfo.ConnectTimes = CurrentCustomerInfo.ConnectTimes + 1;

                Customers.Instance.UpdateCustomerLastConnect(CurrentCustomerInfo);
            }

            Response.Clear();
            Response.Write(result == 0 ? "n" : "y");
            Response.End();
        }

        protected string SetCustomerLevelSel(object value)
        {
            return (CurrentCustomerInfo != null && CurrentCustomerInfo.LastCustomerLevelID == DataConvert.SafeInt(value)) ? "selected=\"true\"" : string.Empty;
        }

        protected string SetCustomerLevelIntroduce(object value)
        {
            int id = DataConvert.SafeInt(value);
            CustomerLevelInfo level = CustomerLevels.Instance.GetModel(id, true);
            if (level != null)
            {
                if (level.Alarmday == 0) return "订车客户";
                if (level.Alarmday <= 3) return "高意向客户";
                return level.Alarmday + "天内购车";
            }
            return string.Empty;
        }

        protected string SetUserSel(object value)
        {
            return (CurrentCustomerInfo != null && CurrentCustomerInfo.OwnerID == DataConvert.SafeInt(value)) ? "selected=\"true\"" : string.Empty;
        }
    }
}