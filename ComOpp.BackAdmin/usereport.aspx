<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usereport.aspx.cs" Inherits="ComOpp.BackAdmin.usereport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        公司：<asp:DropDownList runat="server" ID="ddlCorporation"></asp:DropDownList>
        <asp:Button runat="server" ID="btnSubmit" Text="销售客户管理系统使用情况统计" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
