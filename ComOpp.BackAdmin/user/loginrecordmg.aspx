<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loginrecordmg.aspx.cs"
    Inherits="ComOpp.BackAdmin.user.loginrecordmg" %>

<!--登录记录 列表框-->
<div id="wrap">
    <!--登录记录 操作按钮-->
    <div id="toolb" style="padding: 0; height: auto">
        <div id="schlist" class="schlist" style="display: block;">
            <form runat="server">
            <div style="padding: 6px 0; border-top: 1px #bbb solid;">
                <label>
                    登录人：</label>
                <select id="So_User" panelheight="auto" style="width: 160px">
                </select>
                <div id="GjSearch" style="margin-top: 6px; height: 30px; overflow: hidden; position: relative;">
                <label>
                    登录时间：</label>
                <input class="easyui-datebox" name="form[starttime]" style="width: 90px;">&nbsp;至&nbsp;<input
                    class="easyui-datebox" name="form[endtime]" style="width: 90px">
                <a onclick="return Core.Easyui.Form(this);" class="easyui-linkbutton" style="margin-left: 20px;">
                    确定</a>
                    <%if (Admin.Administrator)
                      {%>
                    <div class="schlist" style="position: absolute; top: 0; left: 310px;">
                        <div style="padding-top: 1px; line-height: 28px;">
                            <label>
                                公司：</label>
                            <asp:dropdownlist runat="server" id="ddlCorporationSearch">
                            </asp:dropdownlist>
                        </div>
                    </div>
                    <%} %>
                </div>
            </div>
            </form>
        </div>
        <div class="aSearch">
        </div>
    </div>
    <!--登录记录列表-->
    <table id="datagrid">
    </table>
</div>
<script type="text/javascript">
    var corpid =0;
    <%if (Admin.Administrator)
      {%>
    corpid = $("#ddlCorporationSearch option:selected").val();
    <%}else{ %>
    corpid = <%=Admin.CorporationID %>;
    <%} %>
    Core.Easyui.Height = 28;
    Core.Easyui.W = 520;
    Core.Easyui.H = 366;
    Core.Easyui.FieldColumns = [
        { field: 'username', title: '用户名', width: 100, align: 'center' },
        { field: 'logintime', title: '登录时间', width: 140, align: 'center',},
        { field: 'lastloginip', title: '登录ip',align: 'left' }
    ];
    Core.Easyui.Init(
        $('#datagrid')
        , Core.Easyui.Height
        , Core.Easyui.FieldColumns
        , { toolbar: '#toolbar', fitColumns: true}
    );
    Core.Easyui.Form = function (o) {
        var form = $(o).parents('form');
        var p = { 
            "starttime": form.find("input[name='form[starttime]']").val()
            , "endtime": form.find("input[name='form[endtime]']").val()
            ,"uid":form.find("#So_User").combobox('getValue')
        };
        try {
            for (key in p) { Core.Easyui.Params.box[key] = p[key]; }
        } catch (e) { }
        Core.Easyui.load(p);
    }
    $(document).ready(function () {
        $("#ddlCorporationSearch").change(function(){
            corpid = $("#ddlCorporationSearch option:selected").val();
            $("#So_User").combobox({
                method: 'get',
                valueField: 'id',
                textField: 'realnameandgroupname',
                url: "/ajax/getsearchlist.aspx?action=userlist&corpid=" + corpid,
                onLoadSuccess: function () {
                    $("#So_User").combobox('setValue', '').combobox('setText', "请选择...");
                }
            });
            Core.Easyui.Form(this);
        });
        $("#So_User").combobox({
            method: 'get',
            valueField: 'id',
            textField: 'realnameandgroupname',
            url: "/ajax/getsearchlist.aspx?action=userlist&corpid=" + corpid,
            onLoadSuccess: function () {
                $("#So_User").combobox('setValue', '').combobox('setText', "请选择...");
            }
        });

        Core.Easyui.get('/user/loginrecordlist.aspx');
    })
</script>
