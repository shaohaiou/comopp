<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chancemg.aspx.cs" Inherits="ComOpp.BackAdmin.chance.chancemg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
<body>
    <!--商机管理状态切换标签-->
    <div class="nav_tabs">
        <div class="eachTab<%=GetInt("state",0) == 0 ? " tchose" : string.Empty %>" data-link="?" data-key="index" data-state="">
            全部显示</div>
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "导入|集客"))
                          { %>
        <div class="eachTab<%=GetInt("state",0) == 1 ? " tchose" : string.Empty %>" data-link="?state=1" data-key="archive" data-state="1">
            导入|集客</div>
            <%} %>
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "清洗|邀约"))
                          { %>
        <div class="eachTab<%=GetInt("state",0) == 2 ? " tchose" : string.Empty %>" data-link="?state=2" data-key="purge" data-state="2">
            清洗|邀约</div>
            <%} %>
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "到店|洽谈"))
                          { %>
        <div class="eachTab<%=GetInt("state",0) == 3 ? " tchose" : string.Empty %>" data-link="?state=3" data-key="appointment" data-state="3">
            到店|洽谈</div>
            <%} %>
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "追踪|促成"))
                          { %>
        <div class="eachTab<%=GetInt("state",0) == 4 ? " tchose" : string.Empty %>" data-link="?state=4" data-key="confer" data-state="4">
            追踪|促成</div>
            <%} %>
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "预订|成交"))
                          { %>
        <div class="eachTab<%=GetInt("state",0) == 5 ? " tchose" : string.Empty %>" data-link="?state=5" data-key="order" data-state="5">
            预订|成交</div>
            <%} %>
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "提车|回访"))
                          { %>
        <div class="eachTab<%=GetInt("state",0) == 10 ? " tchose" : string.Empty %>" data-link="?state=10" data-key="thenend" data-state="10">
            提车|回访</div>
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
    </div>
    <!--线索发起列表表格操作按钮-->
    <div id="toolbar" style="padding: 0; height: auto; min-width: 760px;">
        <div style="margin-bottom: 1px; position: relative;">
                                <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "线索录入"))
                                  { %>
            <%if (GetInt("state") > 0 && GetInt("state") < 4)
              { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconCls="icon-add" plain="true" onclick="Core.Easyui.Action('add');">添加</a>
            <%} %>
            <%} %>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="Core.Easyui.Action('edit');">编辑</a> 
            <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "线索删除"))
              { %>
                <a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-remove" plain="true" onclick="Core.Easyui.Action('remove');">删除</a>
                    <%} %>
                                <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "线索录入"))
                                  { %>
<%if (GetInt("state") > 0 && GetInt("state") < 3)
              { %>
                    <a href="javascript:void(0);" class="easyui-linkbutton" iconCls="icon-daoru" plain="true" onclick="Core.Easyui.Action('import')">导入</a>
                    <%} %>
                    <%} %>
                                <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "线索导出"))
                                  { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-daochu" plain="true"
                onclick="Core.Easyui.Action('excel')">导出</a> 
                    <%} %>
                                <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "权限移交"))
                                  { %><a href="javascript:void(0);" class="easyui-linkbutton"
                    iconcls="icon-qxmove" plain="true" onclick="Core.Easyui.Action('handover',this);">
                    线索批量移交</a> 
                    <%} %><a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-moren"
                        plain="true" onclick="Core.Easyui.Action('diyColumns.recover');">恢复表格默认选项</a>
            <a href="javascript:void(0)" class="diyList fr" onclick="Core.Easyui.Action('diyColumns');">
                自定义列表</a> <a href="javascript:void(0)" id="diySearch" class="diySearch fr">搜索</a>
            <div id="showdlist" class="showdlist">
            </div>
        </div>
        <div id="schlist" class="schlist" style="display: none;">
            <form>
            <div style="padding: 6px 0;">
                <label>
                    搜索条件：</label><select id="So_Field" panelheight="auto" style="width: 100px">
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
                        <%if ( new int[]{1,2,4}.Contains(GetInt("state"))){%><option value="archivemove">主动/7日强转</option><%} %>
                        <%if (GetInt("state") == 0){%><option value="state">线索状态</option><%} %>
                    </select>
                <span class="fieldtext">
                    <input id="textin" name="keywords" class="textin" type="text" value="" style="width: 120px" />&nbsp;</span>
                <span class="fieldselect" style="display: none">
                    <input class="easyui-combobox" name="keywords_select" id="keywords_select" style="width: 120px">&nbsp;</span>
                <a class="addfilter-link" onclick="Core.Easyui.Action('addfilter',this);" style="margin-right: 5px;">
                    添加条件</a>
                <select class="easyui-combobox" id="So_Time" panelheight="auto" style="width: 85px">
                    <option value="posttime">提交时间</option>
                    <option value="invitetime">预约时间</option>
                    <option value="dateline">建档时间</option>
                    <option value="arrivetime">客户来店时间</option>
                    <option value="endtime">预订成交时间</option>
                    <option value="delivertime">提车时间</option>
                </select>
                <input class="easyui-datebox" name="form[starttime]" style="width: 90px;">&nbsp;至&nbsp;<input
                    class="easyui-datebox" name="form[endtime]" style="width: 90px">
                <a onclick="return Core.Easyui.Form(this);" class="easyui-linkbutton" style="margin-left: 20px;">
                    确定</a>
                <!--<a href="#" class="easyui-linkbutton" style="padding:0 5px;">重置</a>-->
                <div id="GjSearch" style="padding-top: 6px; display: none;">
                    <label>
                        筛选条件：</label><span style="color: #f96120;">最多只能增加5个筛选条件</span>
                </div>
            </div>
            </form>
        </div>
        <div class="aSearch">
        </div>
    </div>
    <!--线索发起表格-->
    <table id="datagrid" data-columnskey="index">
    </table>
