using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Text.RegularExpressions;

namespace ComOpp.BackAdmin.chance
{
    public partial class import : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPost())
            {
                int corpid = GetInt("corpid");
                int state = GetInt("state");
                HttpPostedFile file = Request.Files[0];
                DataTable t = ImportDataTableFromExcel(file.InputStream, 0, 0);

                List<InfoTypeInfo> infotypelist = InfoTypes.Instance.GetListByCorpid(corpid, true);
                List<InfoSourceInfo> infosourcelist = InfoSources.Instance.GetListByCorpid(corpid, true);
                List<CarBrandInfo> brandlist = Cars.Instance.GetCarBrandList(true);
                List<AdminInfo> adminlist = Admins.Instance.GetAllAdmins();

                foreach (DataRow row in t.Rows)
                {
                    string customername = row["客户姓名"].ToString();
                    string sex = row["性别"].ToString();
                    string phone = row["客户电话"].ToString();
                    string backupphone = row["备用电话"].ToString();
                    string weixin = row["微信号"].ToString();
                    string infotype = row["信息类型"].ToString();
                    string infosource = row["信息来源"].ToString();
                    string ibuybrand = row["拟购品牌"].ToString();
                    string ibuyseries = row["拟购车系"].ToString();
                    string ibuymodel = row["拟购车型"].ToString();
                    string remark = row["备注"].ToString();
                    string owner = row["线索所有人"].ToString();
                    if (string.IsNullOrEmpty(customername) || string.IsNullOrEmpty(phone)) continue;

                    Regex rphone = new Regex(@"^1[\d]{10}$");
                    if (!rphone.IsMatch(phone)) continue;

                    if (Customers.Instance.GetCustomerByPhone(phone, corpid) == null)
                    {
                        CustomerInfo entity = new CustomerInfo()
                        {
                            Name = customername,
                            Phone = phone,
                            BackupPhone = backupphone,
                            WeixinAccount = weixin,
                            RemarkInfo = remark,
                            SystemRemark = DateTime.Today.ToString("yyyy年MM月dd日") + "导入线索"
                        };
                        switch (sex)
                        {
                            case "男":
                                entity.CustomerSex = 1;
                                break;
                            case "女":
                                entity.CustomerSex = 2;
                                break;
                            default:
                                entity.CustomerSex = 0;
                                break;
                        }
                        entity.InfoTypeID = infotypelist.Exists(l => l.Name.ToLower() == infotype.ToLower().Trim()) ? infotypelist.Find(l => l.Name.ToLower() == infotype.ToLower().Trim()).ID : 0;
                        entity.InfoSourceID = infosourcelist.Exists(l => l.Name.ToLower() == infosource.ToLower().Trim()) ? infosourcelist.Find(l => l.Name.ToLower() == infosource.ToLower().Trim()).ID : 0;
                        entity.IbuyCarBrandID = brandlist.Exists(l => l.Name.ToLower() == ibuybrand.ToLower().Trim()) ? brandlist.Find(l => l.Name.ToLower() == ibuybrand.ToLower().Trim()).ID : 0;
                        if (entity.IbuyCarBrandID > 0)
                        {
                            List<CarSeriesInfo> serieslist = Cars.Instance.GetCarSeriesListByBrandID(entity.IbuyCarBrandID, true);
                            entity.IbuyCarSeriesID = serieslist.Exists(l => l.Name.ToLower() == ibuyseries.ToLower().Trim()) ? serieslist.Find(l => l.Name.ToLower() == ibuyseries.ToLower().Trim()).ID : 0;
                        }
                        else
                            entity.IbuyCarSeriesID = 0;
                        if (entity.IbuyCarSeriesID > 0)
                        {
                            List<CarModelInfo> modellist = Cars.Instance.GetCarModelListBySeriesID(entity.IbuyCarSeriesID, true);
                            entity.IbuyCarModelID = modellist.Exists(l => l.Name.ToLower() == ibuymodel.ToLower().Trim()) ? modellist.Find(l => l.Name.ToLower() == ibuymodel.ToLower().Trim()).ID : 0;
                        }
                        else
                            entity.IbuyCarModelID = 0;

                        entity.OwnerID = AdminID;
                        entity.Owner = Admin.Realname;
                        entity.OwnerPowerGroupID = Admin.PowerGroupID;
                        if (!string.IsNullOrEmpty(owner))
                        {
                            AdminInfo ownerinfo = adminlist.Find(l => l.CorporationID == corpid && l.Realname == owner);
                            if (ownerinfo != null)
                            {
                                entity.OwnerID = ownerinfo.ID;
                                entity.Owner = ownerinfo.Realname;
                                entity.OwnerPowerGroupID = ownerinfo.PowerGroupID;
                            }
                        }


                        entity.CustomerStatus = state;
                        entity.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        entity.CreateUserID = AdminID;
                        entity.CreateUser = Admin.Realname;
                        entity.LastUpdateUserID = AdminID;
                        entity.LastUpdateUser = Admin.Realname;
                        entity.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        entity.PostTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        entity.CorporationID = corpid;

                        Customers.Instance.Add(entity);
                    }
                }


                WriteSuccessMessage("操作完成", "导入数据成功！", "~/chance/chancemg.aspx");
            }
        }

        /// <summary>
        /// 由Excel导入DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="sheetName">Excel工作表索引</param>
        /// <param name="headerRowIndex">Excel表头行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ImportDataTableFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex, int headerColIndex = 0)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(excelFileStream);
            HSSFSheet sheet = (HSSFSheet)workbook.GetSheetAt(sheetIndex);
            DataTable table = new DataTable();
            HSSFRow headerRow = (HSSFRow)sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerColIndex; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "")
                {
                    // 如果遇到第一个空列，则不再继续向后读取
                    cellCount = i + 1;
                    break;
                }
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = (HSSFRow)sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = headerColIndex; j < cellCount; j++)
                {
                    dataRow[j - headerColIndex] = row.GetCell(j) == null ? string.Empty : row.GetCell(j).ToString();
                }
                table.Rows.Add(dataRow);
            }
            excelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }
    }
}