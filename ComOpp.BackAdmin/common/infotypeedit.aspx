<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="infotypeedit.aspx.cs" Inherits="ComOpp.BackAdmin.common.infotypeedit" %>

<div>
    <form class="myform" runat="server" onsubmit="return Core.P.Form(this);">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                名称：</label><input runat="server" type="text" id="txtName" name="txtName" class="traceInp textbox Validform_error"
                    datatype="*" nullmsg="名称" errormsg="请输入名称" />
        </li>
        <li>
            <label class="llabel">
                锁定级别状态：</label>
            <asp:radiobuttonlist runat="server" id="rblLocked" repeatdirection="Horizontal" repeatlayout="Flow"
                cssclass="rbl">
                <asp:ListItem Text="禁用" Value="0" selected="true"></asp:ListItem>
                    <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                </asp:radiobuttonlist>
        </li>
        <li>
            <label class="llabel">
                锁定级别天数：</label><input runat="server" type="text" id="txtLockday" name="txtLockday"
                    class="traceInp textbox Validform_error" datatype="n" nullmsg="锁定级别天数" errormsg="请输入锁定级别天数" />
                    </li>
        <li>
            <label class="llabel">
                锁定级别：</label>
                <asp:CheckBoxList runat="server" id="cblLocklevel" repeatdirection="Horizontal" repeatlayout="Flow" cssclass="rbl"></asp:CheckBoxList>
        </li>
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
        $(".myform").Validform({
            tiptype: 3
        });
        $("#reset").click(function () { dialog('reload'); });
    })
</script>