<script type="text/javascript">
    Array.prototype.baoremove = function (dx) {
        if (isNaN(dx) || dx > this.length) { return false; }
        this.splice(dx, 1);
    }
    var D = new Date();
    var Time = Math.round(new Date(Date.parse(D.getFullYear() + '-' + (D.getMonth() + 1) + '-' + D.getDate())) / 1000);
    Core.Easyui.Height = 67;
    Core.Easyui.Settings = null;
    Core.Easyui.Settings = { "forced": { "offday": <%=Forcedoffday %>, "offcustomerlevel": [<%=Offcustomerlevel %>], "outday": <%=Forcedoutday %>, "outcustomerlevel": [<%=Forcedoutdaylevel %>] }, "voluntary": { "trackmove": <%=Trackmove %>, "movecheck": <%=Movecheck %>, "offday": <%=Voluntaryoffday %>, "outday": <%=Voluntaryoutday %>, "offcheck": <%=Offcheck %>, "outcheck": <%=Outcheck %>} };
    Core.Easyui.State = { 1: "导入|集客", 2: "清洗|邀约", 3: "到店|洽谈", 4: "追踪|促成", 5: "预订|成交", 10: "提车|回访" };
</script>
<script type="text/javascript">
    Core.Easyui.FieldColumns = [
        { field: 'connectalarm', title: '追踪报警', width: 100, align: 'center', sortable: true, formatter: function (value, row, index) {
            if ((parseInt(row.state) >= 2 && parseInt(row.state) <= 4)) {
                switch (value) { case "0": return '正常'; break; case "1": return "正常(24小时内超时)"; break; case "2": return '<span style="color:red">追踪超时</span>'; break; }
            } else {
                return '-';
            }
        }
        },
        { field: 'lastcustomerlevel', title: '追踪级别', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
            return (parseInt(row.state) >= 2 && parseInt(row.state) <= 4) ? value : "-";
        } 
        },
        { field: 'ifollow', title: '追踪', width: 50, align: 'center', formatter: function (value, row, index) {
            return (parseInt(row.state) >= 2 && parseInt(row.state) <= 4) ? "<a onclick='Core.Easyui.Action(\"follow\",this);' data-tid='" + row.id + "' class='coR00f'>追踪</a>" : '-';
        }
        },
        { field: 'lastconnecttime', title: '最后追踪时间', width: 120, align: 'center', sortable: true },
        { field: 'sex', title: '性别', width: 45, align: 'center', sortable: true, formatter: function (value, row, index) {
            switch (value) { case 1: return "<i class='male' title='男'></i>"; break; case 2: return "<i class='female' title='女'></i>"; break; case 0: return '-'; break; }
        }},
        { field: 'phonevest', title: '号码归属地', width: 100, align: 'center', sortable: true },
        { field: 'owner', title: '线索拥有者', width: 100, align: 'center', sortable: true },
        { field: 'infosource', title: '信息来源', width: 100, align: 'center', sortable: true },
        { field: 'infotype', title: '信息类型', width: 100, align: 'center', sortable: true },
        { field: 'tracktag', title: '标签', width: 100, align: 'center' },
        { field: 'ibuycarbrand', title: '拟购品牌', width: 80, align: 'center', sortable: true },
        { field: 'ibuycarmodel', title: '拟购车型', width: 100, align: 'center', sortable: true },
        { field: 'ibuytime', title: '拟购时间', width: 100, align: 'center', sortable: true },
        { field: 'quotedpriceinfo', title: '报价', width: 100, align: 'center' },
        { field: 'promotioninfo', title: '促销内容', width: 120, align: 'center' },
        { field: 'remarkinfo', title: '备注', width: 350, align: 'center' },
        { field: 'tracetimes', title: '追踪次数', width: 100, align: 'center' },
        { field: 'lastconnectway', title: '追踪方式', width: 100, align: 'center' },
        { field: 'lastconnectuser', title: '最后追踪人', width: 100, align: 'center' },
        { field: 'lastconnectdetail', title: '追踪情况', width: 120, align: 'center' },
        { field: 'reservationtime', title: '预约到店时间', width: 120, align: 'center', sortable: true},
        { field: 'visittime', title: '客户来店时间', width: 120, align: 'center', sortable: true},
        { field: 'leavetime', title: '客户离店时间', width: 120, align: 'center', sortable: true},
        { field: 'visitduration', title: '接待时长', width: 80, align: 'center', formatter: function (value, row, index) {
            return Core.rule.isNumber('p.integer', value) ? value + '分钟' : '-';
        }
        },
        { field: 'visitnumber', title: '来店人数', width: 80, align: 'center', formatter: function (value, row, index) {
            return Core.rule.isNumber('p.integer', value) ? value : '-';
        }
        },
        { field: 'isvisit', title: '是否到店', width: 80, align: 'center', sortable: true, formatter: function (value, row, index) {
            switch (value) { case 1: return "<i class='yidaod'></i>"; break; case 0: return "<i class='weidaod'></i>"; break; }
        }
        },
        { field: 'province', title: '省份', width: 80, align: 'center', sortable: true},
        { field: 'city', title: '城市', width: 80, align: 'center', sortable: true },
        { field: 'district', title: '地区', width: 80, align: 'center', sortable: true},
        { field: 'backupphone', title: '备用电话', width: 110, align: 'center' },
        { field: 'address', title: '具体地址', width: 180, align: 'center' },
        { field: 'weixinaccount', title: '微信号', width: 100, align: 'center' },
        { field: 'lastupdateuser', title: '最后操作人', width: 100, align: 'center' },
        { field: 'sbuycarbrand', title: '选购品牌', width: 100, align: 'center' },
        { field: 'sbuycarseries', title: '选购车系', width: 100, align: 'center' },
        { field: 'sbuycarmodel', title: '选购车型', width: 100, align: 'center' },
        { field: 'ordernumber', title: '订单号', width: 100, align: 'center', sortable: true },
        { field: 'knockdownprice', title: '成交价', width: 100, align: 'center', sortable: true },
        { field: 'placeordertime', title: '预订成交时间', width: 100, align: 'center', sortable: true},
        { field: 'picupcartime', title: '提车时间', width: 120, align: 'center', sortable: true},
        { field: 'givecause', title: '战败原因', width: 100, align: 'center' },
        { field: 'failurecauseanalyze', title: '战败原因分析', width: 100, align: 'center' },
        { field: 'createtime', title: '建档时间', width: 120, align: 'center', sortable: true },
        { field: 'posttime', title: '提交时间', width: 120, align: 'center', sortable: true },
        { field: 'marketdirector', title: '市场专员', width: 100, align: 'center', sortable: true, formatter: function (value, row, index) {
            return $.inArray(row.state, ['1']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
        }
        },
        { field: 'dccdirector', title: 'DCC专员', width: 100, align: 'center', sortable: true, formatter: function (value, row, index) {
            return $.inArray(row.state, ['2']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
        }
        },
        { field: 'exhibitiondirector', title: '展厅专员', width: 100, align: 'center', sortable: true, formatter: function (value, row, index) {
            return $.inArray(row.state, ['3']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
        }
        },
        { field: 'director', title: '直销专员', width: 100, align: 'center', sortable: true, formatter: function (value, row, index) {
            return $.inArray(row.state, ['4', '5', '10']) != -1 ? "<span style='color:red'>" + value + "</span>" : value;
        }
        },
        { field: 'showno', title: '自动编号', width: 100, align: 'center', sortable: true },
        { field: 'systemremark', title: '系统备注', width: 200, align: 'center', formatter: function (value, row, index) { return "<span style='color:red'>" + value + "</span>"; } }
    ]

    Core.Easyui.Init($('#datagrid'), Core.Easyui.Height, Core.Easyui.FieldColumns, {
        toolbar: '#toolbar', 
        fitColumns: false, 
        frozenColumns: [[
            { field: 'ck', checkbox: true },
            { field: 'action', title: '操作', width: 40, align: 'center', formatter: function (value, row, index) {
                return "<a class='data-archive-state coRf76120 operate_btn'' data-tid='" + row.id + "' data-state='" + row.state + "' data-stated='" + row.stated + "' data-index='" + index + "' data-posttime='" + row.posttime + "' onClick='Core.Easyui.Action(\"Toolbar\",this)'></a>";
            }},
            { field: 'state', title: '线索状态', width: 80, align: 'center', formatter: function (value, row, index) {
                return Core.Easyui.State[value] ? '<span ' + Core.Easyui.Params.Attention(value, row.posttime, row.lastcustomerlevelid) + '>' + Core.Easyui.State[value] + '</span>' : '';
            }},
            { field: 'name', title: '客户姓名', width: 80, align: 'center' },
            { field: 'phone', title: '客户电话', width: 90, align: 'center', sortable: true},
            { field: 'ibuycarseries', title: '拟购车系', width: 100, align: 'center', sortable: true }
        ]], 
        onSortColumn: function (sort, order) {
            try { Core.Easyui.Params.sort(sort, order); } catch (e) { }
        }, 
        onClickRow: function (index, row) {
            $(this).datagrid('clearSelections');
            $(this).datagrid('selectRow', index);
        }, view: detailview,
        detailFormatter: function (index, row) {
            return '<div class="threetab" data-key="' + index + '" style="padding:0;position:relative;height:305px;">\
            <div class="tabtool" style="float:left;">\
            <div class="tabs1" id="tb1_' + index + '" data-tid="' + row.id + '">追踪记录</div>\
            <div class="tabs2 hide" id="tb2_' + index + '" data-tid="' + row.id + '">最近通话记录</div>\
            <div class="tabs3" id="tb3_' + index + '" data-tid="' + row.id + '">流转记录</div>\
            </div>\
            <div id="ddv_tb1_' + index + '" class="tabslayer" style="position:absolute;top:0;left:25px;"><table id="tb1ddv-' + index + '"></table></div>\
            <div id="ddv_tb2_' + index + '" class="tabslayer" style="position:absolute;top:0;left:25px;"><table id="tb2ddv-' + index + '"></table></div>\
            <div id="ddv_tb3_' + index + '" class="tabslayer" style="position:absolute;top:0;left:25px;"><table id="tb3ddv-' + index + '"></table></div>\
            </div>';
        },
        onExpandRow: function (index, row) {
            var cols01 = [
                { field: 'connnectuser', title: '追踪人', width: 100, align: 'center' },
                { field: 'followtime', title: '追踪时间', width: 130, align: 'center'},
                { field: 'connectway', title: '追踪方式', width: 80, align: 'center' },
                { field: 'customerlevel', title: '追踪级别', width: 70, align: 'center' },
                { field: 'connectdetail', title: '追踪情况', width: 700, align: 'left' }
            ];
            var cols02 = [
                { field: 'dccname', title: '主叫/被叫用户', width: 130, align: 'center' },
                { field: 'phone', title: '客户号码', width: 120, align: 'center' },
                { field: 'starttime', title: '开始时间', width: 180, align: 'center', sortable: true, formatter: function (value, row, index) {
                    return Core.rule.isNumber('p.integer', value[0]) ? value[3] : "-";
                }},
                { field: 'times', title: '通话时长', width: 100, align: 'center', formatter: function (value, row, index) {
                    return value + "秒";
                }},
                { field: 'state', title: '通话结果', width: 80, align: 'center', formatter: function (value, row, index) {
                    try { if (parseInt(row.calltype) == 1) return '-'; } catch (e) { };
                    switch (parseInt(value)) { case 0: return "<span class='coRf76120'>未接通</span>"; break; case 1: return "接通"; break; }
                }},
                { field: 'record', title: '本地录音', width: 460, align: 'center', formatter: function (value, row, index) {
                    if (value != '' && row.uid == "117") {
                        return "<a onclick='Core.Easyui.Player(this);' date-link='" + value + "'><i class='player'></i></a>";
                    } else if (value != '') {
                        return "录音不在本机";
                    } else {
                        return "-";
                    }
                    //return value!='' ? "<a onclick='Core.Easyui.Player(this);' date-link='"+value+"'><i class='player'></i></a>" : "";
                }}
            ];
            var cols03 = [
                { field: 'createtime', title: '时间', width: 120, align: 'center'},
                { field: 'state', title: '线索状态', width: 200, align: 'center', formatter: function (value, row, index) {
                    var iState = { "1": "导入|集客", "2": "清洗|邀约", "3": "到店|洽谈", "4": "追踪|促成", "5": "预订|成交", "10": "提车|回访", "21": "转出待审", "31": "潜客(转出)", "32": "潜客(战败)", "33": "其他潜客" };
                    var w = '';
                    try { w += iState[row.stated]; } catch (e) { w += '' }
                    return (w != '' && w != 'undefined' ? w + ' -> ' : "") + iState[value];
                }},
                { field: 'owner', title: '线索拥有者', width: 100, align: 'center', formatter: function (value, row, index) {
                    return value != '' ? value : "-";
                }},
                { field: 'lastupdateuser', title: '操作员', width: 100, align: 'center', formatter: function (value, row, index) {
                    return value != '' ? value : "-";
                }},
                { field: 'systemmsg', title: '系统消息', width: 560, align: 'left' }
            ];
            function showlist(n_grid, cols, src) {
                n_grid.datagrid({
                    url: src,
                    method: 'get',
                    singleSelect: true,
                    loadMsg: '',
                    height: 305,
                    width: 1070,
                    pagination: true, //是否分页
                    multiSort: true, //多个排序
                    pageSize: 10, //初始化页内行数
                    pageList: [10, 20, 30],
                    fitColumns: true, //宽度自适横向滚动
                    columns: [cols],
                    onLoadSuccess: function () {
                        setTimeout(function () {
                            $('#datagrid').datagrid('fixDetailRowHeight', index);
                        }, 0);
                    }
                });
            }
            $('#tb1_' + index).live("click", function () {
                if ($(this).attr("data-loader") != '1') {
                    showlist($("#tb1ddv-" + index), cols01, "chancefollowlist.aspx?cid=" + $(this).attr("data-tid") + "&r" + Math.random()); 
                    $(this).attr("data-loader", '1');
                }
                var o = $(".threetab[data-key='" + index + "']");
                o.find(".tabtool > div").removeClass('ontab'); $(this).addClass('ontab');
                o.find(".tabslayer").css("visibility", "hidden");
                o.find('#ddv_tb1_' + index).css("visibility", "visible");
                $('#datagrid').datagrid('fixDetailRowHeight', index);
                return;
            });
            $('#tb2_' + index).live("click", function () {
                if ($(this).attr("data-loader") != '1') {
                    showlist($("#tb2ddv-" + index), cols02, "chancefollowcalllist.aspx?cid=" + $(this).attr("data-tid") + "&r" + Math.random()); $(this).attr("data-loader", '1');
                }
                var o = $(".threetab[data-key='" + index + "']");
                o.find(".tabtool > div").removeClass('ontab'); $(this).addClass('ontab');
                o.find(".tabslayer").css("visibility", "hidden");
                o.find('#ddv_tb2_' + index).css("visibility", "visible");
                $('#datagrid').datagrid('fixDetailRowHeight', index);
                return;
            });
            $('#tb3_' + index).live("click", function () {
                if ($(this).attr("data-loader") != '1') {
                    showlist($("#tb3ddv-" + index), cols03, "customermoverecordlist.aspx?cid=" + $(this).attr("data-tid") + "&r" + Math.random()); 
                    $(this).attr("data-loader", '1');
                }
                var o = $(".threetab[data-key='" + index + "']");
                o.find(".tabtool > div").removeClass('ontab'); $(this).addClass('ontab');
                o.find(".tabslayer").css("visibility", "hidden");
                o.find('#ddv_tb3_' + index).css("visibility", "visible");

                $('#datagrid').datagrid('fixDetailRowHeight', index);
                return;
            });
            $('#tb1_' + index).trigger("click");
        }
    });
</script>
<script type="text/javascript">
    Core.Easyui.Params = {
        box: {},
        sort: function (field, order) {
            Core.Easyui.Params.box['sort'] = field;
            Core.Easyui.Params.box['order'] = order;
        },
        Archive: function (posttime,days) {
            if(days == 0 ) return false;
            var P = Core.rule.isDatetime(posttime) ? (new Date(posttime)).getTime() / 1000 : 0;
            var fday = Math.ceil((Time - P) / 86400 + 1);
            return fday >= days;
        },
        Attention:function(state,posttime,level){
            if(!Core.Easyui.Settings || $.inArray(state,[1,2,4])==-1) return "";
            var level  = Core.rule.isNumber('p.integer',level) ? level : 0;
            var P = Core.rule.isDatetime(posttime) ? (new Date(posttime)).getTime()/1000 : 0;
            var fday = Math.ceil((Time-P)/86400+1);
            //强制转出 提早7天
            if($.inArray(state,[2])!=-1){
                try{
                    var ignore = false;
                    if(Core.rule.isNumber('p.integer',level) && Core.Easyui.Settings["forced"]['offcustomerlevel']){
                        if($.inArray(level,Core.Easyui.Settings["forced"]['offcustomerlevel'])!=-1) ignore = true;
                    }					
                    if(!ignore && Core.rule.isNumber('p.integer',Core.Easyui.Settings["forced"]['offday']) && (fday+7)>=parseInt(Core.Easyui.Settings["forced"]['offday'])) return "style='color:red'";			
                }catch(e){}
            }else if($.inArray(state,[4])!=-1){
                try{
                    var ignore = false;
                    if(Core.rule.isNumber('p.integer',level) && Core.Easyui.Settings["forced"]['outcustomerlevel']){
                        if($.inArray(level,Core.Easyui.Settings["forced"]['outcustomerlevel'])!=-1) ignore = true;
                    }
                    if(!ignore && Core.rule.isNumber('p.integer',Core.Easyui.Settings["forced"]['outday']) && (fday+7)>=parseInt(Core.Easyui.Settings["forced"]['outday'])) return "style='color:red'";			
                }catch(e){}
            }
            //主动转出
            if($.inArray(state,[1])!=-1){
                try{
                    return Core.Easyui.Settings["voluntary"]["trackmove"]!='1' ? '' : "style='color:green'";
                }catch(e){}
            }else if($.inArray(state,[2])!=-1){
                try{
                    if(Core.rule.isNumber('p.integer',Core.Easyui.Settings["voluntary"]["offday"]) && fday>=parseInt(Core.Easyui.Settings["voluntary"]["offday"])) return "style='color:green'";
                }catch(e){}
            }else if($.inArray(state,[4])!=-1){
                try{
                    if(Core.rule.isNumber('p.integer',Core.Easyui.Settings["voluntary"]["outday"]) && fday>=parseInt(Core.Easyui.Settings["voluntary"]["outday"])) return "style='color:green'";
                }catch(e){}
            }
        }
    }
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

    var setTfun = null;
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
                break; 
            case 'removefilter':
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
            case 'add':
                dialog('755', <%= GetInt("state") == 3 ? "'480'" : "'435'" %>, $('div.tchose:first').text() + ' - 线索添加', "chanceedit.aspx?state=" + <%=GetInt("state") %> + "&corpid=" + corpid + "&r=" + Math.random()); return;
                break; 
            case 'import':
                dialog('320', '180', $('div.tchose:first').text() + ' - 数据导入', "import.aspx?state=" + <%=GetInt("state") %> + "&corpid=" + corpid +"&r=" + Math.random()); return;
                break; 
            case 'follow':
                dialog('570', '300', '线索追踪', "chancefollowadd.aspx?id=" + $(o).attr('data-tid') + "&r=" + Math.random()); return;
                break; 
            case 'Toolbar':
                var str = '<div class="caozuoDet" style="top:' + $(o).parent(".datagrid-cell").position().top + 'px" id="Toolbar-' + $(o).attr('data-tid') + '" data-state="' + $(o).attr('data-state') + '" data-tid="' + $(o).attr('data-tid') + '" onmouseover="Core.Easyui.Action(\'Toolbar.Over\');" onmouseout="Core.Easyui.Action(\'Toolbar.Out\',this)"><span class="coRf76120">';
                var Tool = { "width": 0, "left": 115, "bar": [] };

                switch ($(o).attr('data-state')) {
                    case "1": Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="2">提交至清洗|邀约</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="<%=Movecheck == 1 ? 21 : 31 %>">转出(提交至潜客数据库)</a>']; Tool.width = 277;
                        <%if(!(Trackmove == 1)){ %> Tool.bar.baoremove(1); Tool.width -= 154; <%} %>
                        break;
                    case "2":
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="3">提交至到店|洽谈</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="4">提交至追踪|促成</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="<%=Offcheck==1?21:31 %>">转出(提交至潜客数据库)</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="32">战败(商谈终止)</a>']; Tool.width = 496;
                        if (!Core.Easyui.Params.Archive($(o).attr('data-posttime'),<%=Voluntaryoffday %>)) { Tool.bar.baoremove(2); Tool.width -= 154; }
                        <%if(IsProcess){ %>Tool.bar.baoremove(1); Tool.width -= 113;<%} %>
                        break;
                    case "3": 
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="2">未到店(提交至清洗|邀约)</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="4">已到店(提交至追踪|促成)</a>']; Tool.width = 331; 
                        break;
                    case "4":
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="5">提交到预订|成交</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="<%=Outcheck==1?21:2 %>">转出(提交至清洗|邀约)</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="32">战败(提交至潜客数据库)</a>'];
                        Tool.width = 425;
                        if (!Core.Easyui.Params.Archive($(o).attr('data-posttime'),<%=Voluntaryoutday %>)) { Tool.bar.baoremove(1); Tool.width -= 148; }
                        break;
                    case "5": 
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="10">客户提车结案</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="32">退单(提交至潜客数据库)</a>']; Tool.width = 259; 
                        break;
                    case "10": 
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="5">退回预订|成交</a>']; Tool.width = 111; 
                        break;
                    case "21": 
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="' + $(o).attr('data-stated') + '">驳回转出请求</a>', '<a onclick="Core.Easyui.Action(\'state\',this);" data-key="' + ($(o).attr('data-stated') != '4' ? '31' : '2') + '">批准转出请求</a>']; Tool.width = 200; 
                        break;
                    case "31": case "32": case "33": 
                        Tool.bar = ['<a onclick="Core.Easyui.Action(\'state\',this);" data-key="2">提交至清洗|邀约</a>']; Tool.width = 123; 
                        break;
                }
                str += Tool.bar.join('<i>|</i>') + '</span><i class="closeCell" onclick="Core.Easyui.Action(\'Toolbar.Remove\');"></i></div>';
                $(o).parent(".datagrid-cell").after(str);

                $("#Toolbar-" + $(o).attr('data-tid')).stop().animate({ width: Tool.width + 'px', left: Tool.left + 'px' });

                Core.Easyui.Action('Toolbar.Remove', $("#Toolbar-" + $(o).attr('data-tid')));
                Core.Easyui.Action('Toolbar.Out', $("#Toolbar-" + $(o).attr('data-tid')));

                $(".datagrid-body").scroll(function () { $(".caozuoDet").fadeOut(function () { $(".caozuoDet").remove(); }); })
                break; 
            case 'Toolbar.Remove':
                $('.caozuoDet').not(o).fadeOut("slow", function () { $('.caozuoDet').not(o).remove(); });
                break; 
            case 'Toolbar.Over':
                clearTimeout(setTfun);
                break; 
            case 'Toolbar.Out':
                setTfun = setTimeout(function () { $(o).fadeOut("slow", function () { $(o).remove(); }); }, 2000);
                break; 
            case 'state':
                var state = $(o).parents(".caozuoDet").attr('data-state') + ":" + $(o).attr('data-key');
                var H = 440;
                switch (state) {
                    case '2:32': case '4:32': case '5:32':
                        H = 480; break;
                    case '2:3': 
                        H = 480; break;
                    case '3:4': case '4:5':
                        H = 510; break;
                    case '5:10': case '10:5':
                        H = 540; break;
                }
                dialog('755', H, $(o).text(), "chanceedit.aspx?id=" + $(o).parents(".caozuoDet").attr('data-tid') + "&state=" + $(o).attr('data-key') + "&r=" + Math.random()); return;
                break; 
            case 'handover':
                var rows = $('#datagrid').datagrid('getSelections');
                var ids = [], iorderid = [];
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].state == "10") continue;
                    ids.push(rows[i].id);
                    iorderid.push(rows[i].showno);
                }
                dialog('450', 220, $(o).text(), "multmove.aspx?ids=" + ids.join(',') + "&iorderid=" + iorderid.join(',') + "&corpid=" + corpid + "&r=" + Math.random()); return;
                break; 
            case 'excel': //导出
                var rows = $('#datagrid').datagrid('getSelections');
                var ids = []
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].id);
                }
                if (!ids.length) return;

                dialog('420', 200, "导出Excel", "export.aspx?corpid=" + corpid + "&ids=" + ids.join(',') + "&r=" + Math.random()); return;
                break; 
            case 'edit':
                try { var rows = Core.Easyui.o.datagrid('getSelections'); } catch (e) { return; }
                if (!rows.length) return;
                if (rows.length > 1) { $.messager.alert('提示', '该操作仅允许1条数据', 'info'); return; }
                var H = 435; switch (parseInt(rows[0].state)) { case 3: H = 510; break; case 5: H = 510; break; case 10: H = 540; break; }
                dialog('755', H, $('div.tchose:first').text() + ' - 编辑', "chanceedit.aspx?id=" + rows[0].id + "&r=" + Math.random()); return;
                break; 
            case 'remove':
                var rows = $('#datagrid').datagrid('getSelections');
                var ids = []
                for (var i = 0; i < rows.length; i++) {
                    ids.push(rows[i].id);
                }
                if (!ids.length) return;
                $.messager.confirm('确认提示', '您确定要执行该操作吗?执行后将无法恢复!', function (r) {
                    if (!r) return;
                    $.get("chancemg.aspx?action=del&corpid=" + corpid + "&ids=" + ids.join(',') + "&r=" + Math.random(), function (data) {
                        if (data != 'ok') return;
                        Core.Easyui.reload(); //window.location.reload();
                    })
                });
                break; 
            case 'diyColumns':
                Core.Easyui.diyColumns.show($("#showdlist"));
                break; 
            case 'diyColumns.recover':
                Core.Easyui.diyColumns.recover();
                window.location.reload();
                break;
        }
    }
    var corpid =0;
    <%if (Admin.Administrator)
      {%>
    corpid = $("#ddlCorporationSearch option:selected").val();
    <%}else{ %>
    corpid = <%=Admin.CorporationID %>;
    <%} %>
    $(document).ready(function () {
        $(".nav_tabs .eachTab").live('click', function () {
            if ($(this).hasClass("unchose") || $(this).hasClass("tchose") || $(this).hasClass("nochose")) return;
            window.location.href = $(this).attr('data-link') + "&corpid=" + corpid;
        });
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
                } else if ($.inArray(option.value, ["arrive", "archivemove","followalarm","customerlevel", "infotype", "infosource", "connectway", "giveupcause", "paymentway", "ibuytime", "tracktag"]) != -1) {
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
                } else if ($.inArray(option.value, ["state"]) != -1) {
                    $("span.fieldtext").hide();
                    $("span.fieldselect").show();
                    $("#keywords_select").combobox({
                        method: 'get',
                        valueField: 'id',
                        textField: 'name',
                        url: "/ajax/getsearchlist.aspx?action=state&datalist=list&corpid=" + corpid,
                        onLoadSuccess: function () {
                            $("#keywords_select").combobox('setValue', '').combobox('setText', "请选择...");
                        }
                    });
                } else if ($.inArray(option.value, ["owneruid", "mpuid", "dccuid", "exuid", "dsuid"]) != -1) {
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

        Core.Easyui.get('chancelist.aspx?corpid=' + corpid<%if(GetInt("state",0)>0){ %> + '&state=<%=GetInt("state",0) %>'<%} %><%if(GetInt("archivemove",0)>0){ %> + '&archivemove=<%=GetInt("archivemove",0) %>'<%} %><%if(!string.IsNullOrEmpty(GetString("followalarm"))){ %> + '&followalarm=<%=GetInt("followalarm",0) %>'<%} %>);
        $(".datagrid td[field='state']").find(".datagrid-cell").mouseover(function () {
            $(".datagrid td[field='state']").find(".bubble_tips").remove();
            $(".datagrid td[field='state']").append("<div class='bubble_tips' style='left:188px;'><i></i><span style='color:red'>红色：该线索将在7日内被系统强制转出</span><br /><span style='color:green'>绿色：该线索可以主动转出</span></div>");
        }).mouseleave(function () { $(".datagrid td[field='state']").find(".bubble_tips").remove(); })
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
