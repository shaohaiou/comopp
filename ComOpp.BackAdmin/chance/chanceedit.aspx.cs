using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;
using System.Data;
using System.Text.RegularExpressions;

namespace ComOpp.BackAdmin.chance
{
    public partial class chanceedit : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        private CustomerInfo currentcustomerinfo = null;
        protected CustomerInfo CurrentCustomerInfo 
        {
            get
            {
                if (currentcustomerinfo == null && GetInt("id") > 0)
                    currentcustomerinfo =  Customers.Instance.GetCustomerByID(GetInt("id"));
                return currentcustomerinfo;
            }
        }

        protected int CarBrandID
        {
            get
            {
                if (CurrentCustomerInfo != null)
                {
                    if (Action == "提交到提车|回访" || Action == "编辑提车|回访" || Action == "编辑预订|成交")
                        return CurrentCustomerInfo.SbuyCarBrandID;
                    return CurrentCustomerInfo.IbuyCarBrandID;
                }
                return Corporation == null ? 0 : Corporation.BrandID;
            }
        }

        protected int CarSeriesID
        {
            get
            {
                if (CurrentCustomerInfo != null)
                {
                    if (Action == "提交到提车|回访" || Action == "编辑提车|回访" || Action == "编辑预订|成交")
                        return CurrentCustomerInfo.SbuyCarSeriesID;
                    return CurrentCustomerInfo.IbuyCarSeriesID;
                }
                return 0;
            }
        }

        protected int CarModelID
        {
            get
            {
                if (CurrentCustomerInfo != null)
                {
                    if (Action == "提交到提车|回访" || Action == "编辑提车|回访" || Action == "编辑预订|成交")
                        return CurrentCustomerInfo.SbuyCarModelID;
                    return CurrentCustomerInfo.IbuyCarModelID;
                }
                return 0;
            }
        }

        protected int ProvinceID
        {
            get
            {
                if (CurrentCustomerInfo != null)
                    return CurrentCustomerInfo.ProvinceID;
                return Corporation == null ? 0 : Corporation.ProvinceID;
            }
        }

        protected int CityID
        {
            get
            {
                if (CurrentCustomerInfo != null)
                    return CurrentCustomerInfo.CityID;
                return Corporation == null ? 0 : Corporation.CityID;
            }
        }

        protected int DistrictID
        {
            get
            {
                if (CurrentCustomerInfo != null)
                    return CurrentCustomerInfo.DistrictID;
                return Corporation == null ? 0 : Corporation.DistrictID;
            }
        }

        protected string SubmitText
        {
            get
            { 
                string result = "确定提交";
                if (GetInt("id") > 0 && GetInt("state") > 0)
                {
                    result = "提交至" + EnumExtensions.GetDescription<CustomerStatus>(Enum.GetName(typeof(CustomerStatus), GetInt("state")));
                }

                return result;
            }
        }

