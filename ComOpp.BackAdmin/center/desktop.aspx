<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="desktop.aspx.cs" Inherits="ComOpp.BackAdmin.center.desktop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>个人中心 - 红旭集团商机管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="http://my.new4s.com/resources/css/common.css" />
    <script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="../js/Validform_v5.3.js"></script>
    <style>
        body
        {
            background: #efefef;
            padding-top: 20px;
        }
    </style>
</head>
<body>
    <div class="myc_tit">
        <div class="mytopTit" style="">
            <div class="topTit_left">
                <img src="../images/logo.jpg" /><span><a href="/center/desktop.aspx" style="color: #2b98c4">个人中心</a></span></div>
            <div class="topTit_right" style="margin: 0;">
                <a href="/center/desktop.aspx">首页</a>|<a href="/logout.aspx">[<%=Admin.Realname %>]退出</a></div>
        </div>
    </div>
    <div class="form_box" style="padding-bottom: 5px;">
        <div class="myc_box bge7" style="height: 129px; background: #f5f5f5;">
            <div class="myc_outBox">
                <div class="fl" style="padding-left: 40px">
                    <table class="myc_table">
                        <tr>
                            <td>
                                <i class="myc_name_ic"></i>姓名：<%=Admin.Realname %><em></em>
                            </td>
                            <td>
                                <i class="myc_male_ic"></i>性别：<%=Admin.Sex == 1 ? "男" : (Admin.Sex == 2 ? "女" : "保密") %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i class="myc_mobile_ic"></i>帐号：<%=AdminName %><em></em>
                            </td>
                            <td>
                                <i class="myc_qq_ic"></i>QQ：<%=Admin.QQ %>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="fr" style="margin: 10px 20px 0 0;">
                    <div class="chongzhi_btn fl" style="display: none">
                        <a href="#"></a>
                    </div>
                    <div class="fl">
                        <a id="setBtn" class="set_alink" href="profile.aspx"><i
                            class="myc_set_ic"></i>设置</a>
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <div class="myc_box clear" style="margin: 20px auto; padding-bottom: 20px;">
            <div class="left_bar fl">
                <i class="nowPosit"></i>
                <div class="each_menu_bar color01 w7px">
                    <a>销售商机系统<span>V1.0</span></a><i class="riangle"></i></div>
                <div class="each_menu_bar color02">
                    <a>内部管理系统<span>V1.0</span></a><i
                        class="riangle"></i></div>
                <%-- <div class="each_menu_bar color02"><a href="http://fuwu.autobms.net" target="_blank">售后商机系统<span>V1.0</span></a><i class="riangle"></i></div>
                <div class="each_menu_bar color02"><a href="http://my.new4s.com/center/desktop/index/apps/4">二手车系统<span>V1.0</span></a><i class="riangle"></i></div>--%>
                <!--color01 color02 color03-->
            </div>
            <script>
                $(function () {
                    try {
                        $(".nowPosit").css("top", $(".left_bar").find(".w7px").position().top + 26);
                    } catch (e) { }
                })
            </script>
            <div class="card_box fr" style="width: 600px;">
                <ul class="cardUl" data-channel="301" data-url="/index.aspx" data-click="0">
                    <li class="height5px"></li>
                    <li class="card_name"><span class="fl" title="<%if (!Admin.Administrator){ %><%= Admin.Corporation %><%} %>"><%if (!Admin.Administrator){ %><%= Admin.Corporation %><%} %></span> </li>
                    <li><span>角色：<%=Admin.PowerGroupName%></span></li>
                    <li><span>联系人：<%=Admin.Realname %></span></li>
                </ul>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $(".cardUl").live('click', function () {
                if ($(this).attr('data-click') == '1') return;
                var o = $(this);
                window.location.href = $(this).attr('data-url');
            });
        })
    </script>
    <div class="footer">
        <div class="footer_con">
            <div class="grayLine">
            </div>
        </div>
        <p class="copystyle">
            红旭集团商机管理系统 V1.0 Powered by <a href="http://www.hongxu.cn">www.hongxu.cn</a>
        2015-2020
    </div>
</body>
</html>
