<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="carseriesedit.aspx.cs" Inherits="ComOpp.BackAdmin.system.carseriesedit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                车系：</label><input runat="server" id="txtName" name="txtName" class="traceInp textbox"
                    type="text" style="width: 150px !important" datatype="*" nullmsg="车系" errormsg="请输入车系名称" />
        </li>
        <li>
            <label class="llabel">
                品牌：</label><asp:DropDownList runat="server" id="ddlCarBrand"></asp:DropDownList>
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