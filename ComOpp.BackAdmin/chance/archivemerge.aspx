<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="archivemerge.aspx.cs" Inherits="ComOpp.BackAdmin.chance.archivemerge" %>

<!--综合查询 列表框-->
<div id="wrap">
<!--综合查询 操作按钮-->
<div id="toolb" style="padding:0;height:auto">
<div id="schlist" class="schlist" style="display:block;">
<form>
<%if (Admin.Administrator)
          {%>
        <div class="schlist">
            <div style="padding-top: 1px; line-height: 28px;">
                <label>
                    公司：</label>
                <asp:DropDownList runat="server" ID="ddlCorporationSearch" AutoPostBack="true">
                </asp:DropDownList>
            </div>
        </div>
        <%} %>
<div style="padding:6px 0;border-top:1px #bbb solid;">
<label>搜索条件：</label><select id="So_Field" panelHeight="auto" style="width:100px">
                <option value="uname">客户姓名</option>
                <option value="phone">客户电话</option>
                <option value="weixin">微信</option>
                <option value="ordernum">订单号</option>
                <option value="series">拟购车系</option>
                <option value="customerlevel">追踪级别</option>
                <option value="infotype">信息类型</option>
                <option value="infosource">信息来源</option>
                <option value="connectway">追踪方式</option>
                <option value="giveupcause">战败原因</option>
                <option value="paymentway">支付方式</option>
                <option value="ibuytime">拟购时间</option>
                <option value="tracktag">线索标签</option>
                <option value="followalarm">追踪报警</option>
                <option value="arrive">是否到店</option>
                <option value="iseries">选购车系</option>
                <option value="owneruid">线索所有人</option>
                <option value="mpuid">市场专员</option>
                <option value="dccuid">DCC专员</option>
                <option value="exuid">展厅专员</option>
                <option value="dsuid">直销专员</option>
                <option value="state">线索状态</option>
</select>
<span class="fieldtext"><input id="textin" name="keywords" class="textin" type="text" value="" style="width:120px" />&nbsp;</span>
                <span class="fieldselect" style="display:none"><input class="easyui-combobox" name="keywords_select" id="keywords_select" style="width:120px">&nbsp;</span>
                <a class="addfilter-link" onclick="Core.Easyui.Action('addfilter',this);" style="margin-right:5px;">添加条件</a>
