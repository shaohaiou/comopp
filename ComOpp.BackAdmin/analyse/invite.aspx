<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="invite.aspx.cs" Inherits="ComOpp.BackAdmin.analyse.invite" %>

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
    <style type="text/css">
        html
        {
            overflow-x: hidden;
            overflow-y: auto;
        }
        /*tab标签切换的样式*/
        #myTab
        {
            _position: absolute;
        }
        .tabs-header
        {
            _margin-left: 11px;
        }
        .tabs-panels
        {
            _padding-left: 12px;
        }
        /*图表下方的table表格样式*/
        .nav_tabs a
        {
            color: #fff;
            text-align: center;
        }
        .chart_is_null
        {
            line-height: 120px;
            text-align: center;
            border: 1px #bbb dotted;
            margin-right: 10px;
        }
        .chartstable tr
        {
            height: 29px;
            line-height: 29px;
            border: 1px #bbb solid;
        }
        .chartstable td
        {
            border-left: 1px #bbb solid;
            text-align: center;
        }
        .chartstable tr.title
        {
            background: #e2e2e2;
            font-weight: bold;
        }
        .chartstable tr.even
        {
            background: #eaeae2;
        }
        .tablebox
        {
            margin: 10px;
        }
        .tablebox span.export
        {
            float: left;
            color: #000;
            margin-left: -50%;
            position: absolute;
            display: none;
        }
        .tablebox span.export a
        {
            margin-left: 50px;
        }
    </style>
</head>
<body>
    <div class="Tab">
        <a href="archive.aspx?corpid=<%=GetInt("corpid") %>">销售线索分析</a> <a href="invite.aspx?corpid=<%=GetInt("corpid") %>" class="selected">客户邀约分析</a>
        <a href="finality.aspx?corpid=<%=GetInt("corpid") %>">成交与战败分析</a> <a href="follow.aspx?corpid=<%=GetInt("corpid") %>">客户追踪分析</a>
    </div>
    <form id="Form1" runat="server">
    <div class="nav_tabs" style="padding-left: 15px; margin-top: 5px; padding-bottom: 5px;
        border-bottom: 1px #ddd solid">
        <div class="eachTab<%if(GetString("active") == "arrive"){ %> tchose<%} %>" data-key="arrive" data-link="invite.aspx?active=arrive&corpid=<%=GetInt("corpid") %>">
            邀约到店量</div>
        <div class="eachTab<%if(GetString("active") == "structure"){ %> tchose<%} %>" data-key="structure" style="width: 130px" data-link="invite.aspx?active=structure&corpid=<%=GetInt("corpid") %>">
            分时邀约结构比达成率</div>
        <%if (Admin.Administrator)
          { %>
        <div style="display: inline-block; padding: 5px;">
            <asp:DropDownList runat="server" ID="ddlCorporation" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <%} %>
    </div>
    <div class="schlist" style="padding-top: 1px; <%if(string.IsNullOrEmpty(GetString("active"))){%>display: none;
        <%}%>">
        <div style="padding: 10px 0px 0px 10px">
            <select id="SoTime" panelheight="auto" style="width: 110px;">
                <option value="">--自定义时间--</option>
                <option value="day1">今天</option>
                <option value="day2">昨天</option>
                <option value="day3">最近7天</option>
                <option value="day4">最近30天</option>
            </select>
            <span>
                <input class="easyui-datebox" name="form[starttime]" id="starttime" value="2016-09-13"
                    style="width: 90px">&nbsp;至&nbsp;<input class="easyui-datebox" name="form[endtime]"
                        id="endtime" value="2016-09-14" style="width: 90px"></span><!--20140715 xuan-->
            <span>
                <select id="SoField" panelheight="auto" style="width: 100px">
                    <option value="member">--全部用户--</option>
                    <option value="uid">依据用户</option>
                    <option value="groupid">依据用户组</option>
                </select></span> <span class="Souid" style="display: none">
                    <select id="Souid" panelheight="auto" style="width: 150px">
                        <option value="0">--请选择用户--</option>
                        <asp:Repeater ID="rptUser" runat="server">
                            <ItemTemplate>
                                <option value="<%#Eval("ID") %>">
                                    <%#Eval("RealnameAndGroupname") %></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select></span><span class="Sogroupid" style="display: none"><select id="Sogroupid"
                        class="easyui-combobox" panelheight="auto" style="width: 150px">
                        <option value="0">--请选择用户组--</option>
                        <asp:Repeater ID="rptGroup" runat="server">
                            <ItemTemplate>
                                <option value="<%#Eval("ID") %>">
                                    <%#Eval("GroupName")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                    </span><a onclick="return Core.Easyui.Form(this);" class="easyui-linkbutton" style="margin-left: 20px;">
                        确定</a> <span class="Validform_checktip"></span>
        </div>
        <div id="GjSearch" style="padding-top: 6px;">
            <label>
                筛选条件：</label><span></span>
        </div>
    </div>
    </form>
    <div id="container" style="margin: 0 auto; padding-left: 10px; padding-top: 18px">
    </div>
    <div class="tablebox">
    </div>
    <script type="text/javascript">
