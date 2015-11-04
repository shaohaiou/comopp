using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ComOpp.Components;
using ComOpp.Tools.Web;

namespace ComOpp.Components.HttpHandler
{
    public class CheckAdminUserNameHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string result = "{{\"msg\":\"{0}\",\"result\":\"{1}\"}}";
            CommConfig config = CommConfig.GetConfig();
            string name = WebHelper.GetString("name");
            AdminInfo admin = Admins.Instance.GetAdminByName(name);
            if (admin != null && admin.ID != WebHelper.GetInt("id"))
            {
                HttpContext.Current.Response.Write(string.Format(result, "用户名已使用", "error"));
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format(result, "用户名通过验证", "success"));
            }
            WebHelper.SetNotCache();
        }
    }
}
