﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="corporationedit.aspx.cs"
    Inherits="ComOpp.BackAdmin.system.corporationedit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                名称：</label><input runat="server" id="txtName" name="txtName" class="traceInp textbox"
                    type="text" style="width: 150px !important" datatype="*" nullmsg="公司名称" errormsg="请输入公司名称" />
        </li>
    </ul>
    <div style="margin-top: 10px; padding-left: 110px">
        <asp:button runat="server" id="btnSubmit" text="保存设置" cssclass="btn" onclick="btnSubmit_Click" />
        <input type="reset" value="清除重置" class="btn" />
    </div>
    </form>
</div>
<script type="text/javascript">
    $(function () {
        $(".myform").Validform({
            tiptype: 3
        });
    });
</script>