<select class="easyui-combobox" id="So_Time" panelHeight="auto" style="width:85px">
                <option value="posttime">提交时间</option>
                <option value="invitetime">预约时间</option>
                <option value="dateline">建档时间</option>
                <option value="arrivetime">客户来店时间</option>
                <option value="endtime">预订成交时间</option>
                <option value="delivertime">提车时间</option>
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
<!--综合查询列表-->
<table id="datagrid"></table>
</div>
<script type="text/javascript">
    var corpid =0;
    <%if (Admin.Administrator)
      {%>
    corpid = $("#ddlCorporationSearch option:selected").val();
    <%}else{ %>
    corpid = <%=Admin.CorporationID %>;
    <%} %>
    Core.Easyui.Height = 35;
    Core.Easyui.W = 800;
    Core.Easyui.H = 400;
    Core.Easyui.FieldColumns = [
{ field: 'infotype', title: '信息类型', width: 100, align: 'center' },
{ field: 'infosource', title: '信息来源', width: 100, align: 'center' },
{ field: 'sex', title: '性别', width: 33, align: 'center', formatter: function (value, row, index) {
    switch (value) { case 1: return "<i class='male' title='男'></i>"; break; case 2: return "<i class='female' title='女'></i>"; break; case 0: return '-'; break; }
} 
},
{ field: 'phonevest', title: '号码归属地', width: 100, align: 'center' },
{ field: 'owner', title: '线索拥有者', width: 100, align: 'center' },
{ field: 'tracktag', title: '标签', width: 100, align: 'center' },
{ field: 'ibuycarbrand', title: '拟购品牌', width: 80, align: 'center' },
{ field: 'ibuycarmodel', title: '拟购车型', width: 100, align: 'center' },
{ field: 'ibuytime', title: '拟购时间', width: 100, align: 'center' },
{ field: 'quotedpriceinfo', title: '报价', width: 100, align: 'center' },
{ field: 'promotioninfo', title: '促销内容', width: 120, align: 'center' },
{ field: 'remarkinfo', title: '备注', width: 100, align: 'center' },
{ field: 'lastcustomerlevel', title: '追踪级别', width: 100, align: 'center' },
{ field: 'connectalarm', title: '追踪报警', width: 100, align: 'center', formatter: function (value, row, index) {
    switch (value) { case "0": return '正常'; break; case "1": return "正常(24小时内超时)"; break; case "2": return '追踪超时'; break; }
} 
},
{ field: 'tracetimes', title: '追踪次数', width: 100, align: 'center' },
{ field: 'lastconnectway', title: '追踪方式', width: 100, align: 'center' },
{ field: 'lastconnecttime', title: '最后追踪时间', width: 120, align: 'center', sortable: true},
{ field: 'lastconnectuser', title: '最后追踪人', width: 100, align: 'center' },
{ field: 'lastconnectdetail', title: '追踪情况', width: 120, align: 'center' },
{ field: 'reservationtime', title: '预约到店时间', width: 120, align: 'center'},
{ field: 'visittime', title: '客户来店时间', width: 120, align: 'center'},
{ field: 'leavetime', title: '客户离店时间', width: 120, align: 'center'},
{ field: 'visitduration', title: '接待时长', width: 80, align: 'center', formatter: function (value, row, index) {
    return Core.rule.isNumber('p.integer', value) ? value + '分钟' : '-';
} 
},
{ field: 'visitnumber', title: '来店人数', width: 80, align: 'center', formatter: function (value, row, index) {
    return Core.rule.isNumber('p.integer', value) ? value : '-';
} 
},
{ field: 'isvisit', title: '是否到店', width: 80, align: 'center', formatter: function (value, row, index) {
    switch (value) { case 1: return "<i class='yidaod'></i>"; break; case 0: return "<i class='weidaod'></i>"; break; }
} 
},
{ field: 'province', title: '省份', width: 80, align: 'center'},
{ field: 'city', title: '城市', width: 80, align: 'center'},
{ field: 'district', title: '地区', width: 80, align: 'center'},
{ field: 'backupphone', title: '备用电话', width: 110, align: 'center' },
{ field: 'address', title: '具体地址', width: 180, align: 'center' },
{ field: 'weixinaccount', title: '微信号', width: 100, align: 'center' },
{ field: 'lastupdateuser', title: '最后操作人', width: 100, align: 'center' },
{ field: 'sbuycarbrand', title: '选购品牌', width: 100, align: 'center' },
{ field: 'sbuycarseries', title: '选购车系', width: 100, align: 'center' },
{ field: 'sbuycarmodel', title: '选购车型', width: 100, align: 'center' },
{ field: 'ordernumber', title: '订单号', width: 100, align: 'center' },
{ field: 'knockdownprice', title: '成交价', width: 100, align: 'center' },
{ field: 'placeordertime', title: '预订成交时间', width: 100, align: 'center', sortable: true},
{ field: 'picupcartime', title: '提车时间', width: 120, align: 'center', sortable: true},
{ field: 'givecause', title: '战败原因', width: 100, align: 'center' },
{ field: 'failurecauseanalyze', title: '战败原因分析', width: 100, align: 'center' },
{ field: 'createtime', title: '建档时间', width: 120, align: 'center', sortable: true},
{ field: 'posttime', title: '提交时间', width: 120, align: 'center', sortable: true},
{ field: 'marketdirector', title: '市场专员', width: 100, align: 'center', formatter: function (value, row, index) {
    return $.inArray(row.state, ['1']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
} 
},
{ field: 'dccdirector', title: 'DCC专员', width: 100, align: 'center', formatter: function (value, row, index) {
    return $.inArray(row.state, ['2', '3']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
} 
},
{ field: 'exhibitiondirector', title: '展厅专员', width: 100, align: 'center', formatter: function (value, row, index) {
    return $.inArray(row.state, ['4']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
} 
},
{ field: 'director', title: '直销专员', width: 100, align: 'center', formatter: function (value, row, index) {
    return $.inArray(row.state, ['5', '6', '7']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
} 
},
{ field: 'showno', title: '自动编号', width: 100, align: 'center' },
{ field: 'systemremark', title: '系统备注', width: 100, align: 'center', formatter: function (value, row, index) { return "<span style='color:red'>" + value + "</span>"; } }
]
    Core.Easyui.Init($('#datagrid'), Core.Easyui.Height, Core.Easyui.FieldColumns, { toolbar: '#toolbar', fitColumns: false, frozenColumns: [[
{ field: 'customerstatus', title: '线索状态', width: 70, align: 'center'},
{ field: 'name', title: '客户姓名', width: 80, align: 'center' },
{ field: 'phone', title: '客户电话', width: 110, align: 'center' },
{ field: 'ibuycarseries', title: '拟购车系', width: 100, align: 'center' }
]]
    });
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

                if ($.inArray(Field["key"], ["uname", "phone", "weixin", "ordernum", "carnum"]) == -1) {
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

                if ($.inArray(option.value, ["series", "iseries"]) != -1) {
                    $("span.fieldtext").hide();
                    $("span.fieldselect").show();
                    $("#keywords_select").combobox({
                        method: 'get',
                        valueField: 'id',
                        textField: 'name',
                        url: "/ajax/getsearchlist.aspx?action=series&corpid=" + corpid,
                        onLoadSuccess: function () {
                            $("#keywords_select").combobox('setValue', '').combobox('setText', "请选择...");
                        }
                    });
                } else if ($.inArray(option.value, ["customerlevel", "infotype", "infosource", "connectway", "giveupcause", "paymentway", "ibuytime", "tracktag"]) != -1) {
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
                } else if ($.inArray(option.value, ["followalarm", "arrive", "state"]) != -1) {
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
                } else if ($.inArray(option.value, ["owneruid", "mpuid", "dccuid", "exuid", "dsuid"]) != -1) {
                    $("span.fieldtext").hide();
                    $("span.fieldselect").show();
                    $("#keywords_select").combobox({
                        method: 'get',
                        valueField: 'uid',
                        textField: 'realnameandgroupname',
                        url: "/ajax/getsearchlist.aspx?action=userlist&corpid=" + corpid,
                        onLoadSuccess: function () {
                            $("#keywords_select").combobox('setValue', '').combobox('setText', "请选择...");
                        }
                    });
                }
            }
        });

        Core.Easyui.get('/chance/chancelist.aspx?corpid=' + corpid + "&issearch=1");
        Core.Easyui.Action('removefilter');
    })
</script>