Core.Easyui.day = {"day1":{"title":"\u4eca\u5929","start":"2016-09-14","end":"2016-09-14"},"day2":{"title":"\u6628\u5929","start":"2016-09-13","end":"2016-09-13"},"day3":{"title":"\u6700\u8fd17\u5929","start":"2016-09-07","end":"2016-09-14"},"day4":{"title":"\u6700\u8fd130\u5929","start":"2016-08-15","end":"2016-09-14"}};
Core.Easyui.Form = function(form){
var form = $(form).parents("form");
var p = {"starttime" : $("#starttime").datebox('getValue'),"endtime" : $("#endtime").datebox('getValue'),"uid" : [],"groupid" : [],"series" : [],"infosource" : []};

form.find("span.Validform_checktip").removeClass('Validform_wrong');
form.find("span.Validform_checktip").html('<i></i>')
if(!Core.rule.isDatetime(p["starttime"]) || !Core.rule.isDatetime(p["endtime"])){
form.find("span.Validform_checktip").addClass('Validform_wrong');
form.find("span.Validform_checktip").html('<i></i>请选择时间范围');
return false;
}
form.find("a.myfilter-link").each(function(index, element) {
        p[$(this).attr('data-key')].push($(this).attr('data-value'));
    });
$.getJSON('?json=1&corpid=<%=GetInt("corpid") %>&active=<%=GetString("active") == "" ? "arrive" : GetString("active") %>&r='+Math.random(),p,function(data){
Core.Highcharts.loader('<%=GetString("active") == "" ? "arrive" : GetString("active") %>',data);
});
//return Core.submit();
}
Core.Easyui.Action = function(method,ikey){
switch(method){
case 'addfilter':
var form   = $("form");
var filter = form.find('#GjSearch');
var o      = filter.find("a.myfilter-link");
var Field  = {"id" : "","value" : "","box" : []};
if($.inArray(ikey,["series","infosource"])!=-1){
Field["key"]  = ikey;Field["name"] = "选择数据";
}else{
Field["key"]  = form.find("#SoField").combobox('getValue');
Field["name"] = form.find("#SoField").combobox('getText');
}
for(var i=0;i<o.length;i++){
Field["box"].push($(o[i]).attr('data-value'));
}
Field["id"]    = $("#So"+Field["key"]).combobox('getValue');
Field["value"] = $("#So"+Field["key"]).combobox('getText');

if(!Core.rule.isNumber('p.integer',Field["id"])) return Core.Easyui.Action('clearAll',form);
if($.inArray(Field["id"],Field["box"])!=-1){
filter.find("a.myfilter-link[data-value='"+Field["id"]+"']").remove();
}
filter.find("span:first").before('<a class="myfilter-link" title="'+Field["value"]+'" data-key="'+Field["key"]+'" data-value="'+Field["id"]+'">'+Field["value"]+'<i></i></a>');

if(!filter.find('span#sh_clearAll').length){
filter.find("span:first").before('<span id="sh_clearAll" class="sh_clearAll">全部清除</span>');
}			
filter.show();
break;case 'removefilter':
    $('#GjSearch').find(".myfilter-link i").live("click",function(){
var o = $(this).parent('a');
$.messager.confirm('确认提示','您确定要删除筛选条件吗?',function(r){
if(!r) return;
$(o).remove();
});
});
$('#GjSearch').find('.sh_clearAll').live("click",function(){
$.messager.confirm('确认提示','您确定要清除筛选条件吗?',function(r){
if(!r) return;
$('#GjSearch').find("a.myfilter-link").remove();
$('#GjSearch').find('.sh_clearAll').remove();
});
});
break;case 'clearAll':
var filter = $('#GjSearch');
filter.find("a.myfilter-link").remove();
return true;
break;
}
}
$(document).ready(function(){
$(".Tab").idTabs();
$("#SoTime").combobox({
onSelect  : function(option){
$("#starttime").datebox("setValue","");
$("#endtime").datebox("setValue","");
if($.inArray(option.value,["day1","day2","day3","day4"])==-1) return;
try{
$("#starttime").datebox("setValue",Core.Easyui.day[option.value]['start']);
$("#endtime").datebox("setValue",Core.Easyui.day[option.value]['end']);
}catch(e){}
}
});
Core.Easyui.Action('removefilter');

$("#SoField").combobox({
onSelect  : function(option){
var form = $(this).parents('form');
Core.Easyui.Action('clearAll');
$("span.Souid").hide();$("span.Sogroupid").hide();
if($.inArray(option.value,['uid','groupid'])==-1) return;
$("span.So"+option.value).show();
$("#So"+option.value).combobox('setValue','').combobox('setText',option.value=='uid' ? "--请选择用户--" : "--请选择用户组--");
return true;
}
});
$("#Soseries").combobox({
onSelect  : function(option){Core.Easyui.Action('addfilter',"series");}
});
$("#Soinfosource").combobox({
onSelect  : function(option){Core.Easyui.Action('addfilter',"infosource");}
});
$("#Souid").combobox({
onSelect  : function(option){Core.Easyui.Action('addfilter',"Field");}
});
$("#Sogroupid").combobox({
onSelect  : function(option){Core.Easyui.Action('addfilter',"Field");}
});
})
var chart;
Core.Highcharts = {
box  : {"unit" : "","columns" : [],"series" : [],"height" : 200,"L" : 0,"bar" : {"H" : 70,"L1" : 30,"L2" : 65,"L3" : 88},"table" : ""},
isnull : function(){
$("#container").html("<div class='chart_is_null'>暂无数据</div>");
},
init : function(params){
Core.Highcharts.box["unit"]    = Core.Highcharts.box["table"] = '';Core.Highcharts.box["L"] = 0;
Core.Highcharts.box["columns"] = Core.Highcharts.box["series"]  = [];
$("#container").html('');
$(".tablebox").html('<table class="chartstable" width="100%" border="0" cellspacing="1" cellpadding="2"></table>');
},
addParams : function(name,params){
try{Core.Highcharts.box[name] = params;}catch(e){}
},
chart : function(params){
chart = new Highcharts.Chart({ 
chart: { 
renderTo: 'container', type: 'bar', 
height: Core.Highcharts.box["height"],
plotBorderWidth: 0, marginRight: 60, backgroundColor: '#f7f6f2',
style:{fontSize: '12px',color: '#262626'} 
},	
plotOptions: {bar: {pointPadding: 1,groupPadding: 1,borderWidth: 0,pointWidth: 15,minPointLength: 2,shadow: false}},	
title: {text: ' ', style: {fontWeight: 'bold', fontFamily: 'simsun', fontSize: '12px',color: '#262626'} }, 
xAxis: { categories: Core.Highcharts.box["columns"]},
yAxis:{
title: {text: ''},
lineColor: '#ddd',
lineWidth: 1,
labels: {step:1,formatter: function () {return this.value + Core.Highcharts.box["unit"];}}
},
//位置
legend: {
layout: 'horizontal', backgroundColor: null, align: 'right', borderWidth: 0, verticalAlign: 'top', floating: true, itemMarginBottom: 10, shadow: false, x: -10,y: -10,
style: {color: '#262626',fontSize: '12px', fontWeight: 'normal'}
},
tooltip: { //工具提示
animation: true,backgroundColor: '#f7f6f2',
},
series : Core.Highcharts.box["series"]
});
}
}
    </script>
    <script type="text/javascript">
        Core.Highcharts.loader = function (method, params) {
            var series = [];
            Core.Highcharts.init();
            try { Core.Highcharts.box["unit"] = params["unit"]; } catch (e) { }
            var table = $(".tablebox").find("table");
            switch (method) {
                case 'arrive':
                    Core.Highcharts.box["unit"] = '';
                    if (Core.of(params['user']) == 'undefined') return Core.Highcharts.isnull();
                    Core.Highcharts.addParams("columns", params["user"]);
                    try {
                        series = [
{ type: 'column', name: '邀约到店量', data: params["total"], dataLabels: { enabled: true} },
];
                        Core.Highcharts.box["table"] = '<tr class="title"><td colspan="2"><span class="export"><a>导出Excel</a></span></td></tr>';
                        Core.Highcharts.box["table"] += '<tr class="title"><td>姓名</td><td>邀约到店量</td></tr>';
                        for (key in params['user']) {
                            Core.Highcharts.box["table"] += '<tr><td>' + params["user"][key] + '</td><td>' + params["total"][key] + '</td></tr>';
                            Core.Highcharts.box["L"]++;
                        }
                        Core.Highcharts.box["table"] += '<tr><td>合计</td><td>' + params["heji"] + '</td></tr>';
                        $(table).append(Core.Highcharts.box["table"]);
                    } catch (e) { }
                    Core.Highcharts.addParams("series", series);
                    Core.Highcharts.box["height"] = Core.Highcharts.box["bar"]["H"] + (Core.Highcharts.box["L"] * Core.Highcharts.box["bar"]["L" + series.length]);
                    Core.Highcharts.chart();
                    break; case 'structure':
                    Core.Highcharts.box["table"] = '<tr class="title"><td colspan="4"><span class="export"><a>导出Excel</a></span></td></tr>';
                    if (Core.of(params['user']) == 'undefined') return Core.Highcharts.isnull();
                    Core.Highcharts.addParams("columns", params["user"]);
                    try {
                        series = [
{ type: 'column', name: '工作日', data: params["week15"], dataLabels: { enabled: true} },
{ type: 'column', name: '周末', data: params["week67"], dataLabels: { enabled: true} },
{ type: 'column', name: '比达成率', data: params["pct"], dataLabels: { enabled: true, formatter: function () { return '' + this.y + '% '; } } }
];
                        Core.Highcharts.box["table"] += '<tr class="title"><td>客服姓名</td><td>工作日</td><td>周末</td><td>比达成率</td></tr>';
                        for (key in params['user']) {
                            Core.Highcharts.box["table"] += '<tr><td>' + params["user"][key] + '</td><td>' + params["week15"][key] + '</td><td>' + params["week67"][key] + '</td><td>' + params["pct"][key] + '%</td></tr>';
                            Core.Highcharts.box["L"]++;
                        }
                        Core.Highcharts.box["table"] += '<tr><td>合计</td><td>' + params["heji"]["week15"] + '</td><td>' + params["heji"]["week67"] + '</td><td>' + params["heji"]["pct"] + '%</td></tr>';
                        $(table).append(Core.Highcharts.box["table"]);
                    } catch (e) { }
                    Core.Highcharts.addParams("series", series);
                    Core.Highcharts.box["height"] = Core.Highcharts.box["bar"]["H"] + (Core.Highcharts.box["L"] * Core.Highcharts.box["bar"]["L" + series.length]);
                    Core.Highcharts.chart();
                    break;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".nav_tabs .eachTab").click(function () {
                if ($(this).hasClass("tchose")) return;
                window.location.href = $(this).attr("data-link");
            })
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
