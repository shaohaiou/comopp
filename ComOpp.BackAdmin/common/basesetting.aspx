<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="basesetting.aspx.cs" Inherits="ComOpp.BackAdmin.common.basesetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>基础设置-红旭集团商机管理系统V1.0</title>
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
        <a href="basesetting.aspx" class="selected">基础设置</a> <a href="customerlevelmg.aspx">客户等级</a>
        <a href="infotypemg.aspx">信息类型</a> <a href="infosourcemg.aspx">信息来源</a> <a href="connectwaymg.aspx">
            追踪方式</a> <a href="giveupcausemg.aspx">放弃原因</a> <a href="paymentwaymg.aspx">支付方式</a>
        <a href="ibuytimemg.aspx">拟购时间</a> <a href="tracktagmg.aspx">线索标签</a> <a href="archivesetting.aspx">
            转出设置</a>
    </div>
    <div>
        <div>
            <form class="myform" runat="server">
            <asp:ScriptManager runat="server" ID="sm1">
            </asp:ScriptManager>
                    <ul class="tabs_box_one">
            <asp:UpdatePanel runat="server" ID="up1">
                <ContentTemplate>
                        <%if (Admin.Administrator)
                          {%>
                        <li>
                            <label class="llabel">
                                公司：</label>
                            <asp:DropDownList runat="server" ID="ddlCorporation" AutoPostBack="true" OnSelectedIndexChanged="ddlCorporation_SelectedIndexChanged">
                            </asp:DropDownList>
                        </li>
                        <%} %>
                        <li>
                            <label class="llabel">
                                默认品牌：</label>
                            <asp:DropDownList runat="server" ID="ddlCarBrand">
                            </asp:DropDownList></li>
                        <li>
                            <label class="llabel">
                                所属区域：</label>
                            <asp:DropDownList runat="server" ID="ddlProvince" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="ddlCity" AutoPostBack="true" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList runat="server" ID="ddlDistrict">
                            </asp:DropDownList></li>
                        <li>
                            <label class="llabel" style="width: 180px; max-width: 180px; margin-left: 20px">
                                DCC电话营销员负责邀约到店：</label>
                            <asp:RadioButtonList runat="server" ID="rblIsProcess" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="是【推荐】" Value="1"></asp:ListItem>
                                <asp:ListItem Text="否" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </li>
                </ContentTemplate>
            </asp:UpdatePanel>
                        <li>
                            <div class="blank_h20">
                            </div>
                        </li>
                        <li>
                            <label class="llabel">
                                &nbsp;</label>
                            <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn" Text="确定保存" />
                        </li>
                    </ul>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#rblIsProcess label").eq(1).html("<span title=\"如果选择否，用户可以将“清洗|邀约”模块的线索绕过“到店|洽谈”模块直接提交至“追踪|促成”模块，由直销专员负责邀约到店，这将导致展厅督查专员无法监控该线索到店情况\">" + $("#rblIsProcess label").eq(1).html() + "</span>");

            Core.Easyui.resize();
        })
    </script>
</body>
</html>
