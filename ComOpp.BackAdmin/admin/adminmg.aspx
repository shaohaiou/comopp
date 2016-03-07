<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminmg.aspx.cs" Inherits="ComOpp.BackAdmin.admin.adminmg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理员管理-红旭集团销售客户管理系统V1.0</title>
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
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="Core.Easyui.Action('add');">添加</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="Core.Easyui.Action('edit');">编辑</a>
        </div>
        <form runat="server" onsubmit="Core.Easyui.Form();return false;">
        <div class="schlist">
            <div style="padding-top: 1px;">
                <label>
                    关键字：</label><input type="text" class="textin" id="txtKeywords" name="txtKeywords"
                        value="" style="width: 110px" placeholder="用户名/姓名/QQ" />
                <a href="javascript:void(0);" onclick="return Core.Easyui.Form();" class="easyui-linkbutton"
                    style="margin-left: 20px;">确定</a>
            </div>
        </div>
        </form>
    </div>
    <table id="datagrid">
    </table>
    <script type="text/javascript">
        Core.Easyui.Init($('#datagrid'), 99, [
        { field: 'ck', checkbox: true, width: 30 },
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
        { field: 'corp', title: '所属公司', width: 160, align: 'center' },
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
        Core.Easyui.Form = function (o) {
            var p = { 'keywords': $("#txtKeywords").val() };
            Core.Easyui.load(p);
        }
        Core.Easyui.Action = function (method) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    dialog('500', '400', '添加管理员', "adminedit.aspx?from=<%=CurrentUrl %>&r=" + Math.random());
                    break;
                case 'edit':
                    if (!rows.length) { $.messager.alert('提示', '请选择一条记录', 'info'); return; }
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    dialog('500', '400', '编辑:' + rows[0].realname, "adminedit.aspx?id=" + rows[0].id + "&from=<%=CurrentUrl %>&r=" + Math.random());
                    break;
            }
        }
        $(document).ready(function () {
            Core.Easyui.get("/admin/adminlist.aspx");
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