        private string action = string.Empty;
        protected string Action
        {
            get
            {
                if (string.IsNullOrEmpty(action))
                {
                    int id = GetInt("id");
                    int state = GetInt("state");
                    if (id > 0)
                    {
                        if (state == (int)CustomerStatus.导入_集客)
                            action = "提交到导入|集客";
                        else if (state == (int)CustomerStatus.清洗_邀约)
                            action = "提交到清洗|邀约";
                        else if (state == (int)CustomerStatus.到店_洽谈)
                            action = "提交到到店|洽谈";
                        else if (state == 0 && CurrentCustomerInfo.CustomerStatus == (int)CustomerStatus.到店_洽谈)
                            action = "编辑到店|洽谈";
                        else if (state == (int)CustomerStatus.追踪_促成)
                            action = "提交到追踪|促成";
                        else if (state == (int)CustomerStatus.预订_成交)
                            action = "提交到预订|成交";
                        else if (state == 0 && CurrentCustomerInfo.CustomerStatus == (int)CustomerStatus.预订_成交)
                            action = "编辑预订|成交";
                        else if (state == (int)CustomerStatus.提车_回访)
                            action = "提交到提车|回访";
                        else if (state == 0 && CurrentCustomerInfo.CustomerStatus == (int)CustomerStatus.提车_回访)
                            action = "编辑提车|回访";
                        else if (state == (int)CustomerStatus.潜客_战败)
                            action = "提交到战败";
                        else if (state == (int)CustomerStatus.转出待审)
                            action = "提交到转出|待审";
                        else if (state == (int)CustomerStatus.潜客_转出)
                            action = "提交到潜客|转出";
                    }
                    else
                    {
                        if (state == (int)CustomerStatus.到店_洽谈)
                            action = "新增到店|洽谈";
                    }
                }

                return action;
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
            rptInfoType.DataSource = InfoTypes.Instance.GetListByCorpid(Corporation == null ? 0 : Corporation.ID,true);
            rptInfoType.DataBind();

            rptInfoSource.DataSource = InfoSources.Instance.GetListByCorpid(Corporation == null ? 0 : Corporation.ID, true);
            rptInfoSource.DataBind();

            rptPaymentway.DataSource = PaymentWays.Instance.GetListByCorpid(Corporation == null ? 0 : Corporation.ID, true);
            rptPaymentway.DataBind();

            rptIbuytime.DataSource = Ibuytimes.Instance.GetListByCorpid(Corporation == null ? 0 : Corporation.ID, true);
            rptIbuytime.DataBind();

            rptTracktag.DataSource = Tracktags.Instance.GetListByCorpid(Corporation == null ? 0 : Corporation.ID, true);
            rptTracktag.DataBind();

            List<AdminInfo> list = Admins.Instance.GetUsers();
            if (Corporation != null)
                list = list.FindAll(a=>a.CorporationID == Corporation.ID);
            list = list.OrderBy(l=>l.UserRole).ThenBy(l=>l.Realname).ToList();
            rptOwner.DataSource = list;
            rptOwner.DataBind();

            DataTable tblCustomerStatus = EnumExtensions.ToTable<CustomerStatus>();
            rptCustomerStatus.DataSource = tblCustomerStatus.DefaultView; 
            rptCustomerStatus.DataBind();

            rptGiveupCause.DataSource = GiveupCauses.Instance.GetListByCorpid(Corporation == null ? 0 : Corporation.ID, true);
            rptGiveupCause.DataBind();

            if (CurrentCustomerInfo != null)
            {
                List<TracktagInfo> tags = Tracktags.Instance.GetList(true);
                string[] ids = CurrentCustomerInfo.TracktagID.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                rpttags.DataSource = tags.FindAll(t => ids.Contains(t.ID.ToString()));
                rpttags.DataBind();
            }
        }

        private void FillData(CustomerInfo entity)
        {
            entity.Name = Request["form[uname]"];
            entity.Phone = Request["form[phone]"];
            entity.BackupPhone = Request["form[sparephone]"];
            entity.ProvinceID = DataConvert.SafeInt(Request["form[province]"]);
            entity.CityID = DataConvert.SafeInt(Request["form[city]"]);
            entity.DistrictID = DataConvert.SafeInt(Request["form[district]"]);
            entity.Address = Request["form[address]"];
            entity.WeixinAccount = Request["form[weixin]"];
            entity.InfoTypeID = DataConvert.SafeInt(Request["form[infotype]"]);
            entity.InfoSourceID = DataConvert.SafeInt(Request["form[infosource]"]);
            entity.PaymentWayID = DataConvert.SafeInt(Request["form[paymentway]"]);
            if (Action == "提交到预订|成交" || Action == "编辑预订|成交" || Action == "提交到提车|回访" || Action == "编辑提车|回访")
            {
                entity.SbuyCarBrandID = DataConvert.SafeInt(Request["form[brand]"]);
                entity.SbuyCarSeriesID = DataConvert.SafeInt(Request["form[series]"]);
                entity.SbuyCarModelID = DataConvert.SafeInt(Request["form[spec]"]);
            }
            else
            {
                entity.IbuyCarBrandID = DataConvert.SafeInt(Request["form[brand]"]);
                entity.IbuyCarSeriesID = DataConvert.SafeInt(Request["form[series]"]);
                entity.IbuyCarModelID = DataConvert.SafeInt(Request["form[spec]"]);
            }
            entity.IbuyTimeID = DataConvert.SafeInt(Request["form[ibuytime]"]);
            entity.QuotedpriceInfo = Request["form[price]"];
            entity.PromotionInfo = Request["form[slogan]"];
            entity.RemarkInfo = Request["form[content]"];
            entity.OwnerID = DataConvert.SafeInt(Request["form[owneruid]"]);
            entity.Owner = new Regex("\\([\\s\\S]+\\)").Replace(Request["form[owner]"],string.Empty);
            entity.CustomerSex = DataConvert.SafeInt(Request["form[sex]"]);
            entity.TracktagID = Request["form[tracktag]"];
            entity.Tracktag = Request["form[tracktagname]"];
            entity.CustomerStatus = DataConvert.SafeInt(Request["form[customerstatus]"]);
            if (GetString("action") == "recover")
            {
                entity.DelState = 0;
                entity.CustomerStatusSource = entity.CustomerStatus;
                entity.CustomerStatus = (int)CustomerStatus.清洗_邀约;
                entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                entity.CheckStatus = 0;
                entity.LurkStatus = 0;
            }
        }

        private void CommitData()
        {
            int result = 0;
            CustomerInfo entity = new CustomerInfo();
            int id = GetInt("id");
            if (id > 0 && CurrentCustomerInfo != null)
            {
                entity = Customers.Instance.GetCustomerByID(id);
                entity.LastUpdateUserID = AdminID;
                entity.LastUpdateUser = Admin.Realname;
                entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                FillData(entity);
                if (Action == "提交到导入|集客")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.导入_集客;
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    entity.CheckStatus = 0;
                    entity.LurkStatus = 0;
                }
                if (Action == "提交到清洗|邀约")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.清洗_邀约;
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    entity.CheckStatus = 0;
                    entity.LurkStatus = 0;
                }
                else if (Action == "提交到到店|洽谈")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.到店_洽谈;
                    entity.ReservationTime = Request["form[invitetime]"];
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else if (Action == "编辑到店|洽谈")
                {
                    entity.VisitTime = Request["form[arrivetime]"];
                    entity.LeaveTime = Request["form[leavetime]"];
                    entity.VisitDuration = DataConvert.SafeInt(Request["form[reception]"]);
                    entity.VisitNumber = DataConvert.SafeInt(Request["form[arrivepeoplenum]"]);
                    if(entity.IsVisit == 0) entity.ReservationTime = Request["form[invitetime]"];
                    entity.IsVisit = 1;
                }
                else if (Action == "提交到追踪|促成")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.追踪_促成;
                    entity.VisitTime = Request["form[arrivetime]"];
                    entity.LeaveTime = Request["form[leavetime]"];
                    entity.VisitDuration = DataConvert.SafeInt(Request["form[reception]"]);
                    entity.VisitNumber = DataConvert.SafeInt(Request["form[arrivepeoplenum]"]);
                    entity.IsVisit = 1;
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else if (Action == "提交到预订|成交")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.预订_成交;
                    entity.PlaceOrderTime = Request["form[endtime]"];
                    entity.KnockdownPrice = Request["form[strikeprice]"];
                    entity.OrderNumber = Request["form[ordernum]"];
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else if (Action == "编辑预订|成交")
                {
                    entity.PlaceOrderTime = Request["form[endtime]"];
                    entity.KnockdownPrice = Request["form[strikeprice]"];
                    entity.OrderNumber = Request["form[ordernum]"];
                }
                else if (Action == "提交到提车|回访")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.提车_回访;
                    entity.PlaceOrderTime = Request["form[endtime]"];
                    entity.KnockdownPrice = Request["form[strikeprice]"];
                    entity.OrderNumber = Request["form[ordernum]"];
                    entity.PicupcarTime = Request["form[delivertime]"];
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else if (Action == "编辑提车|回访")
                {
                    entity.PlaceOrderTime = Request["form[endtime]"];
                    entity.KnockdownPrice = Request["form[strikeprice]"];
                    entity.OrderNumber = Request["form[ordernum]"];
                    entity.PicupcarTime = Request["form[delivertime]"];
                }
                else if (Action == "提交到战败")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.潜客_战败;
                    entity.GiveupCauseID = DataConvert.SafeInt(Request["form[giveupcause]"]);
                    entity.FailureCauseAnalyze = Request["form[failurereason]"];
                    entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    entity.LurkStatus = 1;
                }
                else if (Action == "提交到转出|待审")
                {
                    entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.转出待审;
                    entity.CheckStatus = 1;
                }
                else if (Action == "提交到潜客|转出")
                {
                    if (entity.CustomerStatus != (int)CustomerStatus.转出待审)
                        entity.CustomerStatusSource = entity.CustomerStatus;
                    entity.CustomerStatus = (int)CustomerStatus.潜客_转出;
                    entity.LurkStatus = 1;
                    entity.CheckStatus = 0;
                }

