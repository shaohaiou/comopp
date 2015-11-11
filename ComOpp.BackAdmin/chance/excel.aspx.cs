using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Text;

namespace ComOpp.BackAdmin.chance
{
    public partial class excel : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "线索导出"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(GetInt("corpid") > 0 && !string.IsNullOrEmpty(GetString("ids")))
                    ExportData();
            }
        }

        #region 导出Excel

        static HSSFWorkbook hssfworkbook;

        public void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();
            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "红旭集团";
            hssfworkbook.DocumentSummaryInformation = dsi;
            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "xxx";
            hssfworkbook.SummaryInformation = si;
        }

        private void ExportData()
        {
            int corpid = GetInt("corpid");
            string[] ids = GetString("ids").Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            InitializeWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(0);
            rowHeader.CreateCell(0).SetCellValue("线索状态");
            rowHeader.CreateCell(1).SetCellValue("客户姓名");
            rowHeader.CreateCell(2).SetCellValue("客户电话");
            rowHeader.CreateCell(3).SetCellValue("拟购车系");
            rowHeader.CreateCell(4).SetCellValue("信息类型");
            rowHeader.CreateCell(5).SetCellValue("信息来源");
            rowHeader.CreateCell(6).SetCellValue("性别");
            rowHeader.CreateCell(7).SetCellValue("号码归属地");
            rowHeader.CreateCell(8).SetCellValue("线索拥有者");
            rowHeader.CreateCell(9).SetCellValue("标签");
            rowHeader.CreateCell(10).SetCellValue("拟购品牌");
            rowHeader.CreateCell(11).SetCellValue("拟购车型");
            rowHeader.CreateCell(12).SetCellValue("拟购时间");
            rowHeader.CreateCell(13).SetCellValue("报价");
            rowHeader.CreateCell(14).SetCellValue("促销内容");
            rowHeader.CreateCell(15).SetCellValue("备注");
            rowHeader.CreateCell(16).SetCellValue("追踪级别");
            rowHeader.CreateCell(17).SetCellValue("追踪报警");
            rowHeader.CreateCell(18).SetCellValue("追踪次数");
            rowHeader.CreateCell(19).SetCellValue("追踪方式");
            rowHeader.CreateCell(20).SetCellValue("最后追踪时间");
            rowHeader.CreateCell(21).SetCellValue("最后追踪人");
            rowHeader.CreateCell(22).SetCellValue("追踪情况");
            rowHeader.CreateCell(23).SetCellValue("预约到店时间");
            rowHeader.CreateCell(24).SetCellValue("客户来店时间");
            rowHeader.CreateCell(25).SetCellValue("客户离店时间");
            rowHeader.CreateCell(26).SetCellValue("接待时长");
            rowHeader.CreateCell(27).SetCellValue("来店人数");
            rowHeader.CreateCell(28).SetCellValue("是否到店");
            rowHeader.CreateCell(29).SetCellValue("省份-城市-地区");
            rowHeader.CreateCell(30).SetCellValue("备用电话");
            rowHeader.CreateCell(31).SetCellValue("具体地址");
            rowHeader.CreateCell(32).SetCellValue("微信号");
            rowHeader.CreateCell(33).SetCellValue("最后操作人");
            rowHeader.CreateCell(34).SetCellValue("选购品牌");
            rowHeader.CreateCell(35).SetCellValue("选购车系");
            rowHeader.CreateCell(36).SetCellValue("选购车型");
            rowHeader.CreateCell(37).SetCellValue("订单号");
            rowHeader.CreateCell(38).SetCellValue("成交价");
            rowHeader.CreateCell(39).SetCellValue("战败原因");
            rowHeader.CreateCell(40).SetCellValue("战败原因分析");
            rowHeader.CreateCell(41).SetCellValue("建档时间");
            rowHeader.CreateCell(42).SetCellValue("提交时间");
            rowHeader.CreateCell(43).SetCellValue("市场专员");
            rowHeader.CreateCell(44).SetCellValue("DCC专员");
            rowHeader.CreateCell(45).SetCellValue("展厅专员");
            rowHeader.CreateCell(46).SetCellValue("直销专员");
            rowHeader.CreateCell(47).SetCellValue("自动编号");
            rowHeader.CreateCell(48).SetCellValue("系统备注");


            List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(corpid,true);
            list = list.FindAll(l=>ids.Contains(l.ID.ToString()));

            for (int i = 0; i < list.Count; i++)
            {
                HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(list[i].CustomerStatus == (int)CustomerStatus.潜客_转出 ? (list[i].CustomerStatusSourceName + "(转出)") : list[i].CustomerStatusName);
                row.CreateCell(1).SetCellValue(list[i].Name);
                row.CreateCell(2).SetCellValue(list[i].Phone);
                row.CreateCell(3).SetCellValue(list[i].IbuyCarSeries);
                row.CreateCell(4).SetCellValue(list[i].InfoType);
                row.CreateCell(5).SetCellValue(list[i].InfoSource);
                row.CreateCell(6).SetCellValue(list[i].CustomerSex == 1 ? "男" : (list[i].CustomerSex == 2 ? "女" : "保密"));
                row.CreateCell(7).SetCellValue(list[i].PhoneVest);
                row.CreateCell(8).SetCellValue(list[i].Owner);
                row.CreateCell(9).SetCellValue(list[i].Tracktag);
                row.CreateCell(10).SetCellValue(list[i].IbuyCarBrand);
                row.CreateCell(11).SetCellValue(list[i].IbuyCarModel);
                row.CreateCell(12).SetCellValue(list[i].IbuyTime);
                row.CreateCell(13).SetCellValue(list[i].QuotedpriceInfo);
                row.CreateCell(14).SetCellValue(list[i].PromotionInfo);
                row.CreateCell(15).SetCellValue(list[i].RemarkInfo);
                row.CreateCell(16).SetCellValue(list[i].LastCustomerLevel);
                row.CreateCell(17).SetCellValue(list[i].ConnectAlarm == "0" ? "正常" : (list[i].ConnectAlarm == "1" ? "正常(24小时内超时)" : (list[i].ConnectAlarm == "2" ? "追踪超时" : string.Empty)));
                row.CreateCell(18).SetCellValue(list[i].ConnectTimes);
                row.CreateCell(19).SetCellValue(list[i].LastConnectway);
                row.CreateCell(20).SetCellValue(list[i].LastConnectTime);
                row.CreateCell(21).SetCellValue(list[i].LastConnectUser);
                row.CreateCell(22).SetCellValue(list[i].LastConnectDetail);
                row.CreateCell(23).SetCellValue(list[i].ReservationTime);
                row.CreateCell(24).SetCellValue(list[i].VisitTime);
                row.CreateCell(25).SetCellValue(list[i].LeaveTime);
                row.CreateCell(26).SetCellValue(list[i].IsVisit == 0 ? string.Empty : list[i].VisitDuration.ToString());
                row.CreateCell(27).SetCellValue(list[i].IsVisit == 0 ? string.Empty : list[i].VisitNumber.ToString());
                row.CreateCell(28).SetCellValue(list[i].IsVisit == 0 ? "否" : "是");
                row.CreateCell(29).SetCellValue(string.Format("{0}-{1}-{2}", list[i].Province, list[i].City, list[i].District));
                row.CreateCell(30).SetCellValue(list[i].BackupPhone);
                row.CreateCell(31).SetCellValue(list[i].Address);
                row.CreateCell(32).SetCellValue(list[i].WeixinAccount);
                row.CreateCell(33).SetCellValue(list[i].LastUpdateUser);
                row.CreateCell(34).SetCellValue(list[i].SbuyCarBrand);
                row.CreateCell(35).SetCellValue(list[i].SbuyCarSeries);
                row.CreateCell(36).SetCellValue(list[i].SbuyCarModel);
                row.CreateCell(37).SetCellValue(list[i].OrderNumber);
                row.CreateCell(38).SetCellValue(list[i].KnockdownPrice);
                row.CreateCell(39).SetCellValue(list[i].GiveupCause);
                row.CreateCell(40).SetCellValue(list[i].FailureCauseAnalyze);
                row.CreateCell(41).SetCellValue(list[i].CreateTime);
                row.CreateCell(42).SetCellValue(list[i].PostTime);
                row.CreateCell(43).SetCellValue(list[i].MarketDirector);
                row.CreateCell(44).SetCellValue(list[i].DCCDirector);
                row.CreateCell(45).SetCellValue(list[i].ExhibitionDirector);
                row.CreateCell(46).SetCellValue(list[i].Director);
                row.CreateCell(47).SetCellValue(list[i].ShowNo);
                row.CreateCell(48).SetCellValue(list[i].SystemRemark);
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                hssfworkbook.Write(ms);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString(), Encoding.UTF8).ToString() + ".xls");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
                hssfworkbook = null;
            }
        }

        #endregion
    }
}