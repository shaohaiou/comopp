<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ComOpp.BackAdmin.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录 - 红旭集团销售客户管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="css/mycommon.css" />
    <script src="js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="js/Validform_v5.3.js" type="text/javascript"></script>
    <script type="text/javascript">
        if (self.location != top.location) {
            top.location.href = self.location;
        }
        $(function () {
            /*表单验证*/
            $(".myform").Validform({
                tiptype: 3
            });
            if ($("#lblMsg").text() != "") {
                setTimeout(function () {
                    $("#lblMsg").text("");
                }, 2000);
            }
        })
    </script>
</head>
<body>
    <div class="myc_tit">
        <div class="mytopTit" style="">
            <div class="topTit_left">
                <img src="images/logo.jpg" /><span>系统登录</span></div>
            <div class="topTit_right">
            </div>
        </div>
    </div>
    <div class="form_box">
        <div class="login_fm">
            <form class="myform" runat="server" id="form1" autocomplete="off">
            <p class="margin_p">
                <label>
                    登录帐号：</label></p>
            <p>
                <input type="text" id="txtUserName" name="txtUserName" runat="server" value="" maxlength="20" datatype="name"
                    nullmsg="不能为空" errormsg="请输入正确的用户名" /><span class="Validform_checktip"><i></i></span></p>
            <p class="margin_p">
                <label>
                    登录密码：</label></p>
            <p>
                <input type="password" id="txtUserPwd" name="txtUserName" runat="server" value="" maxlength="20" datatype="pwd"
                    nullmsg="不能为空" errormsg="请输入正确密码" /><span class="Validform_checktip"><i></i></span></p>
            <p>
                <asp:Label runat="server" ID="lblMsg" Text="" CssClass="red" style="position: absolute;margin-top: 10px;"></asp:Label>
                <asp:Button ID="btSave" runat="server" CssClass="submitBtn" Text="登录" OnClick="btSave_Click" /></p>
            </form>
        </div>
        <div class="form_right">
        </div>
    </div>
    <div class="footer">
        <div class="footer_con">
            <div class="grayLine">
            </div>
        </div>
        <p class="copystyle">
            红旭集团销售客户管理系统 V1.0 Powered by <a href="http://www.hongxu.cn">www.hongxu.cn</a> 2015-2020
    </div>
</body>
</html>