                result = Customers.Instance.Update(entity);
            }
            else if(id == 0)
            {
                entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                entity.CreateUserID = AdminID;
                entity.CreateUser = Admin.Realname;
                entity.LastUpdateUserID = AdminID;
                entity.LastUpdateUser = Admin.Realname;
                entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                entity.CorporationID = Admin.CorporationID;
                entity.SystemRemark = DateTime.Today.ToString("yyyy年MM月dd日") + "线索录入";
                FillData(entity);

                if (Action == "新增到店|洽谈")
                {
                    entity.VisitTime = Request["form[arrivetime]"];
                    entity.VisitNumber = DataConvert.SafeInt(Request["form[arrivepeoplenum]"]);
                    entity.IsVisit = 1;
                }

                result = Customers.Instance.Add(entity);
            }

            Response.Clear();
            Response.Write(result == 0 ? "n" : "y");
            Response.End();
        }

        protected string SetCustomerStatusSel(object value)
        {
            string result = string.Empty;

            if (CurrentCustomerInfo != null && CurrentCustomerInfo.CustomerStatus == DataConvert.SafeInt(value))
                return "selected=\"true\"";
            else if (CurrentCustomerInfo == null && GetInt("state") > 0 && GetInt("state") == DataConvert.SafeInt(value))
                return "selected=\"true\"";
            else if (CurrentCustomerInfo == null && GetInt("state") == 0 && (int)CustomerStatus.清洗_邀约 == DataConvert.SafeInt(value))
                return "selected=\"true\"";

            return result;
        }

        protected string SetOwnerSel(object value)
        {
            string result = string.Empty;

            if (CurrentCustomerInfo != null)
            {
                if(CurrentCustomerInfo.OwnerID == DataConvert.SafeInt(value))
                    return "selected=\"true\"";
            }
            else if (AdminID == DataConvert.SafeInt(value))
                return "selected=\"true\"";

            return result;
        }

        protected string SetInfoTypeSel(object value)
        {
            return (CurrentCustomerInfo != null && CurrentCustomerInfo.InfoTypeID == DataConvert.SafeInt(value)) ? "selected=\"true\"" : string.Empty;
        }

        protected string SetInfoSourceSel(object value)
        {
            return (CurrentCustomerInfo != null && CurrentCustomerInfo.InfoSourceID == DataConvert.SafeInt(value)) ? "selected=\"true\"" : string.Empty;
        }

        protected string SetPaymentwaySel(object value)
        {
            return (CurrentCustomerInfo != null && CurrentCustomerInfo.PaymentWayID == DataConvert.SafeInt(value)) ? "selected=\"true\"" : string.Empty;
        }

        protected string SetIbuytimeSel(object value)
        {
            return (CurrentCustomerInfo != null && CurrentCustomerInfo.IbuyTimeID == DataConvert.SafeInt(value)) ? "selected=\"true\"" : string.Empty;
        }
    }
}