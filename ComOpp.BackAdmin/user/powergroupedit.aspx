<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="powergroupedit.aspx.cs"
    Inherits="ComOpp.BackAdmin.user.powergroupedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>账户组<%= GetInt("id") > 0 ? "编辑" : "添加" %>-红旭集团销售客户管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
    <link rel="stylesheet" type="text/css" href="../plugins/jquery-easyui-1.3.6/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../plugins/jquery-easyui-1.3.6/themes/icon.css" />
    <script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="../plugins/jquery-easyui-1.3.6/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../plugins/jquery-easyui-1.3.6/datagrid-detailview.js"></script>
    <script type="text/javascript" src="../plugins/jquery-easyui-1.3.6/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../js/highcharts.js"></script>
    <script type="text/javascript" src="../js/public.core.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/jquery.idTabs.min.js"></script>
    <script type="text/javascript" src="../js/Validform_v5.3.js"></script>
    <style type="text/css">
        .appsauth
        {
            margin-left: 3px;
            color: #393939;
            line-height: 20px;
        }
        .appsauth div
        {
            font-weight: bold;
            clear: both;
        }
        .appsauth ul
        {
            margin-left: 8px;
        }
        .appsauth ul li
        {
            float: left;
            margin-right: 3px;
        }
        .appsauth li input
        {
            margin-right: 1px;
        }
    </style>
</head>
<body>
    <div class="Tab">
        <a href="powergroupmg.aspx">账户管理</a> <a href="javascript:void(0);" class="selected">
            账户组<%= GetInt("id") > 0 ? "编辑" : "添加" %></a>
    </div>
    <div>
        <form class="myform" runat="server">
        <ul class="tabs_box_one">
            <li>
                <label class="llabel">
                    名称：</label><input runat="server" id="txtGroupName" name="txtGroupName" value="" class="traceInp textbox"
                        type="text" style="width: 150px !important" datatype="*" nullmsg="账户组名称" errormsg="请输入账户组名称" /></li>
            <li>
                <table width="100%" border="0" cellspacing="1" cellpadding="2">
                    <tr>
                        <td width="110" align="right" valign="top">
                            授权权限：
                        </td>
                        <td style="padding-bottom: 8px" class="rbl">
                            <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_ItemDataBound">
                                <ItemTemplate>
                                    <div class="appsauth">
                                        <div>
                                            <%# Eval("ModuleName")%></div>
                                        <ul>
                                            <asp:Repeater runat="server" ID="rptModules">
                                                <ItemTemplate>
                                                    <li style="margin: 0">
                                                        <label>
                                                            <input type="checkbox" value="<%# Eval("ModuleName") %>" class="module" <%# GetPowerStatus(Eval("ModuleName")) %> /><%#Eval("ModuleName")%></label>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <%# GetCanviewPowerGroupHtml(Eval("ModuleName"))%>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </li>
        </ul>
        <div style="margin-top: 10px; padding-left: 110px">
            <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="保存设置" CssClass="btn" />
            <input type="reset" value="清除重置" class="btn">
        </div>
        <div class="hide">
            <input type="hidden" runat="server" id="hdnGroupPower" />
            <input type="hidden" runat="server" id="hdnCanViewGroup" />
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
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();

            $(".module").click(function () {
                $("#hdnGroupPower").val($(".module:checked").map(function () {
                    return $(this).val();
                }).get().join(','));
            });
            $(".canviewgroup").click(function () {
                $("#hdnCanViewGroup").val($(".canviewgroup:checked").map(function () {
                    return $(this).val();
                }).get().join(','));
            });
            $("#cbxcanview").click(function () {
                $(".archivegroup").toggle();
            });
        })
    </script>
</body>
</html>
