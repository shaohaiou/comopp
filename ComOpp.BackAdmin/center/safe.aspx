<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="safe.aspx.cs" Inherits="ComOpp.BackAdmin.center.safe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>密码修改-红旭集团销售客户管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="../css/mycommon.css" />
    <script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="../js/Validform_v5.3.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".myform").Validform({
                tiptype: 3
            })
        });
    </script>
    <style>
        body
        {
            background: #efefef;
            padding-top: 20px;
        }
    </style>
</head>
<body>
    <div class="myc_tit">
        <div class="mytopTit" style="">
            <div class="topTit_left">
                <img src="../images/logo.jpg" alt="" /><span><a href="/center/profile.aspx" style="color: #2b98c4">个人中心</a></span></div>
            <div class="topTit_right" style="margin: 0;">
                <a href="../index.aspx">首页</a>| <a href="../logout.aspx">[<%=Admin.Realname %>]退出</a></div>
        </div>
    </div>
    <div class="form_box">
        <div class="psw_form" style="padding: 3% 0;">
            <form class="myform bd0" runat="server" id="form1" autocomplete="off">
            <p class="margin_p">
                <label>
                    原密码：</label></p>
            <p>
                <input runat="server" id="txtOldpassword" name="txtOldpassword" type="password" value=""
                    maxlength="20" datatype="pwd" nullmsg="不能为空" errormsg="请输入正确原密码" /></p>
            <p class="margin_p">
                <label>
                    新密码：</label></p>
            <p>
                <input runat="server" id="txtPassword" type="password" name="txtPassword" maxlength="20"
                    datatype="pwd" placeholder="密码为5-20字符,可使用字母、数字" nullmsg="不能为空" errormsg="密码为5-20字符,可使用字母、数字" />
                <span class="Validform_checktip"><i></i></span>
            </p>
            <p class="margin_p">
                <label>
                    确认密码：</label></p>
            <p>
                <input runat="server" id="txtPassword2" type="password" name="txtPassword2" maxlength="20"
                    datatype="pwd" recheck="txtPassword" nullmsg="请再输入一次密码！" errormsg="两次输入的密码不一致" /><span
                        class="Validform_checktip"><i></i></span></p>
            <p>
                <asp:Button runat="server" ID="btnSubmit" CssClass="submitBtn" Text="保存设置" OnClick="btnSubmit_Click" />
            </p>
            </form>
        </div>
    </div>
    <div class="footer">
        <div class="footer_con">
            <div class="grayLine">
            </div>
        </div>
        <p class="copystyle">
            红旭集团销售客户管理系统 V1.0 Powered by <a href="http://www.hongxu.cn">www.hongxu.cn</a>
        2015-2020
    </div>
</body>
</html>
