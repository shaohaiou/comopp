<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="infotypemg.aspx.cs" Inherits="ComOpp.BackAdmin.common.infotypemg" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>信息类型管理-红旭集团商机管理系统V1.0</title>
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
        <a href="basesetting.aspx">基础设置</a> <a href="customerlevelmg.aspx">客户等级</a> <a href="infotypemg.aspx"
            class="selected">信息类型</a> <a href="infosourcemg.aspx">信息来源</a> <a href="connectwaymg.aspx">
                追踪方式</a> <a href="giveupcausemg.aspx">放弃原因</a> <a href="paymentwaymg.aspx">支付方式</a>
        <a href="ibuytimemg.aspx">拟购时间</a> <a href="tracktagmg.aspx">线索标签</a> <a href="archivesetting.aspx">
            转出设置</a>
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
                </div>
            </div>
            <table id="datagrid">
            </table>
        </div>
    </div>
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
        { field: 'showname', title: '名称', width: 120, align: 'left',formatter:function(value,row,index){
            return (row.datalevel != 0 ? "" : "<span title='系统默认数据无法删除' style='color:red'>[系统]</span>")+value;
        } },
        { field: 'locked', title: '锁定级别状态',width:120, align: 'center',formatter:function(value,row,index){
            return value == 0 ? "禁用" : "启用";
        } },
        { field: 'lockday', title: '锁定级别天数',width:120, align: 'center'},
        { field: 'locklevelname', title: '锁定级别',width:120, align: 'center'},
        {field:'id',title:'排序',align:'left',formatter:function(value,row,index){
            var w = "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"moveup\",this);' data-id='"+value+"'>上移</a> ";
            w += "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"movedown\",this);' data-id='"+value+"'>下移</a> ";
            w += "<a href='javascript:void(0);' onClick='Core.Easyui.Action(\"movetop\",this);' data-id='"+value+"'>置顶</a>";
            return w;
        }
        }
        ]);
        Core.Easyui.Action = function (method, o) {
            var rows = $('#datagrid').datagrid('getSelections');
            switch (method) {
                case 'add':
                    dialog('580', '300', '添加', "infotypeedit.aspx?from=<%=CurrentUrl %>&corpid=" + corpid + "&r=" + Math.random());
                    break;
                case 'edit':
                    if (!rows.length) { $.messager.alert('提示', '请选择一条记录', 'info'); return; }
                    if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                    dialog('580', '300', '编辑', "infotypeedit.aspx?from=<%=CurrentUrl %>&id=" + rows[0].id + "&r=" + Math.random());
                    break;
                case 'remove':
                    var ids = []
                    for (var i = 0; i < rows.length; i++) {
                        <%if (!Admin.Administrator){%>if(rows[i].datalevel != 0)<%} %>
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
            Core.Easyui.get("/common/infotypelist.aspx?corpid=" + corpid);
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
