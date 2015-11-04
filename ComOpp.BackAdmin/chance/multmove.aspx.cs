using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;
using ComOpp.Tools;

namespace ComOpp.BackAdmin.chance
{
    public partial class multmove : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
            if (!Admin.Administrator && Admin.UserRole != UserRoleType.系统管理员 && !CheckGroupPower(Admin.GroupPower, "权限移交"))
            {
                WriteMessage("/message.aspx", "系统提示", "没有权限！", "", "/index.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(GetString("method")))
                    BindControler();
                else
                    MoveData();
            }
        }

        private void BindControler()
        {
            int corpid = GetInt("corpid");
            if (corpid > 0)
            {
                List<AdminInfo> list = Admins.Instance.GetAllAdmins();
                list = list.FindAll(l => l.CorporationID == corpid);
                rptOuter.DataSource = list;
                rptOuter.DataBind();

                rptOwner.DataSource = list;
                rptOwner.DataBind();
            }
        }

        private void MoveData()
        {
            string method = GetString("method");
            int owneruid = GetInt("owneruid");
            string password = GetString("password");
            int page = GetInt("page");
            int maxpage = 0;
            if (owneruid == 0 || string.IsNullOrEmpty(password))
                return;
            if (Admin.Password != EncryptString.MD5(password))
            {
                Response.Write("success,error.pwd,");
                Response.End();
            }

            if (method == "ids")
            {
                int result = 0;
                try
                {
                    string[] ids = GetString("ids").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    maxpage = ids.Length;
                    AdminInfo admin = Admins.Instance.GetAdmin(owneruid);
                    CustomerInfo customer = Customers.Instance.GetCustomerByID(DataConvert.SafeInt(ids[page - 1]));
                    customer.OwnerID = owneruid;
                    customer.Owner = admin.Realname;
                    customer.OwnerPowerGroupID = admin.PowerGroupID;
                    customer.LastUpdateUserID = AdminID;
                    customer.LastUpdateUser = Admin.Realname;
                    customer.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    result = Customers.Instance.Move(customer);
                }
                catch
                {
                    Response.Clear();
                    Response.Write("failed");
                    Response.End();
                }
                Response.Clear();
                if (result > 0)
                    Response.Write("success,0," + maxpage + "," + page);
                else
                    Response.Write("failed");
                Response.End();
            }
            else if (method == "member")
            {
                int result = 0;
                try
                {
                    int source = GetInt("source");
                    string idsstr = Session["menbercustomerlistids"] as string;
                    if (string.IsNullOrEmpty(idsstr))
                    {
                        AdminInfo adminsource = Admins.Instance.GetAdmin(source);

                        List<CustomerInfo> list = Customers.Instance.GetCustomerListByCorporation(adminsource.CorporationID, true);
                        list = list.FindAll(l => l.OwnerID == adminsource.ID);
                        idsstr = string.Join(",", list.Select(l => l.ID));
                        Session["menbercustomerlistids"] = idsstr;
                    }
                    string[] ids = idsstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    maxpage = ids.Length;

                    AdminInfo admin = Admins.Instance.GetAdmin(owneruid);
                    CustomerInfo customer = Customers.Instance.GetCustomerByID(DataConvert.SafeInt(ids[page - 1]));
                    customer.OwnerID = owneruid;
                    customer.Owner = admin.Realname;
                    customer.OwnerPowerGroupID = admin.PowerGroupID;
                    customer.LastUpdateUserID = AdminID;
                    customer.LastUpdateUser = Admin.Realname;
                    customer.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    result = Customers.Instance.Move(customer);
                    if (page == maxpage)
                        Session["menbercustomerlistids"] = null;
                }
                catch
                {
                    Session["menbercustomerlistids"] = null;
                    Response.Clear();
                    Response.Write("failed");
                    Response.End();
                }
                Response.Clear();
                if (result > 0)
                    Response.Write("success,0," + maxpage + "," + page);
                else
                    Response.Write("failed");
                Response.End();
            }
        }

        protected string SetAdminSel(object o)
        {
            string result = string.Empty;
            int id = DataConvert.SafeInt(o);
            if (id == AdminID)
                result = "selected=\"selected\"";
            return result;
        }
    }
}