<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="noticeview.aspx.cs" Inherits="ComOpp.BackAdmin.support.noticeview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>公告通知-红旭集团销售客户管理系统V1.0</title>
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
<body><style>
.tzxx_content{padding:50px 10%;}
.tzxx_content h1{height:50px;font-size:24px;color:#009c91;line-height:50px;text-align:center;}
.tzxx_content .info{height:20px;line-height:20px;text-align:center;padding-top:10px;color:#666;}
.tzxx_content .tzxx_body{padding:30px 0;font-size:14px;text-indent:28px;line-height:22px;color:#333;}
</style>
<div class="content_r">
<div class="tzxx_content">
<h1 ><%= CurrentNotice.Title%></h1>
<p class="info"><%= CurrentNotice.AddTime.ToString("yyyy年MM月dd日 HH:mm")%>&nbsp;&nbsp;&nbsp;&nbsp;<%=CurrentNotice.Realname %></p>
<div class="tzxx_body">
<table style="width:100%;">
<tbody>
<tr>
<td>
<%= CurrentNotice.Content%>
</td>
</tr>
</tbody></table>
</div>
</div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        Core.Easyui.resize();
    })
</script>
</body>
</html>