<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="corporationmg.aspx.cs"
    Inherits="ComOpp.BackAdmin.system.corporationmg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公司管理-红旭集团商机管理系统V1.0</title>
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
    <div class="Tab">
        <a href="corporationmg.aspx" class="selected">公司管理</a> 
        <a href="modulemg.aspx">模块管理</a>
        <a href="carbrandmg.aspx">车辆品牌</a>
        <a href="carseriesmg.aspx">车系管理</a>
        <a href="carmodelmg.aspx">车型管理</a>
    </div>
    <div>
        <div style="padding-top: 1px;">
            <div id="toolbar">
                <div class="datagrid-toolbar" style="margin-top: 1px">
                    <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                        onclick="Core.Easyui.Action('add');">添加</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                            iconcls="icon-edit" plain="true" onclick="Core.Easyui.Action('edit');">编辑</a>
                    <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                        onclick="Core.Easyui.Action('remove');">删除</a>
                </div>
            </div>
            <table id="datagrid">
            </table>
        </div>
    </div>
    <script type="text/javascript">
        Core.Easyui.Init($('#datagrid'), 99, [
                { field: 'ck', checkbox: true, width: 30 },
                { field: 'name', title: '公司名称', align: 'left' },
                { field: 'id', title: '排序', width: 120, align: 'center', formatter: function (value, row, index) {
                    var w = "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"moveup\",this);' data-id='" + value + "'>上移</a> ";
                    w += "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"movedown\",this);' data-id='" + value + "'>下移</a> ";
                    w += "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"movetop\",this);' data-id='" + value + "'>置顶</a>";
                    return w;
                }
                }]);

        Core.Easyui.Action = function (method, o) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    dialog('580', '300', '添加', "corporationedit.aspx?r=" + Math.random());
                    break;
                case 'edit':
                    if (!rows.length) return;
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    if (rows[0].channel == 0) return;

                    dialog('580', '300', '编辑', "corporationedit.aspx?id=" + rows[0].id + "&r=" + Math.random()); return;
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
            Core.Easyui.get("/system/corporationlist.aspx");
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
