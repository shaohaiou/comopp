﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComOpp.Components
{
    public class AdminBase : PageBase
    {
        /// <summary>
        /// 检查当前后台用户是否登陆
        /// </summary>
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
            if (!HXContext.Current.AdminUser.Administrator)
            {
                Response.Clear();
                Response.Write("您没有权限操作！");
                Response.End();
                return;
            }
        }

        /// <summary>
        /// 显示操作成功的页面
        /// </summary>
        protected override string SuccessPageUrl
        {
            get
            {
                return "~/message.aspx";
            }
        }

        /// <summary>
        /// 显示操作失败的页面
        /// </summary>
        protected override string ErrorPageUrl
        {
            get
            {
                return "~/message.aspx";
            }
        }

        /// <summary>
        /// 后台用户名
        /// </summary>
        protected string AdminName
        {
            get
            {
                if (HXContext.Current.AdminCheck)
                {
                    return HXContext.Current.AdminUser.UserName;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 后台用户ID
        /// </summary>
        protected int AdminID
        {
            get
            {
                return HXContext.Current.AdminUserID;
                //return 1;
            }
        }

        /// <summary>
        /// 后台用户实体
        /// </summary>
        protected AdminInfo Admin
        {
            get
            {
                return HXContext.Current.AdminUser;
            }
            set
            {
                HXContext.Current.AdminUser = value;
            }
        }

        protected CorporationInfo Corporation
        {
            get
            {
                return HXContext.Current.CurrentCorporation;
            }
            set
            {
                HXContext.Current.CurrentCorporation = value;
            }
        }

        protected PowerGroupInfo CurrentPowerGroup
        {
            get
            {
                return HXContext.Current.CurrentPowerGroup;
            }
            set
            {
                HXContext.Current.CurrentPowerGroup = value;
            }
        }

        protected virtual string FromUrl
        {
            get
            {
                string from = GetString("from");
                if (!string.IsNullOrEmpty(from))
                {
                    return System.Web.HttpUtility.UrlDecode(from);
                }
                return string.Empty;
            }
        }
    }
}
