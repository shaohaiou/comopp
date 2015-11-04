<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="connectwayedit.aspx.cs" Inherits="ComOpp.BackAdmin.common.connectwayedit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                名称：</label><input runat="server" type="text" id="txtName" name="txtName" class="traceInp textbox Validform_error"
                    datatype="*" nullmsg="名称" errormsg="请输入名称" />
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
