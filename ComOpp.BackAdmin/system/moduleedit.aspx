<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="moduleedit.aspx.cs" Inherits="ComOpp.BackAdmin.system.moduleedit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                名称：</label><input runat="server" id="txtModuleName" name="txtModuleName" class="traceInp textbox"
                    type="text" style="width: 150px !important" datatype="*" nullmsg="模块名称" errormsg="请输入模块名称" />
        </li>
        <li>
            <label class="llabel">
                上级模块：</label><input runat="server" id="txtParentName" name="txtParentName" class="traceInp textbox"
                    type="text" style="width: 150px !important" errormsg="请输入上级模块名称" />
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
