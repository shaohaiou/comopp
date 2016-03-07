<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="archivesetting.aspx.cs"
    Inherits="ComOpp.BackAdmin.common.archivesetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>车云|汽车销售客户管理系统V2.0</title>
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
        <a href="basesetting.aspx">基础设置</a> <a href="customerlevelmg.aspx?corpid=<%=GetInt("corpid") %>">客户等级</a> <a href="infotypemg.aspx?corpid=<%=GetInt("corpid") %>">
            信息类型</a> <a href="infosourcemg.aspx?corpid=<%=GetInt("corpid") %>">信息来源</a> <a href="connectwaymg.aspx?corpid=<%=GetInt("corpid") %>">追踪方式</a>
        <a href="giveupcausemg.aspx?corpid=<%=GetInt("corpid") %>">放弃原因</a> <a href="paymentwaymg.aspx?corpid=<%=GetInt("corpid") %>">支付方式</a> <a href="ibuytimemg.aspx?corpid=<%=GetInt("corpid") %>">
            拟购时间</a> <a href="tracktagmg.aspx?corpid=<%=GetInt("corpid") %>">线索标签</a> <a href="archivesetting.aspx?corpid=<%=GetInt("corpid") %>" class="selected">
                转出设置</a>
    </div>
    <div>
        <div>
            <form class="myform" runat="server">
            <ul class="tabs_box_one">
                <%if (Admin.Administrator)
                  { %>
                <li style="margin-left: 50px; margin-top: 20px; font-weight: bold">公司：<asp:DropDownList runat="server" ID="ddlCorporation" AutoPostBack="true" OnSelectedIndexChanged="ddlCorporation_SelectedIndexChanged"></asp:DropDownList></li>
                <%} %>
                <li style="margin-left: 50px; margin-top: 20px; font-weight: bold">导入|集客 -> 潜客数据库</li>
                <li style="margin-left: 105px">主动转出：<label style="margin-right: 3px"><input runat="server"
                    type="checkbox" id="cbxTrackmove" />
                    开启</label></li>
                <li style="margin-left: 105px">转出审核：<label style="margin-right: 3px"><input runat="server"
                    type="checkbox" id="cbxMovecheck" />
                    开启</label></li>
                <li style="margin-left: 50px; margin-top: 20px; font-weight: bold">清洗|邀约 -> 潜客数据库</li>
                <li style="margin-left: 105px">强制转出条件：线索提交至模块满
                    <input type="text" runat="server" id="txtForcedoffday" value="0" class="textbox Validform_error"
                        style="width: 30px; height: 18px; line-height: 18px;" datatype="n" nullmsg="强制转出天数"
                        errormsg="请设置强制转出天数" />
                    天(0表示不启用) <span class="Validform_checktip"></span></li>
                <li style="margin-left: 105px" class="rbl">强制转出忽略客户级别：
                    <asp:CheckBoxList runat="server" ID="cblOffcustomerlevel" RepeatLayout="Flow" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                    <span class="Validform_checktip"></span></li>
                <li style="margin-left: 105px">主动转出条件：线索提交至模块第
                    <input type="text" id="txtVoluntaryoffday" value="0" class="textbox Validform_error"
                        style="width: 30px; height: 18px; line-height: 18px;" runat="server" datatype="n"
                        nullmsg="主动转出天数" errormsg="请设置主动转出天数" />
                    天(0表示不启用) <span class="Validform_checktip"></span></li>
                <li style="margin-left: 105px">主动转出审核：<label style="margin-right: 3px"><input runat="server"
                    id="cbxOffcheck" type="checkbox" />
                    开启</label></li>
                <li style="margin-left: 50px; margin-top: 20px; font-weight: bold">追踪|促成 -> 清洗|邀约</li>
                <li style="margin-left: 105px">强制转出条件：线索提交至模块满
                    <input type="text" runat="server" id="txtForcedoutday" value="0" class="textbox Validform_error"
                        style="width: 30px; height: 18px; line-height: 18px;" datatype="n" nullmsg="强制转出天数"
                        errormsg="请设置强制转出天数">
                    天(0表示不启用)<span class="Validform_checktip"></span></li>
                <li style="margin-left: 105px" class="rbl">强制转出忽略客户级别：
                    <asp:CheckBoxList runat="server" ID="cblForcedoutdaylevel" RepeatLayout="Flow" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </li>
                <li style="margin-left: 105px">主动转出条件：线索提交至模块第
                    <input type="text" runat="server" id="txtVoluntaryoutday" value="0" class="textbox Validform_error"
                        style="width: 30px; height: 18px; line-height: 18px;" datatype="n" nullmsg="主动转出天数"
                        errormsg="请设置主动转出天数">
                    天(0表示不启用)<span class="Validform_checktip"></span></li>
                <li style="margin-left: 105px">主动转出审核：<label style="margin-right: 3px"><input runat="server"
                    id="cbxOutcheck" type="checkbox" />
                    开启</label></li>
                <li>
                    <div class="blank_h20">
                    </div>
                </li>
                <li>
                    <label class="llabel">
                        &nbsp;</label>
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn" Text="确定保存" OnClick="btnSubmit_Click" />
                    <input type="reset" class="btn" value="清除重置">
                </li>
            </ul>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".myform").Validform({
                tiptype: 3
            });
            Core.Easyui.resize();
        })
    </script>
</body>
</html>
