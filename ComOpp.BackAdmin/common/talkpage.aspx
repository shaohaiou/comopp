<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="talkpage.aspx.cs" Inherits="ComOpp.BackAdmin.common.talkpage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>红旭集团商机管理系统V1.0</title>
<link rel="stylesheet" type="text/css" href="../css/common.css">
<script type="text/javascript" src="../js/jquery-1.8.2.min.js"></script>
</head>
<body>
<div class="huashu_box">
<div class="hsDetail">
<h1><%=CurrentTalk == null ? string.Empty : CurrentTalk.Title%></h1>
<div class="hsDetail_body">
<table style="width:100%;"><tr><td style="font-size:12px;color:#666;">
<%=CurrentTalk == null ? string.Empty : CurrentTalk.Content%>
</td></tr></table>
</div>
</div>
</div>	
</body>
</html>