<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminedit.aspx.cs" Inherits="ComOpp.BackAdmin.admin.adminedit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                用户名：</label><input runat="server" type="text" id="txtUserName" name="txtUserName" /></li>
        <li>
            <label class="llabel">
                密码：</label><input runat="server" type="text" id="txtPassword" name="txtPassword" /></li>
        <li>
            <label class="llabel">
                姓名：</label><input runat="server" type="text" id="txtRealname" name="txtRealname" /></li>
        <li>
            <label class="llabel">
                手机：</label><input runat="server" type="text" id="txtMobile" name="txtMobile" /></li>
        <li>
            <label class="llabel">
                QQ：</label><input runat="server" type="text" id="txtQQ" name="txtMobile" /></li>
        <li>
            <label class="llabel">
                性别：</label>
                <asp:RadioButtonList runat="server" ID="rblSex" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rbl">
                    <asp:ListItem Text="保密" Value="0"></asp:ListItem>
                    <asp:ListItem Text="男" Value="1"></asp:ListItem>
                    <asp:ListItem Text="女" Value="2"></asp:ListItem>
                </asp:RadioButtonList></li>
        <li>
            <label class="llabel">
                状态：</label>
                <asp:RadioButtonList runat="server" ID="rblState" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="rbl">
                    <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                    <asp:ListItem Text="锁定" Value="2"></asp:ListItem>
                </asp:RadioButtonList></li>
        <%if (CurrentAdmin == null || !CurrentAdmin.Administrator)
          {%>
        <li>
            <label class="llabel">
                所属公司：</label><asp:dropdownlist runat="server" id="ddlCorp"></asp:dropdownlist></li>
        <%} %>
    </ul>
    <table class="table" width="100%" border="0" cellspacing="1" cellpadding="2">
    </table>
    <div style="margin-top: 8px; padding-left: 130px">
        <asp:button runat="server" id="btnSubmit" text="保存设置" cssclass="btn" onclick="btnSubmit_Click" />
        <input type="button" id="reset" class="btn" value="清除重置" />
    </div>
    </form>
</div>
<script type="text/javascript">
    $(function () {
        $("#reset").click(function () { dialog('reload'); })
    })
</script>
