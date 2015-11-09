<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ComOpp.BackAdmin.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>红旭集团商机管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="plugins/jquery-easyui-1.3.6/themes/default/easyui.css">
    <link rel="stylesheet" type="text/css" href="plugins/jquery-easyui-1.3.6/themes/icon.css">
    <link rel="stylesheet" type="text/css" href="css/layout.css">
    <link rel="stylesheet" type="text/css" href="css/common.css">
    <script type="text/javascript" src="js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="plugins/jquery-easyui-1.3.6/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="plugins/jquery-easyui-1.3.6/datagrid-detailview.js"></script>
    <script type="text/javascript" src="plugins/jquery-easyui-1.3.6/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="js/jquery.idTabs.min.js"></script>
    <script type="text/javascript" src="js/layout.js"></script>
    <script type="text/javascript" src="js/public.core.js"></script>
    <script type="text/javascript" src="js/common.js"></script>
    <script type="text/javascript" src="js/cpcparam.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/cpccommon.js" charset="utf-8"></script>
    <script type="text/javascript">
        if (self.location != top.location) {
            top.location.href = self.location;
        }
    </script>
</head>
<body class="easyui-layout">
    <div>
        <object id="qnviccub" type="application/x-cc301plugin" width="0" height="0">
            <param name="onload" value="pluginLoaded" />
        </object>
    </div>
    <div data-options="region:'north',border:false" style="height: 36px; overflow: hidden;
        background: #2BB7AA">
        <div class="header">
            <a class="sysname fl">
                <%if (!Admin.Administrator)
                  { %><%= Admin.Corporation %>｜<%} %>商机管理系统(销售)</a>
            <ul class="litNav fr">
                <li style="border-left: 0;"><a href="center/profile.aspx">个人中心</a></li>
                <li>
                    <%=Admin.Realname %></li>
                <li style="display: none"><a href="">消息<i>（1）</i></a></li>
                <li><a href="logout.aspx">退出</a></li>
            </ul>
            <div class="fr">
                <div class="hsSearch fl">
                    <input id="search_input" class="hsInput" type="text" placeholder="话术搜索" />
                    <input class="hsSubmit" type="submit" value="" onclick="HsSearch()">
                </div>
                <div class="slideBtn fl">
                </div>
            </div>
        </div>
    </div>
    <div data-options="region:'west',split:true,border:false" style="width: 200px; padding: 0;
        background: #e8e8e8;">
        <a class="layout_btn west_btn" href="javascript:void(null);" onclick="$('body').layout('collapse','west');westTop()">
        </a>
        <div class="layout_boxs">
            <div class="mainbav">
                <dl id="ltree">
                    <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "公告通知管理,公告通知列表"))
                      { %>
                    <dt data-key="support/noticemg.aspx"><i class="l icon1"></i>公告通知</dt>
                    <%} %>
                    <dt><i class="r"></i><i class="l icon3"></i>商机管理</dt>
                    <dd>
                        <p>
                            <a href="chance/chancemg.aspx" target="riframe">商机管理</a></p>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "潜客数据库"))
                          { %>
                        <p>
                            <a href="chance/potentialmg.aspx" target="riframe">潜客数据库</a></p>
                        <%} %>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "转出审核"))
                          { %>
                        <p>
                            <a href="chance/checkmg.aspx" target="riframe">转出审核</a></p>
                        <%} %>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "综合查询"))
                          { %>
                        <p>
                            <a href="javascript:void(0);" onclick="showData('chance/archivemerge.aspx');">综合查询</a></p>
                        <%} %>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "追踪流水"))
                          { %>
                        <p>
                            <a href="chance/followmg.aspx" target="riframe">追踪流水</a></p>
                        <%} %>
                    </dd>
                    <!--<dt data-key="analyse/index/callcenter/connect"><i class="l icon5">
                    </i>分析报表</dt>-->
                    <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "话术管理"))
                      { %>
                    <dt data-key="support/talkmg.aspx"><i class="l icon6"></i>话术管理</dt>
                    <%} %>
                    <!--<dt data-key="http://sales.new4s.com/signup/signup/index"><i class="l icon1"></i>报名窗口</dt>-->
                    <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "基础数据"))
                      { %>
                    <dt><i class="r"></i><i class="l icon7"></i>系统管理</dt>
                    <dd>
                        <%if (Admin.Administrator)
                          {%>
                        <p>
                            <a href="system/corporationmg.aspx" target="riframe">系统设置</a></p>
                        <%} %>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "基础数据"))
                          { %>
                        <p>
                            <a href="common/basesetting.aspx" target="riframe">基础数据</a></p>
                        <%} %>
                        <%if (Admin.Administrator)
                          {%>
                        <p>
                            <a href="system/logmg.aspx" target="riframe">日志管理</a></p>
                        <%} %>
                    </dd>
                    <%} %>
                    <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "账户组") || CheckGroupPower(Admin.GroupPower, "账户管理"))
                      { %>
                    <dt><i class="r"></i><i class="l icon8"></i>用户管理</dt>
                    <dd>
                        <%if (Admin.Administrator)
                          {%>
                        <p>
                            <a href="admin/adminmg.aspx" target="riframe">管理员管理</a></p>
                        <%} %>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "账户组"))
                          { %>
                        <p>
                            <a href="user/powergroupmg.aspx" target="riframe">账户组</a></p>
                        <%} %>
                        <%if (Admin.Administrator || Admin.UserRole == ComOpp.Components.UserRoleType.系统管理员 || CheckGroupPower(Admin.GroupPower, "账户管理"))
                          { %>
                        <p>
                            <a href="user/usermg.aspx" target="riframe">账户管理</a></p>
                        <%} %>
                    </dd>
                    <%} %>
                </dl>
            </div>
        </div>
    </div>
    <div id="layout_center" data-options="region:'center',border:false">
        <div id="huashuBox" class="layout_box center_iframe huashuIframe">
            <div id="oneBox" class="huashu_box fl" style="width: 26%; overflow: hidden;">
            </div>
            <div id="twoBox" class="huashu_box fl" style="width: 30%; overflow: hidden;">
            </div>
            <div id="threeBox" class="huashu_box fl" style="width: 44%; overflow: hidden;">
            </div>
        </div>
        <iframe id="riframe" name="riframe" height="500" class="layout_box center_iframe"
            frameborder="0" width="100%" src="desktop.aspx"></iframe>
    </div>
    <script type="text/javascript">
        var hsBool = false;
        var isOpen = false;
        $(function () {
            //下拉按钮鼠标事件
            $('.slideBtn').live("click", function () {
                if (!isOpen) {
                    huashuOpen();
                    return;
                } else {
                    huashuClose();
                    return;
                }
            });
            $('#huashuBox').css('height', $('#huashuBox').parent().height());

            $("#search_input").focus(function () {
                $(document).keydown(function (event) {
                    if (event.keyCode == 13) {
                        HsSearch();
                    }
                })
            });
            $("#search_input").blur(function () {
                $(document).unbind("keydown");
            });
        })
        function HsSearch() {
            var search_txt = $("#search_input").val();
            if (search_txt == '') {
                huashuClose();
                return;
            }
            huashuOpen();
            isOpen = true;
            $("#huashuBox").find("#hsPage2").attr("src", "common/talklist.aspx?keywords=" + search_txt);
        }
        function huashuOpen() {
            if (!hsBool) {
                var huashuHtml1 = '<iframe id="hsPage1" src="common/talktag.aspx" name="hsPage1" frameborder="0" width="100%" height="100%" scrolling="yes"></iframe>';
                var huashuHtml2 = '<iframe id="hsPage2" src="common/talklist.aspx" name="hsPage2" frameborder="0" width="100%" height="100%" scrolling="yes"></iframe>';
                var huashuHtml3 = '<iframe id="hsPage3" src="common/talkpage.aspx" name="hsPage3" frameborder="0" width="100%" height="100%" scrolling="yes"></iframe>';
                $('#oneBox').html(huashuHtml1);
                $('#twoBox').html(huashuHtml2);
                $('#threeBox').html(huashuHtml3);
                hsBool = true;
            }
            $('.slideBtn').addClass('slidedown');
            $('#huashuBox').slideDown('slow');
            isOpen = true;
        }
        function huashuClose() {
            $('.slideBtn').removeClass('slidedown');
            $('#huashuBox').slideUp('slow');
            isOpen = false;
            $("#search_input").val('');
        }
    </script>
</body>
</html>
