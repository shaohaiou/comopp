<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="ComOpp.BackAdmin.center.profile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>个人中心-红旭集团商机管理系统V1.0</title>
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
                <img src="../images/logo.jpg" alt="" /><span><a href="/center/desktop.aspx" style="color: #2b98c4">个人中心</a></span></div>
            <div class="topTit_right" style="margin: 0;">
                <a href="/index.aspx">首页</a>|<a href="../logout.aspx">[<%=Admin.Realname %>]退出</a></div>
        </div>
    </div>
    <div class="form_box">
        <div class="psw_form" style="padding: 3% 0;">
            <form class="myform bd0" runat="server" id="form1" autocomplete="off">
            <p class="margin_p">
                <label>
                    用户账号：</label></p>
            <p>
                <input type="text" value="<%=AdminName %>" disabled="disabled" /><a class="change_psw" href="safe.aspx">修改密码&nbsp;&gt;&gt;</a><span
                    class="Validform_checktip"><i></i></span></p>
            <p class="margin_p">
                <label>
                    真实姓名：</label></p>
            <p>
                <input runat="server" id="txtRealname" type="text" name="txtRealname" value=""
                    maxlength="30" datatype="name" placeholder="用户昵称或真实姓名" nullmsg="请填写完整的真实姓名" errormsg="请填写完整的真实姓名" /></p>
            <p class="margin_p">
                <label>
                    手机号：</label></p>
            <p>
                <input runat="server" id="txtMobile" type="text" name="txtMobile" value=""
                    maxlength="11" datatype="mobile" placeholder="填写您的手机号码" errormsg="请填写正确的手机号码" /></p>
            <p class="margin_p">
                <label>
                    性别：</label></p>
            <p class="margin_p" style="margin-top: 15px; padding-left: 5px;">
                <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal" CssClass="rbl" RepeatLayout="Flow">
                    <asp:ListItem Text="保密" Value="0"></asp:ListItem>
                    <asp:ListItem Text="男" Value="1"></asp:ListItem>
                    <asp:ListItem Text="女" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </p>
            <p class="margin_p">
                <label>
                    QQ：</label></p>
            <p>
                <input runat="server" type="text" name="txtQQ" id="txtQQ" value="" maxlength="15"
                    placeholder="填写能联系到您的QQ号码" /><span class="Validform_checktip"><i></i></span></p>
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
            红旭集团商机管理系统 V1.0 Powered by <a href="http://www.hongxu.cn">www.hongxu.cn</a>
        2015-2020
    </div>
</body>
</html>
