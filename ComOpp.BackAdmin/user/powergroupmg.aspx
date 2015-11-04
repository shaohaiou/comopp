<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="powergroupmg.aspx.cs" Inherits="ComOpp.BackAdmin.user.powergroupmg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>账户组管理-红旭集团商机管理系统V1.0</title>
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
        <div class="datagrid-toolbar" style="margin-top: 1px">
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="Core.Easyui.Action('add');">添加</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="Core.Easyui.Action('edit');">编辑</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                onclick="Core.Easyui.Action('remove');">删除</a>
        </div>
        <%if (Admin.Administrator)
          {%>
        <form runat="server">
        <div class="schlist">
            <div style="padding-top: 1px; line-height: 28px;">
                <label>
                    所属公司：</label>
                <asp:DropDownList runat="server" ID="ddlCorporationSearch" AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        </form>
        <%} %>
         <div class="aSearch"></div>
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
        Core.Easyui.Init($('#datagrid'), 68, [
        { field: 'ck', checkbox: true, width: 30 },
        { field: 'groupname', title: '名称', width: 120, align: 'center' },
        { field: 'membercount', title: '成员数量', width: 80, align: 'center' },
        { field: 'lastupdatetime', title: '最后更新', width: 160, align: 'center' },
            { field: 'id', title: '排序', align: 'left', formatter: function (value, row, index) {
                var w = "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"moveup\",this);' data-id='" + value + "'>上移</a> ";
                w += "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"movedown\",this);' data-id='" + value + "'>下移</a> ";
                w += "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"movetop\",this);' data-id='" + value + "'>置顶</a>";
                return w;
            }
            }
        ],{singleSelect : true});
        Core.Easyui.Form = function () {
            var p = { 'corpid': $("#ddlCorporationSearch option:selected").val() };
            Core.Easyui.load(p);
        }
        Core.Easyui.Action = function (method,o) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    window.location.href = "powergroupedit.aspx?from=<%=CurrentUrl %>&corpid=" + corpid + "&r=" + Math.random();
                    break;
                case 'edit':
                    if (!rows.length) { $.messager.alert('提示', '请选择一条记录', 'info'); return; }
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    window.location.href = "powergroupedit.aspx?id=" + rows[0].id + "&from=<%=CurrentUrl %>&r=" + Math.random();
                    break;
                case 'remove':
                    var ids = []
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].id);
                    }
                    if (!ids.length) return;
                    if (!confirm("您确定要执行该操作吗?执行后将无法恢复!")) return false;
                    $.get("?action=del&ids=" + ids.join(',') + "&r=" + Math.random(), function () {
                        Core.Easyui.reload();
                    })
                    break;
                case 'moveup':
                case 'movedown':
                case 'movetop':
                    $.getJSON("?action=" + method + "&id=" + $(o).attr('data-id'), function (data) {
                        if (data.state != 1) return;
                        Core.Easyui.reload();
                    })
                    break;
            }
        }
        $(document).ready(function () {
            Core.Easyui.get("/user/powergrouplist.aspx?corpid=" + corpid);
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
