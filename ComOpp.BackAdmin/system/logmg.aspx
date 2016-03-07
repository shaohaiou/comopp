<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logmg.aspx.cs" Inherits="ComOpp.BackAdmin.system.logmg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>红旭集团销售客户管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="../plugins/jquery-easyui-1.3.6/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../plugins/jquery-easyui-1.3.6/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
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
    <form>
    <!--商机管理状态切换标签-->
    <div class="nav_tabs">
        <div class="eachTab<%=GetInt("type",-1) == -1 ? " tchose" : string.Empty %>" data-type="-1">
            全部日志</div>
        <div class="eachTab<%=GetInt("type",-1) == 0 ? " tchose" : string.Empty %>" data-type="0">
            信息记录</div>
        <div class="eachTab<%=GetInt("type",-1) == 1 ? " tchose" : string.Empty %>" data-type="1">
            警告日志</div>
        <div class="eachTab<%=GetInt("type",-1) == 2 ? " tchose" : string.Empty %>" data-type="2">
            错误日志</div>
        <div class="eachTab<%=GetInt("type",-1) == 3 ? " tchose" : string.Empty %>" data-type="3">
            调试信息</div>
    </div>
    <!--线索发起列表表格操作按钮-->
    <div id="toolbar" style="padding: 0; height: auto; min-width: 760px;">
        <div class="schlist">
            <div style="padding-top: 1px;">
                <label>记录时间</label>
                <input class="easyui-datebox" name="form[starttime]" style="width: 90px;">&nbsp;至&nbsp;<input
                    class="easyui-datebox" name="form[endtime]" style="width: 90px">
                <a href="javascript:void(0);" onclick="return Core.Easyui.Form(this);" class="easyui-linkbutton"
                    style="margin-left: 20px;">确定</a>
            </div>
        </div>
    </div>
    </form>
    <!--线索发起表格-->
    <table id="datagrid" data-columnskey="index">
    </table>
    <script type="text/javascript">
        var type = <%=GetInt("type",-1) %>;
        Core.Easyui.Init($('#datagrid'), 99, [
            { field: 'eventdate', title: '记录时间', width: 130, align: 'center' },
            { field: 'eventtype', title: '事件类型', width: 80, align: 'center' },
            { field: 'message', title: '内容', align: 'left',formatter:function(value,row,index){
                return row.eventtype == '错误' ? ("<a onclick='Core.Easyui.Action(\"showdetail\",this);' data-tid='" + row.id + "' class='coR00f'>" + value +"</a>") : value;
            } }
        ],{singleSelect:true});
        Core.Easyui.Form = function (o) {
            var form = $(o).parents('form');
            var p = { 
            'startime':form.find("input[name='form[starttime]']").val(),
            'endtime':form.find("input[name='form[endtime]']").val()};
            Core.Easyui.get("/system/loglist.aspx?type=" + type + "&startime=" + form.find("input[name='form[starttime]']").val() + "&endtime=" + p.endtime);
            return false;
        }
        Core.Easyui.Action = function(method, o){
            switch (method) {
                case 'showdetail':
                    dialog('800', '500', '日志详情', "logdetail.aspx?id=" + $(o).attr("data-tid") + "&r=" + Math.random());
                    break;
            }
        };
        $(document).ready(function () {
            $(".nav_tabs .eachTab").live('click', function () {
                if ($(this).hasClass("unchose") || $(this).hasClass("tchose") || $(this).hasClass("nochose")) return;
                $(".nav_tabs .eachTab").removeClass("tchose");
                $(this).addClass("tchose");
                var form = $(this).parents('form');
                var p = { 
                'startime':form.find("input[name='form[starttime]']").val(),
                'endtime':form.find("input[name='form[endtime]']").val()};
                type = $(this).attr('data-type');
                Core.Easyui.get("/system/loglist.aspx?type=" + type + "&startime=" + form.find("input[name='form[starttime]']").val() + "&endtime=" + p.endtime);
            });

            Core.Easyui.get("/system/loglist.aspx?type=" + type);
        })
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
