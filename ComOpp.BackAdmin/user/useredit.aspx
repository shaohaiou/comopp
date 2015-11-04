<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="useredit.aspx.cs" EnableEventValidation="false" Inherits="ComOpp.BackAdmin.user.useredit" %>

<div>
    <form class="myform" runat="server">
    <ul class="tabs_box_one">
        <li>
            <label class="llabel">
                用户名：</label><input runat="server" type="text" id="txtUserName" name="txtUserName" />
            <asp:customvalidator id="cvUserName" runat="server" clientvalidationfunction="ValidationName"
                cssclass="red" errormessage="该账户名已经被使用" text="该账户名已经被使用" setfocusonerror="True"
                controltovalidate="txtUserName" enableclientscript="true" display="Dynamic"></asp:customvalidator>
        </li>
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
            <asp:radiobuttonlist runat="server" id="rblSex" repeatdirection="Horizontal" repeatlayout="Flow"
                cssclass="rbl">
                    <asp:ListItem Text="保密" Value="0"></asp:ListItem>
                    <asp:ListItem Text="男" Value="1"></asp:ListItem>
                    <asp:ListItem Text="女" Value="2"></asp:ListItem>
                </asp:radiobuttonlist>
        </li>
        <li>
            <label class="llabel">
                状态：</label>
            <asp:radiobuttonlist runat="server" id="rblState" repeatdirection="Horizontal" repeatlayout="Flow"
                cssclass="rbl">
                    <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                    <asp:ListItem Text="锁定" Value="2"></asp:ListItem>
                </asp:radiobuttonlist>
        </li>
        <%if (Admin.Administrator)
          { %>
        <li>
            <label class="llabel">
                所属公司：</label><asp:dropdownlist runat="server" id="ddlCorporation"></asp:dropdownlist></li>
        <%} %>
        <%if (CurrentAdmin == null || CurrentAdmin.UserRole == ComOpp.Components.UserRoleType.普通用户)
          { %>
        <li>
            <label class="llabel">
                账户组：</label><asp:dropdownlist runat="server" id="ddlPowerGroupEdit"></asp:dropdownlist></li>
        <%} %>
    </ul>
    <table class="table" width="100%" border="0" cellspacing="1" cellpadding="2">
    </table>
    <div style="margin-top: 8px; padding-left: 130px">
        <asp:button runat="server" id="btnSubmit" text="保存设置" cssclass="btn" onclick="btnSubmit_Click" />
        <input type="button" id="reset" class="btn" value="清除重置" />
    </div>
    <div class="hide">
        <input type="hidden" runat="server" id="hdnPowerGroup" />
    </div>
    </form>
</div>
<script type="text/javascript">
    var username = '';
    $(function () {
        $("#reset").click(function () { dialog('reload'); });

        username = $("#txtUserName").val();

        $("#ddlPowerGroupEdit").change(function () {
            $("#hdnPowerGroup").val($("#ddlPowerGroupEdit option:selected").val());
        });
        $("#ddlCorporation").change(function () {
            $.ajax({ url: '/ajax/getpowergroupbycorp.aspx?id=' + $("#ddlCorporation option:selected").val(),
                success: function (data) {
                    $("#ddlPowerGroupEdit").html(data);
                    if ($("#ddlPowerGroupEdit option").length > 0) {
                        $("#hdnPowerGroup").val($("#ddlPowerGroupEdit option:first").val());
                    }
                    $("#ddlPowerGroupEdit").unbind("change").change(function () {
                        $("#hdnPowerGroup").val($("#ddlPowerGroupEdit option:selected").val());
                    });
                }
            });
        });
    })
    function ValidationName(source, arguments) {
        var v = false;
        if (username == $("#txtUserName").val()) {
            arguments.IsValid = true;
            return;
        }
        $.ajax({
            url: 'checkadmin.axd?d=' + new Date(),
            async: false,
            dataType: "json",
            data: { name: $("#txtUserName").val() },
            error: function (msg) {
                alert("发生错误！");
            },
            success: function (data) {
                if (data.result == 'success') {
                    v = true;
                }
                else {
                    v = false;
                }
            }
        });
        arguments.IsValid = v;
    }
</script>
