<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="export.aspx.cs" Inherits="ComOpp.BackAdmin.chance.export" %>
<style type="text/css">
.Exceldown a {margin-right:5px;color:blue}
</style>
<div class="Excelbox">
      <div style="margin-left:30px;line-height:26px;">总文件数：1份</div>
   <div style="margin-left:30px;line-height:26px;" class="Exceldown" data-max="1">下载文件：</div>
   <div id="ExcelWin" data-link="excel.aspx?corpid=<%=GetInt("corpid") %>&ids=<%=GetString("ids") %>" style="display:none"></div>
   </div>
<script type="text/javascript">
    $(document).ready(function () {
        for (var i = 1; i <= $(".Exceldown").attr("data-max"); i++) {
            $(".Exceldown").append('<a data-link="' + $("#ExcelWin").attr('data-link') + '" title="点击下载">第' + i + '份</a>');
        }
        $(".Exceldown a").live('click', function () {
            $("#ExcelWin").html('<iframe src="' + $(this).attr('data-link') + '" frameborder="0" width="0" height="0" scrolling="no" style="display:none"></iframe>');
        });
    });
</script>