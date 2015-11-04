<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import.aspx.cs" Inherits="ComOpp.BackAdmin.chance.import" %>
<div class="fileBox">
  <form method="post" enctype="multipart/form-data" name="myform" action="import.aspx?state=<%=GetInt("state") %>&corpid=<%=GetInt("corpid") %>" onSubmit="return Core.Easyui.Post(this);"> 
      <input type='text' name='excelfield' id='excelfield' class='stxt' placeholder="请选择Excel文件"/>  
      <input type='button' class='mlbtn' value='浏览...' />
      <input type="file" name="fileField" class="sfile" id="fileField" size="28" onchange="document.getElementById('excelfield').value=this.value" />
      <p style="color:#f96120;" id="FormTip" class="mt10">请限定每次数据量为1000条以内！</p>
      <p class="mt10"><a href="/App_Data/archive.xls" style="text-decoration:underline;">下载模板文件</a></p>
      <div class="call_save_box fr" style="margin:5px 20px 10px 0;"><a onClick="javascript:$('#dialog').dialog('close')" class="btn">取消</a></div>
      <div class="call_save_box fr" style="margin:5px 20px 10px 0;"><input type="submit" class="btn" value="确定"></div>
  </form>
</div>
<script type="text/javascript">
    Core.Easyui.Post = function (form) {
        var form = $(form);
        form.find('#FormTip').html('请限定每次数据量为1000条以内！');
        var Field = $("#fileField").val();
        if (Field == "") return false;
        if ($.inArray(Field.substr(Field.lastIndexOf('.')).toLowerCase(), ['.xlsx', '.xls']) == -1) {
            form.find('#FormTip').html("数据导入仅支持Excel文件");
            return false;
        }
        return Core.submit();
    }
</script>