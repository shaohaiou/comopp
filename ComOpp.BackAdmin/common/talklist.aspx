<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="talklist.aspx.cs" Inherits="ComOpp.BackAdmin.common.talklist" %>

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
<div id="mainhsList" class="box ml5" style="display:block">
    <%=GetTalklist() %>
            </div>
    <div style="padding:0px 5px"></div>
</div>	
<script type="text/javascript">
    jQuery(function ($) {
        $("#mainhsList").find("a").click(function () {
            $('#hsPage3', window.parent.document).attr("src", $(this).attr("href"));
            return false;
        });
    });
</script>	
</body>
</html>
