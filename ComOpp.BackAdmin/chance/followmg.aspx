<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="followmg.aspx.cs" Inherits="ComOpp.BackAdmin.chance.followmg" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>红旭集团商机管理系统V1.0</title>
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
<body><div id="toolbar" style="padding:0;height:auto;min-width:760px;margin-top:1px">
    <%if (Admin.Administrator)
              {%>
            <div class="schlist" style="float:right;display:inline-block;">
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
    <div id="schlist" class="schlist" style="display:block;">
        <form>
        <div style="padding:6px 0;">
            <label>搜索条件：</label><select id="So_Field" panelHeight="auto" style="width:100px">
            <option value="uname">客户姓名</option>
            <option value="phone">客户电话</option>
            <option value="uid">追踪者姓名</option>
            <option value="connectway">追踪方式</option>
            <option value="customerlevel">客户追踪级别</option>
            </select>
            <span class="fieldtext"><input id="textin" name="keywords" class="textin" type="text" value="" style="width:120px" />&nbsp;</span>
            <span class="fieldselect" style="display:none"><input class="easyui-combobox" name="keywords_select" id="keywords_select" style="width:120px">&nbsp;</span>
            <a class="addfilter-link" onclick="Core.Easyui.Action('addfilter',this);" style="margin-right:5px;">添加条件</a>
            <select class="easyui-combobox" id="So_Time" panelHeight="auto" style="width:85px">
            <option value="followtime">追踪时间</option>
            </select>	
            <input class="easyui-datebox" name="form[starttime]" style="width:90px;">&nbsp;至&nbsp;<input class="easyui-datebox" name="form[endtime]" style="width:90px">
            <a onclick="return Core.Easyui.Form(this);" class="easyui-linkbutton" style="margin-left:20px;">确定</a>
            <div id="GjSearch" style="margin-top:6px;height:24px;overflow:hidden">
                <label>筛选条件：</label><span style="color:#f96120;">最多只能增加5个筛选条件</span>
            </div>
        </div>
        </form>
    </div>
    <div class="aSearch"></div>
</div>
<!--线索发起表格-->
<table id="datagrid"></table>
<script type="text/javascript">
        var corpid =0;
        <%if (Admin.Administrator)
          {%>
        corpid = $("#ddlCorporationSearch option:selected").val();
        <%}else{ %>
        corpid = <%=Admin.CorporationID %>;
        <%} %>
    Core.Easyui.Init($('#datagrid'), 37, [
    //{field:'tid',title:'TID',width:50,align:'center'},
{field: 'createtime', title: '追踪时间', width: 120, align: 'center'},
{ field: 'customername', title: '客户姓名', width: 100, align: 'center' },
{ field: 'customerphone', title: '客户电话', width: 100, align: 'center' },
{ field: 'connnectuser', title: '追踪者姓名', width: 100, align: 'center' },
{ field: 'connectway', title: '追踪方式', width: 80, align: 'center' },
{ field: 'customerlevel', title: '客户追踪级别', width: 80, align: 'center' },
{ field: 'connectdetail', title: '追踪情况', align: 'left' }
], { toolbar: '#toolbar' });
    Core.Easyui.Form = function (o) {
        var form = $(o).parents('form');
        var p = { "sotime": form.find("#So_Time").combobox('getValue'), "starttime": form.find("input[name='form[starttime]']").val(), "endtime": form.find("input[name='form[endtime]']").val() };
        if (form.find("a.myfilter-link").length) {
            form.find("a.myfilter-link").each(function (index, element) {
                p[$(this).attr('data-key')] = $(this).attr('data-value');
            });
        }
        try {
            for (key in p) { Core.Easyui.Params.box[key] = p[key]; }
        } catch (e) { }
        Core.Easyui.load(p);
    }

    Core.Easyui.Action = function (method, o) {
        switch (method) {
            case 'addfilter':
                var form = $(o).parents('form');
                var filter = form.find('#GjSearch');
                var o = filter.find("a.myfilter-link");
                var Field = { "key": form.find("#So_Field").combobox('getValue'), "name": form.find("#So_Field").combobox('getText'), "value": "", "ivalue": "", "box": [] };
                for (var i = 0; i < o.length; i++) {
                    Field["box"].push($(o[i]).attr('data-key'));
                }
                if ($.inArray(Field["key"], Field["box"]) != -1) {
                    filter.find("a.myfilter-link[data-key='" + Field["key"] + "']").remove();
                } else {
                    if (filter.find("a.myfilter-link").length >= 5) return;
                }
                Field['value'] = Field['ivalue'] = form.find(":text[name='keywords']").val();

                if ($.inArray(Field["key"], ["uname", "phone"]) == -1) {
                    Field['value'] = form.find("#keywords_select").combobox('getValue');
                    Field['ivalue'] = form.find("#keywords_select").combobox('getText');
                }
                if (Field['value'] == '') return;
                filter.find("span:first").before('<a class="myfilter-link" title="' + Field['name'] + '" data-key="' + Field["key"] + '" data-value="' + Field['value'] + '">' + Field['ivalue'] + '<i></i></a>');

                if (!filter.find('span#sh_clearAll').length) {
                    filter.find("span:first").before('<span id="sh_clearAll" class="sh_clearAll">全部清除</span>');
                }
                filter.show();
                break; case 'removefilter':
                $('#GjSearch').find(".myfilter-link i").live("click", function () {
                    var o = $(this).parent('a');
                    $.messager.confirm('确认提示', '您确定要删除筛选条件吗?', function (r) {
                        if (!r) return;
                        $(o).remove();
                    });
                });
                $('#GjSearch').find('.sh_clearAll').live("click", function () {
                    $.messager.confirm('确认提示', '您确定要清除筛选条件吗?', function (r) {
                        if (!r) return;
                        $('#GjSearch').find("a.myfilter-link").remove();
                        $('#GjSearch').find('.sh_clearAll').remove();
                    });
                });
                break;
        }
    }
    $(document).ready(function () {
        $("#So_Field").combobox({
            onSelect: function (option) {
                $("span.fieldtext").show();
                $("span.fieldselect").hide();
                $("#keywords_select").combobox('clear');

                if ($.inArray(option.value, ["customerlevel","connectway"]) != -1) {
                    $("span.fieldtext").hide();
                    $("span.fieldselect").show();
                    $("#keywords_select").combobox({
                        method: 'get',
                        valueField: 'id',
                        textField: 'name',
                        url: "/ajax/getsearchlist.aspx?action=" + option.value + "&corpid=" + corpid,
                        onLoadSuccess: function () {
                            $("#keywords_select").combobox('setValue', '').combobox('setText', "请选择...");
                        }
                    });
                } else if ($.inArray(option.value, ["uid"]) != -1) {
                    $("span.fieldtext").hide();
                    $("span.fieldselect").show();
                    $("#keywords_select").combobox({
                        method: 'get',
                        valueField: 'id',
                        textField: 'realnameandgroupname',
                        url: "/ajax/getsearchlist.aspx?action=userlist&corpid=" + corpid,
                        onLoadSuccess: function () {
                            $("#keywords_select").combobox('setValue', '').combobox('setText', "请选择...");
                        }
                    });
                }
            }
        });
        Core.Easyui.get("followlist.aspx?corpid="+corpid);
        Core.Easyui.Action('removefilter');
    })
</script>
<script type="text/javascript">
    $(document).ready(function () {
        Core.Easyui.resize();
    })
</script>
</body>
</html>