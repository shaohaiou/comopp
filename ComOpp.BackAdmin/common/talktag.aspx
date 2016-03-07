<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="talktag.aspx.cs" Inherits="ComOpp.BackAdmin.common.talktag" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>红旭集团销售客户管理系统V1.0</title>
<link rel="stylesheet" type="text/css" href="../css/common.css">
<script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
<style>
body,html{height:100%;}
</style>
</head>

<body>
<div class="huashu_box">
<ul id="hs_tags" class="box hsb">        
        <li><a href="common/talklist.aspx?keywords=价格">价格</a></li>
        <li><a href="common/talklist.aspx?keywords=竞品">竞品</a></li>
        <li><a href="common/talklist.aspx?keywords=参数">参数</a></li>
        <li><a href="common/talklist.aspx?keywords=交通">交通</a></li>
        <li><a href="common/talklist.aspx?keywords=售后">售后</a></li>
        <li><a href="common/talklist.aspx?keywords=品牌">品牌</a></li>
        <li><a href="common/talklist.aspx?keywords=保险">保险</a></li>
        <li><a href="common/talklist.aspx?keywords=信贷">信贷</a></li>
        <li><a href="common/talklist.aspx?keywords=上牌">上牌</a></li>
</ul>
</div>
<script type="text/javascript">
    jQuery(function ($) {
        $("#hs_tags").find("a").click(function () {
            $('#hsPage2', window.parent.document).attr("src", $(this).attr("href"));
            return false;
        });
    });
</script>	
</body>
</html>