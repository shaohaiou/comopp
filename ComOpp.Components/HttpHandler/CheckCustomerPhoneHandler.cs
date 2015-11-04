using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ComOpp.Components;
using ComOpp.Tools.Web;

namespace ComOpp.Components.HttpHandler
{
    public class CheckCustomerPhoneHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string result = "{{\"msg\":\"{0}\",\"result\":\"{1}\",\"isdel\":\"{2}\",\"id\":\"{3}\"}}";
            CommConfig config = CommConfig.GetConfig();
            string phone = WebHelper.GetString("phone");
            CustomerInfo customer = Customers.Instance.GetCustomerByPhone(phone);
            if (customer != null)
            {
                HttpContext.Current.Response.Write(string.Format(result, "该客户电话已经存在", "error", customer.DelState,customer.ID));
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format(result, "该客户电话通过验证", "success",string.Empty,string.Empty));
            }
            WebHelper.SetNotCache();
        }
    }
}
