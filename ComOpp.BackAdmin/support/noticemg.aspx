<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="noticemg.aspx.cs" Inherits="ComOpp.BackAdmin.support.noticemg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>公告通知-红旭集团销售客户管理系统V1.0</title>
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
                <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "公告通知管理"))
                          { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-add" plain="true"
                onclick="Core.Easyui.Action('add');">添加</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="Core.Easyui.Action('edit');">编辑</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-remove" plain="true" onclick="Core.Easyui.Action('remove');">删除</a>
            <%} %>
            <%if (Admin.Administrator)
              {%>
            <div class="schlist" style="float: right; display: inline-block;">
                <form id="Form1" runat="server">
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
                        placeholder="标题" />&nbsp;
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
            return (row.datalevel != 0 ? "" : "<span title='系统默认数据无法编辑和删除' style='color:red'>[系统]</span>") + "<a href='noticeview.aspx?id=" + row.id + "'>" + value + "</a>";
        }
        },
        { field: 'realname', title: '发布人', width: 120, align: 'center' },
        { field: 'addtimestr', title: '发布时间', width: 120, align: 'center', formatter: function (value, row, index) {
            return value;
        }
        }
        ], { toolbar: '#toolbar', singleSelect: true });
        Core.Easyui.Form = function (o) {
            var p = { 
                'keywords': $("#txtKeywordsSearch").val(), 
                "starttime": $("input[name='txtStarttimeSearch']").val(), 
                "endtime": $("input[name='txtEndtimeSearch']").val()
            };
            Core.Easyui.get("noticelist.aspx?corpid=" + corpid + "&keywords=" + p.keywords + "&starttime=" + p.starttime + "&endtime=" + p.endtime);
        }
        Core.Easyui.Action = function (method) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    window.location.href = "noticeedit.aspx?from=<%=CurrentUrl %>&corpid=" + corpid + "&r=" + Math.random();
                    break;
                case 'edit':
                    if (!rows.length) { $.messager.alert('提示', '请选择一条记录', 'info'); return; }
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    <%if (!Admin.Administrator){%>if(rows[0].datalevel == 0){ $.messager.alert('提示', '系统信息，不允许编辑', 'info');return;}<%} %>
                    window.location.href = "noticeedit.aspx?from=<%=CurrentUrl %>&id=" + rows[0].id + "&r=" + Math.random();
                    break;
                case 'remove':
                    var ids = []
                    for (var i = 0; i < rows.length; i++) {
                        <%if (!Admin.Administrator){%>if(rows[0].datalevel != 0)<%} %>
                        ids.push(rows[i].id);
                    }
                    if (!ids.length) return;
                    if (!confirm("您确定要执行该操作吗?执行后将无法恢复!")) return;

                    $.get("?action=del&ids=" + ids.join(',') + "&r=" + Math.random(), function () {
                        $('#datagrid').datagrid("reload");
                    });
                    break;
            }
        }
        $(document).ready(function () {
            Core.Easyui.get("noticelist.aspx?corpid=" + corpid);
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
