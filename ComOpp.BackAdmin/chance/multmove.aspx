<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="multmove.aspx.cs" Inherits="ComOpp.BackAdmin.chance.multmove" %>
<div>
  <div class="handover" style="line-height:30px;text-align:center;display:none">
    <div style="margin-top:20px">数据处理中,请勿关闭窗口!</div>
    <div class="progress">进度：<span class="page">0</span>/<span class="maxpage">0</span></div>
  </div>
  <form runat="server" name="handover" id="form1">
  <table class="guestdata">
    <%if (string.IsNullOrEmpty(GetString("ids")))
      { %>
      <tr>
        <td class="tit" style="width:130px">数据转出者：</td>
        <td><select class="easyui-combobox" id="source" style="width:135px;height:26px;" name="form[source]">
                  <option value="">请选择...</option>
                  <asp:Repeater runat="server" id="rptOuter">
                    <itemtemplate>
                        <option value="<%#Eval("ID") %>" <%# SetAdminSel(Eval("ID")) %>><%#Eval("RealnameAndGroupname")%></option>
                    </itemtemplate>
                  </asp:Repeater>
            </select></td>
    </tr>
    <%}
      else
      { %>
        <tr>
        <td class="tit" style="width:130px">转出编号：</td>
        <td><div style="width:200px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;" title="<%=GetString("iorderid") %>"><%=GetString("iorderid") %></div></td>
    </tr>
    <%} %>
        <tr>
        <td class="tit" style="width:130px">数据转入者：</td>
        <td><select class="easyui-combobox" id="owneruid" style="width:135px;height:26px;" name="form[owneruid]">
                  <option value="">请选择...</option>
                  <asp:Repeater runat="server" id="rptOwner">
                    <itemtemplate>
                        <option value="<%#Eval("ID") %>"><%#Eval("RealnameAndGroupname")%></option>
                    </itemtemplate>
                  </asp:Repeater>
                                </select></td>
    </tr>
    <tr>
        <td class="tit" style="width:130px">确认账户密码：</td>
        <td><input type="password" name="form[password]" style="width:135px; line-height:26px"></td>
    </tr>
  </table>
  <%if(string.IsNullOrEmpty(GetString("ids"))){%>
  <div style="margin-left:130px; line-height:22px;color:red">
    <input type="checkbox" name="form[confirm]" id="checkbox" /> 确定要转移该用户所有线索
  </div>
  <%} %>
    <div class="call_save_box fl" style="margin:0px 0px 0px 90px;"><input type="button" onclick="Core.Easyui.Post(this);" class="btn" value="确定提交"></div>
  <div class="call_save_box fl" style="margin:0px 20px 0px 20px;"><a onClick="javascript:$('#dialog').dialog('close')" class="btn">取消关闭</a></div>
  <div class="formTip fr" style="padding:0px;line-height:26px; padding-right:8px">请设置完整的转移信息</div>
  </form>
</div>
<script type="text/javascript">
    Core.Easyui.Post = function () {
        var form = $("#form1");
        var params = { "owneruid": $('#owneruid').combobox('getValue'), "password": form.find(":password[name='form[password]']").val() };
        <%if(string.IsNullOrEmpty(GetString("ids"))){ %>
        params['source'] = $('#source').combobox('getValue');
        params["method"] = 'member';
        <%}else{ %>
        params['ids'] = '<%=GetString("ids") %>';
        params["method"] = 'ids';
        <%} %>
        switch (params["method"]) {
            case 'ids':
                if (!Core.rule.common('*', params['ids'])) {
                    form.find('.formTip').html('数据加载失败'); return false;
                }
                if (!Core.rule.isNumber('p.integer', params['owneruid'])) {
                    form.find('.formTip').html('请选择数据转入者'); return false;
                }
                if (!Core.rule.common('pwd', params['password'])) {
                    form.find('.formTip').html('账户密码错误'); return false;
                }
                break; 
            case 'member':
                if (!Core.rule.isNumber('p.integer', params['source'])) {
                    form.find('.formTip').html('请选择数据转出者'); return false;
                }
                if (!Core.rule.isNumber('p.integer', params['owneruid'])) {
                    form.find('.formTip').html('请选择数据转入者'); return false;
                }
                if (params['source'] == params['owneruid']) {
                    form.find('.formTip').html('数据转出者和转入者相同!'); return false;
                }
                if (!Core.rule.common('pwd', params['password'])) {
                    form.find('.formTip').html('账户密码错误'); return false;
                }
                if (!form.find(":checked[name='form[confirm]']").length) {
                    form.find('.formTip').html('请确认转移数据!'); return false;
                }
                break;
        }
        if (!confirm("您确定要将选中的数据进行批量权限移交吗？")) return false;

        form.hide();
        $(".handover").show();
        Core.Easyui.Handover(params, 1);
    }
    Core.Easyui.Handover = function (params, page) {
        params['page'] = page;
        $.get("multmove.aspx", $.param(params), function (data) {
            if (data.indexOf('success,') == -1) {
                $(".progress").html('<span style="color:red">程序执行失败</span>'); return false;
            }
            var p = data.split(',');
            if (p[1] == 'error.pwd') {
                $(".progress").html('<span style="color:red">账户密码错误</span>'); return false;
            } else if (parseInt(p[2]) == 0) {
                $(".progress").html('<span style="color:red">数据处理成功!</span>');
                setTimeout(function () { window.location.reload(); }, 1000);
                return false;
            } else if (Core.rule.isNumber('p.integer', p[2]) && Core.rule.isNumber('p.integer', p[3])) {
                p[1] = parseInt(p[1]);
                p[2] = parseInt(p[2]);
                p[3] = parseInt(p[3]);
                $(".progress .page").html(p[3]);
                $(".progress .maxpage").html(p[2]);

                if (p[3] < p[2]) Core.Easyui.Handover(params, parseInt(p[3]) + 1);
                if (p[3] >= p[2]) {
                    $(".progress").html('<span style="color:green">数据处理成功!</span>');
                    window.location.reload();
                    return false;
                }
            }
        });
    }
</script>