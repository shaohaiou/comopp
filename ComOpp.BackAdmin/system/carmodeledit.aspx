<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="carmodeledit.aspx.cs" EnableEventValidation="false"  Inherits="ComOpp.BackAdmin.system.carmodeledit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                车型：</label><input runat="server" id="txtName" name="txtName" class="traceInp textbox"
                    type="text" style="width: 150px !important" datatype="*" nullmsg="车型" errormsg="请输入车型名称" />
        </li>
        <li>
            <label class="llabel">
                品牌：</label><asp:DropDownList runat="server" id="ddlCarBrand"></asp:DropDownList>
        </li>
        <li>
            <label class="llabel">
                车系：</label><asp:DropDownList runat="server" id="ddlCarSeries"></asp:DropDownList>
        </li>
    </ul>
    <div style="margin-top: 10px; padding-left: 110px">
        <asp:button runat="server" id="btnSubmit" text="保存设置" cssclass="btn" onclick="btnSubmit_Click" />
        <input type="reset" value="清除重置" class="btn" />
    </div>
    <div class="hide">
        <input type="hidden" runat="server" id="hdnCarSeries" />
    </div>
    </form>
</div>
<script type="text/javascript">
    $(function () {
        $(".myform").Validform({
            tiptype: 3
        });

        $("#ddlCarSeries").change(function () {
            $("#hdnCarSeries").val($("#ddlCarSeries option:selected").val());
        });
        $("#ddlCarBrand").change(function () {
            $.ajax({ url: '/ajax/getcarseriesbybrand.aspx?id=' + $("#ddlCarBrand option:selected").val(),
                success: function (data) {
                    $("#ddlCarSeries").html(data);
                    if ($("#ddlCarSeries option").length > 0) {
                        $("#hdnCarSeries").val($("#ddlCarSeries option:first").val());
                    }
                    $("#ddlCarSeries").unbind("change").change(function () {
                        $("#hdnCarSeries").val($("#ddlCarSeries option:selected").val());
                    });
                }
            });
        });
    });
</script>