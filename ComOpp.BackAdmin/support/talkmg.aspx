<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="talkmg.aspx.cs" Inherits="ComOpp.BackAdmin.support.talkmg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>话术管理-红旭集团商机管理系统V1.0</title>
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
    <div id="toolbar" style="padding: 0; height: auto; margin-top: 1px">
        <div>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="Core.Easyui.Action('add');">添加</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-remove" plain="true" onclick="Core.Easyui.Action('remove');">删除</a>
            <%if (Admin.Administrator)
              {%>
            <div class="schlist" style="float: right; display: inline-block;">
                <form runat="server">
                <div style="padding-top: 1px; line-height: 28px;">
                    <label>
                        公司：</label>
                    <asp:DropDownList runat="server" ID="ddlCorporationSearch" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                </form>
            </div>
            <%} %>
            <div class="aSearch">
            </div>
        </div>
        <div class="schlist">
            <div style="padding-top: 1px;">
                <label>
                    关键字：</label><input type="text" class="textin" id="txtKeywordsSearch" value="" style="width: 110px"
                        placeholder="标题/标签" />&nbsp;
                <label>
                    时间：</label><input id="txtStarttimeSearch" name="txtStarttimeSearch" class="easyui-datebox" style="width: 90px">&nbsp;至&nbsp;<input
                        id="txtEndtimeSearch" name="txtEndtimeSearch" class="easyui-datebox" style="width: 90px">
                <a href="javascript:void(0);" onclick="return Core.Easyui.Form(this);" class="easyui-linkbutton"
                    style="margin-left: 20px;">确定</a>
            </div>
        </div>
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
        Core.Easyui.Init($('#datagrid'), 37, [
        { field: 'ck', checkbox: true, width: 30 },
        { field: 'title', title: '标题', align: 'left', formatter: function (value, row, index) {
            return "<a href='talkedit.aspx?id=" + row.id + "'>" + value + "</a>";

        }
        },
        { field: 'tag', title: '标签', width: 180, align: 'center' },
        { field: 'realname', title: '发布者', width: 120, align: 'center' },
        { field: 'addtime', title: '发布日期', width: 120, align: 'center', formatter: function (value, row, index) {
            return value.split(' ')[0];
        }
        }
        ], { toolbar: '#toolbar', singleSelect: false });
        Core.Easyui.Form = function (o) {
            var p = { 
                'keywords': $("#txtKeywordsSearch").val(), 
                "starttime": $("input[name='txtStarttimeSearch']").val(), 
                "endtime": $("input[name='txtEndtimeSearch']").val()
            };
            Core.Easyui.get("talklist.aspx?corpid=" + corpid + "&keywords=" + p.keywords + "&starttime=" + p.starttime + "&endtime=" + p.endtime);
        }
        Core.Easyui.Action = function (method) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    window.location.href = "talkedit.aspx?from=<%=CurrentUrl %>&corpid=" + corpid + "&r=" + Math.random();
                    break;
                case 'remove':
                    var ids = []
                    for (var i = 0; i < rows.length; i++) {
                        ids.push(rows[i].id);
                    }
                    if (!ids.length) return;
                    if (!confirm("您确定要执行该操作吗?执行后将无法恢复!")) return false;

                    $.get("?action=del&ids=" + ids.join(',') + "&r=" + Math.random(), function () {
                        $('#datagrid').datagrid("reload");
                    });
                    break;
            }
        }
        $(document).ready(function () {
            Core.Easyui.get("talklist.aspx?corpid=" + corpid);
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
