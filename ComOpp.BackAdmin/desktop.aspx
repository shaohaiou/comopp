<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="desktop.aspx.cs" Inherits="ComOpp.BackAdmin.desktop" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>红旭集团商机管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="plugins/jquery-easyui-1.3.6/themes/default/easyui.css">
    <link rel="stylesheet" type="text/css" href="plugins/jquery-easyui-1.3.6/themes/icon.css">
    <link rel="stylesheet" type="text/css" href="css/common.css">
    <script type="text/javascript" src="js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="plugins/jquery-easyui-1.3.6/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="plugins/jquery-easyui-1.3.6/datagrid-detailview.js"></script>
    <script type="text/javascript" src="plugins/jquery-easyui-1.3.6/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
    <style type="text/css">
        html
        {
            overflow-x: hidden;
            overflow-y: auto;
        }
        .datagrid .panel-body
        {
            border: 0;
        }
        /*去掉大表格的边框*/
    </style>
</head>
<body style="padding: 10px">
    <%if (Admin.Administrator)
      {%>
    <form runat="server">
    <div style="padding: 10px 0px;">
    <asp:DropDownList runat="server" ID="ddlCorporation" AutoPostBack="true" OnSelectedIndexChanged="ddlCorporation_SelectedIndexChanged"></asp:DropDownList>
    </div>
    </form>
    <%} %>
    <div class="m_contain">
        <div class="top_tit">
            商机管理</div>
        <div style="padding: 20px 35px; height: 100px">
            <div>
                <div style="width: 300px; overflow: hidden; float: left">
                    <div class="mynews clear">
                        <p class="ic2 fl">
                            <i></i>共有<span class="red"><%=VoluntaryNum%></span>条线索可主动转出</p>
                        <a class="fr" href="chance/chancemg.aspx?corpid=<%=corpid %>&archivemove=1">点击查看</a></div>
                </div>
                <div style="width: 300px; overflow: hidden; float: left; border-left: 1px solid #ccc;
                    margin-left: 30px; padding-left: 30px">
                    <div class="mynews clear">
                        <p class="ic2 fl">
                            <i></i>共有<span class="red"><%=ForcedNum%></span>条线索7日内将强制转出</p>
                        <a class="fr" href="chance/chancemg.aspx?corpid=<%=corpid %>&archivemove=2">点击查看</a></div>
                </div>
            </div>
            <div style="clear: both">
                <div style="width: 300px; overflow: hidden; float: left">
                    <div class="mynews clear">
                        <p class="ic2 fl">
                            <i></i>共有<span class="red"><%=ConnecttimeoutNum %></span>条线索追踪超时</p>
                        <a class="fr" href="chance/chancemg.aspx?corpid=<%=corpid %>&followalarm=2">点击查看</a></div>
                </div>
                <div style="width: 300px; overflow: hidden; float: left; border-left: 1px solid #ccc;
                    margin-left: 30px; padding-left: 30px">
                    <div class="mynews clear">
                        <p class="ic2 fl">
                            <i></i>共有<span class="red"><%=ConnecttimeoutingNum%></span>条线索24小时内将追踪超时</p>
                        <a class="fr" href="chance/chancemg.aspx?corpid=<%=corpid %>&followalarm=1">点击查看</a></div>
                </div>
            </div>
        </div>
    </div>
    <div class="m_contain" style="display: none">
        <div class="top_tit">
            营销分析</div>
        <div>
            <div class="each_tit">
                经销商KPI<div class="cTime">
                    <a class="now" style="border-left: 1px #bbb solid;">年</a><a>月</a><a>日</a></div>
            </div>
            <ul class="percent">
                <li>
                    <div class="jqm-round-wrap">
                        <div class="jqm-round-bg">
                        </div>
                        <canvas id="jqm-round-sector0" class="jqm-round-sector"></canvas>
                        <div class="jqm-round-circle">
                            <p>
                                50.00%</p>
                        </div>
                    </div>
                    <div class="per_data">
                        <p class="p1">
                            50.00</p>
                        <p class="p2">
                            线索有效率</p>
                        <div class="pline">
                        </div>
                        <p class="p3">
                            900000</p>
                        <p class="p4">
                            日均</p>
                    </div>
                    <div class="litCircle">
                        ?</div>
                </li>
                <li>
                    <div class="jqm-round-wrap">
                        <div class="jqm-round-bg">
                        </div>
                        <canvas id="jqm-round-sector1" class="jqm-round-sector"></canvas>
                        <div class="jqm-round-circle">
                            <p>
                                75.00%</p>
                        </div>
                    </div>
                    <div class="per_data">
                        <p class="p1">
                            75.00</p>
                        <p class="p2">
                            到店率</p>
                        <div class="pline">
                        </div>
                        <p class="p3">
                            900000</p>
                        <p class="p4">
                            日均</p>
                    </div>
                    <div class="litCircle">
                        ?</div>
                </li>
                <li>
                    <div class="jqm-round-wrap">
                        <div class="jqm-round-bg">
                        </div>
                        <canvas id="jqm-round-sector2" class="jqm-round-sector"></canvas>
                        <div class="jqm-round-circle">
                            <p>
                                89.90%</p>
                        </div>
                    </div>
                    <div class="per_data">
                        <p class="p1">
                            89.90</p>
                        <p class="p2">
                            到店成交率</p>
                        <div class="pline">
                        </div>
                        <p class="p3">
                            900000</p>
                        <p class="p4">
                            日均</p>
                    </div>
                    <div class="litCircle">
                        ?</div>
                </li>
            </ul>
        </div>
        <div style="display: none">
            <div class="top_tit">
                线索状态查询</div>
            <table id="rate_list">
            </table>
        </div>
    </div>
</body>
</html>
