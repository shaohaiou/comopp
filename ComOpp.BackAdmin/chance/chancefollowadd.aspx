<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chancefollowadd.aspx.cs" Inherits="ComOpp.BackAdmin.chance.chancefollowadd" %>
<div>
  <form class="myform" runat="server">
    <table class="guestdata followtable" width="100%">
      <tr>
        <td class="tit">客户姓名：</td>
        <td><%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.Name%></td>
        <td class="tit">客户电话：</td>
        <td><%= CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.Phone%></td>
      </tr>
      <tr>
        <td class="tit">追踪方式：</td>
        <td><select class="easyui-combobox selec" style="width:140px;height:28px;" name="form[connectway]" id="connectway">
                  <option value="">请选择追踪方式</option>
                  <asp:Repeater id="rptConnectway" runat="server">
                    <itemtemplate>
                        <option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
                    </itemtemplate>
                  </asp:Repeater>
                                </select></td>
        <td class="tit">追踪时间：</td>
        <td title="格式:yyyy-mm-dd hh:mm"><input id="followtime" class="easyui-datetimebox" name="form[followtime]" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm") %>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:150px"></td>
      </tr>
      <tr>
        <td class="tit">追踪情况：</td>
        <td colspan="3"><textarea style="width:430px;height:54px;" name="form[followinfo]"></textarea></td>
      </tr>
      <tr>
        <td class="tit">追踪级别：</td>
        <td><select class="easyui-combobox selec" style="width:140px;height:28px;" name="form[customerlevel]" id="customerlevel">
                  <option value="">请选择追踪级别</option>
                  <asp:Repeater id="rptCustomerLevel" runat="server">
                    <itemtemplate>
                        <option value="<%#Eval("ID") %>" <%# SetCustomerLevelSel(Eval("ID")) %>><%#Eval("Name") %></option>
                    </itemtemplate>
                  </asp:Repeater>
                                </select></td>
        <td class="tit">追踪客服：</td>
        <td><select class="easyui-combobox selec" style="width:150px;height:28px;" name="form[uid]" id="uid">
                  <option value="">请选择追踪客服</option>
                  <asp:Repeater id="rptUser" runat="server">
                    <itemtemplate>
                        <option value="<%#Eval("ID") %>" <%# SetUserSel(Eval("ID")) %>><%#Eval("Realname")%></option>
                    </itemtemplate>
                  </asp:Repeater>
                            </select></td>
      </tr>
    </table>
  <div class="formTip fl"></div>
  <div class="call_save_box fr" style="margin:0 20px 10px 0;"><a onClick="javascript:$('#dialog').dialog('close')" class="btn">取消关闭</a></div>
  <div class="call_save_box fr" style="margin:0 20px 10px 0;"><input type="button" class="btn" value="确定提交" onclick="return Core.Easyui.Post(this);"></div>
  </form>
</div>
<script type="text/javascript">
Core.Easyui.Post = function(form){
var form = $(form).parents("form");
var p    = {
"connectway"    : form.find("#connectway").combobox('getValue'),
"followtime"    : form.find("#followtime").datetimebox('getValue'),
"followinfo"    : form.find("textarea[name='form[followinfo]']").val(),
"customerlevel" : form.find("#customerlevel").combobox('getValue'),
"uid"           : form.find("#uid").combobox('getValue'),
"uname"           : form.find("#uid").combobox('getText'),
}

form.find('.formTip').html('');
if(!Core.rule.isNumber('p.integer',p["connectway"])){
form.find('.formTip').html('请选择追踪方式!');return false;
}else if(!Core.rule.isDatetime(p["followtime"])){
form.find('.formTip').html('请设置追踪时间!');return false;
}else if(!Core.rule.common('*',p["followinfo"])){
form.find('.formTip').html('请输入追踪情况!');return false;
}else if(!Core.rule.isNumber('p.integer',p["customerlevel"])){
form.find('.formTip').html('请选择追踪级别!');return false;
}else if(!Core.rule.isNumber('p.integer',p["uid"])){
form.find('.formTip').html('请选择追踪客服!');return false;
}
try{
var t1 = Date.parse(p["followtime"].toString().replace(/-/g, "/"));
var t2 = new Date(Date.parse(new Date())+86400*1000);
var t2 = Date.parse(new Date(t2.getFullYear()+"-"+(t2.getMonth()+1)+"-"+t2.getDate()+" 00:00:00"));

if(t1>=t2){
form.find('.formTip').html('请选择正确的追踪时间!');return false;
}
}catch(e){}	

form.find(":button").attr('disabled','disabled');
$.post('chancefollowadd.aspx?id=<%=GetInt("id") %>',{"form" : p},function(data){
if(data!='y'){
form.find(":button").removeAttr('disabled');
form.find('.formTip').html("数据提交失败!");
}else{
Core.Easyui.reload();$('#dialog').dialog('close');
}
})
}
</script>