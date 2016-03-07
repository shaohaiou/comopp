<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usermg.aspx.cs" Inherits="ComOpp.BackAdmin.user.usermg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>账户管理-红旭集团销售客户管理系统V1.0</title>
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
</head>
<body>
    <div id="toolbar">
        <div class="datagrid-toolbar" style="margin-top: 1px">
            <% if (PowerGroupCount > 0)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="Core.Easyui.Action('add');">添加</a>
            <%} %><a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-edit"
                plain="true" onclick="Core.Easyui.Action('edit');">编辑</a>
            <%if (Admin.Administrator)
              { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                onclick="Core.Easyui.Action('remove');">删除</a>
            <%} %>
        </div>
        <form runat="server" onsubmit="return Core.Easyui.Form();">
        <div class="schlist">
            <div style="padding-top: 1px;">
                <%if (Admin.Administrator)
                  {%>
                <label>
                    所属公司：</label>
                <asp:DropDownList runat="server" ID="ddlCorporationSearch">
                </asp:DropDownList>
                <%}else{ %>
                <label>
                    权限组：</label>
                <asp:DropDownList runat="server" ID="ddlPowerGroupSearch">
                </asp:DropDownList>
                <%} %>
                <label>
                    关键字：</label><input type="text" class="textin" id="txtKeywords" name="txtKeywords"
                        value="" style="width: 110px" placeholder="用户名/姓名/QQ" />
                <label>
                    状态：</label>
                <asp:DropDownList runat="server" ID="ddlState">
                    <asp:ListItem Text="全部" Value="0"></asp:ListItem>
                    <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                    <asp:ListItem Text="锁定" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <a href="javascript:void(0);" onclick="return Core.Easyui.Form();" class="easyui-linkbutton"
                    style="margin-left: 20px;">确定</a>
            </div>
        </div>
        </form>
    </div>
    <table id="datagrid">
    </table>
    <script type="text/javascript">
        var corpid =0;
        <%if (Admin.Administrator)
          {%>
        corpid = $("#ddlCorporationSearch option:selected").val();
        <%}else{ %>
        corpid = <%=Admin.CorporationID %>;
        <%} %>
        Core.Easyui.Init($('#datagrid'), 99, [
        { field: 'ck', checkbox: true, width: 30 },
        <%if(Admin.Administrator) {%>
        { field: 'corp', title: '所属公司', width: 120, align: 'center' },
        <%} %>
        { field: 'group', title: '权限组', width: 120, align: 'center' },
        { field: 'username', title: '用户名', width: 120, align: 'center' },
        { field: 'realname', title: '姓名', width: 80, align: 'center' },
        { field: 'mobile', title: '手机号', width: 100, align: 'center' },
        { field: 'sex', title: '性别', width: 60, align: 'center', formatter: function (value, row, index) {
            switch (value) {
                case 1: return "男"; break;
                case 2: return "女"; break;
                default: return "保密"; break; 
            }
        }
        },
        { field: 'qq', title: 'QQ', width: 100, align: 'center' },
        { field: 'state', title: '状态', width: 60, align: 'center', formatter: function (value, row, index) {
            switch (value) {
                case '1': return "正常"; break;
                case '2': return "锁定"; break;
                default: return "异常"; break;
            }
        }
        },
        { field: 'lastlogin', title: '最后登录', align: 'left' }
        ], { singleSelect: true });
        Core.Easyui.Form = function () {
            var p = { 
            'keywords':$("#txtKeywords").val(),
            'state': $("#ddlState option:selected").val(),
            'powergroup':$("#ddlPowerGroupSearch option:selected").val()};
            Core.Easyui.get("/user/userlist.aspx?corpid=" + corpid + "&keywords=" + p.keywords + "&state=" + p.state + "&powergroup=" + p.powergroup);
            return false;
        }
        Core.Easyui.Action = function (method,o) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    dialog('580', '400', '添加账户', "useredit.aspx?from=<%=CurrentUrl %>&corpid=" + corpid + "&r=" + Math.random());
                    break;
                case 'edit':
                    if (!rows.length) { $.messager.alert('提示', '请选择一条记录', 'info'); return; }
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    dialog('580', '400', '编辑' + rows[0].realname, "useredit.aspx?id=" + rows[0].id + "&from=<%=CurrentUrl %>&r=" + Math.random());
                    break;
                case 'remove':
                    if (!rows.length) { $.messager.alert('提示', '请选择一条记录', 'info'); return; }
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    if (!confirm("您确定要执行该操作吗?执行后将无法恢复!")) return false;
                    $.get("?action=del&id=" + rows[0].id + "&r=" + Math.random(), function () {
                        Core.Easyui.reload();
                    })
                    break;
            }
        }
        $(document).ready(function () {
            Core.Easyui.get("/user/userlist.aspx?corpid=" + corpid);

            $("#ddlCorporationSearch").change(function(){
                corpid = $("#ddlCorporationSearch option:selected").val();
                $("#txtKeywords").val("");
                $("#ddlState").val(0);
                Core.Easyui.get("/user/userlist.aspx?corpid=" + corpid);
            });
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
