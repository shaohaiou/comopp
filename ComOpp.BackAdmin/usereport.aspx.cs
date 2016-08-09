using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using ComOpp.Tools;

namespace ComOpp.BackAdmin
{
    public partial class usereport : AdminBase
    {
        protected override void Check()
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCorporation.DataSource = Corporations.Instance.GetList(true);
                ddlCorporation.DataTextField = "Name";
                ddlCorporation.DataValueField = "ID";
                ddlCorporation.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender,EventArgs e)
        {
            ExportUseDetail();
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

        protected void ExportUseDetail()
        {
            List<AdminInfo> adminlist = Admins.Instance.GetUsers();
            List<CustomerInfo> customerlist = Customers.Instance.GetCustomerListByCorporation(DataConvert.SafeInt(ddlCorporation.SelectedValue),true);
            CustomerConnectRecordQuery query;
            List<CustomerConnectRecordInfo> connectrecordlist;
            adminlist = adminlist.FindAll(a => a.CorporationID == DataConvert.SafeInt(ddlCorporation.SelectedValue)).OrderBy(c=>c.PowerGroupID).ToList();
            InitializeWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet("Sheet1");
            HSSFRow rowHeader = (HSSFRow)sheet1.CreateRow(0);
            rowHeader.CreateCell(0).SetCellValue("姓名");
            rowHeader.CreateCell(1).SetCellValue("线索数量");
            rowHeader.CreateCell(2).SetCellValue("跟踪次数");
            rowHeader.CreateCell(3).SetCellValue("登录次数");
            rowHeader.CreateCell(4).SetCellValue("最后登录时间");

            for (int i = 0; i < adminlist.Count; i++)
            {
                HSSFRow row = (HSSFRow)sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(adminlist[i].Realname);
                row.CreateCell(4).SetCellValue(adminlist[i].LastLoginTime.HasValue ? adminlist[i].LastLoginTime.Value.ToString("yyyy-M-d HH:mm:ss") : "");
                row.CreateCell(3).SetCellValue(adminlist[i].LoginTimes);
                row.CreateCell(1).SetCellValue(customerlist.FindAll(c => c.OwnerID == adminlist[i].ID).Count);
                int recordcount = 0;
                query = new CustomerConnectRecordQuery();
                query.ConnectUserID = adminlist[i].ID;
                connectrecordlist = CustomerConnectRecords.Instance.GetList(query, 1, 1, ref recordcount);
                row.CreateCell(2).SetCellValue(recordcount);
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                hssfworkbook.Write(ms);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.AppendHeader("Content-Disposition", "attachment; filename=销售客户管理系统使用情况统计.xls");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
                hssfworkbook = null;
            }
        }

        #endregion
    }
}