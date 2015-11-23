<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chanceedit.aspx.cs" Inherits="ComOpp.BackAdmin.chance.chanceedit" %>

<style type="text/css">
    table.guestdata
    {
        margin: 0 auto;
        top: -6px;
    }
    table.guestdata tr th
    {
        font-size: 14px;
        font-weight: bold;
        width: 30px;
        padding-left: 10px;
    }
    table.guestdata tr.new-line
    {
        border-bottom: 1px #bbbbbb dashed;
    }
    table.guestdata tr.new-line td
    {
        padding-bottom: 6px;
    }
    table.guestdata tr td textarea
    {
        margin: 0;
        padding: 5px 0 0 0;
        height: 33px;
        width: 594px;
        width: 595px\9;
    }
    table.guestdata tr td input
    {
        height: 24px;
        line-height: 24px;
    }
    .formTip
    {
        padding-left: 40px;
    }
    table.guestdata tr td.tit
    {
        padding-top: 6px;
        width: 96px;
    }
    table.guestdata tr td
    {
        width: 135px;
        padding-top: 6px;
    }
    table.guestdata tr td input
    {
        width: 133px;
    }
    .mytags
    {
        padding-left: 9px;
        height: 30px;
    }
</style>
<div id="TrackBox">
    <form class="myform" method="post" runat="server">
    <table class="guestdata">
        <%if (Action == "提交到到店|洽谈")
          {%>
        <tr class="new-line">
            <th rowspan="1">&nbsp;</th><!--清洗|邀约->到店|洽谈-->
            <td class="tit"><span class="star">*</span> 预约来店时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="invitetime" name="form[invitetime]" value="" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;" ></td>
            <td class="tit">&nbsp;</td>
            <td>&nbsp;</td>
            <td class="tit">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <%}
          else if (Action == "编辑到店|洽谈")
          { %>
        <tr>
            <th rowspan="2">客户到店确认</th><!--预约到店/预约到店->正在商谈-->
            <td class="tit"><span class="star">*</span> 来店时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="arrivetime" name="form[arrivetime]" value="<%=CurrentCustomerInfo.IsVisit == 1 ? CurrentCustomerInfo.VisitTime : string.Empty %>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;"></td>
            <td class="tit"><span class="star">*</span> 来店人数：</td>
            <td><input type="text" name="form[arrivepeoplenum]" value="<%= CurrentCustomerInfo.IsVisit == 1 ? CurrentCustomerInfo.VisitNumber.ToString() : string.Empty%>" /></td>
            <td class="tit"><%= CurrentCustomerInfo.IsVisit == 0 ? "预约来店时间：" : string.Empty %></td>
            <td title="格式:yyyy-mm-dd hh:mm"><%if(CurrentCustomerInfo.IsVisit == 0){ %><input class="easyui-datetimebox" id="invitetime" name="form[invitetime]" value="<%=CurrentCustomerInfo.ReservationTime %>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;" ><%} %></td>
        </tr>
        <tr class="new-line">
            <td class="tit"><span class="star">*</span> 离店时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="leavetime" name="form[leavetime]" value="<%=CurrentCustomerInfo.IsVisit == 1 ? CurrentCustomerInfo.LeaveTime : string.Empty%>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;"></td>
            <td class="tit">接待时长：</td>
            <td title="单位:分钟"><input type="text" name="form[reception]" value="<%=CurrentCustomerInfo.IsVisit == 1 ?  CurrentCustomerInfo.VisitDuration.ToString() : string.Empty %>" id="reception" readonly="readonly" placeholder="单位:分钟" /></td>
            <td class="tit">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <%}
          else if (Action == "新增到店|洽谈")
          {%>
            <tr class="new-line">
            <th rowspan="1">&nbsp;</th><!--意向客户->预约到店-->
            <td class="tit"><span class="star">*</span> 来店时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="arrivetime" name="form[arrivetime]" value="" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;"></td>
            <td class="tit"><span class="star">*</span> 来店人数：</td>
            <td><input type="text" name="form[arrivepeoplenum]" value="" /></td>
            <td class="tit">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <%}
          else if (Action == "提交到追踪|促成"){ %>
        <tr>
            <th rowspan="2">客户到店确认</th><!--预约到店/预约到店->正在商谈-->
            <td class="tit"><span class="star">*</span> 来店时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="arrivetime" name="form[arrivetime]" value="<%=CurrentCustomerInfo.IsVisit == 1 ?   CurrentCustomerInfo.VisitTime : string.Empty%>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;"></td>
            <td class="tit"><span class="star">*</span> 来店人数：</td>
            <td><input type="text" name="form[arrivepeoplenum]" value="<%= CurrentCustomerInfo.IsVisit == 1 ? CurrentCustomerInfo.VisitNumber.ToString() : string.Empty%>" /></td>
            <td class="tit"></td>
            <td title="格式:yyyy-mm-dd hh:mm"></td>
        </tr>
        <tr class="new-line">
            <td class="tit"><span class="star">*</span> 离店时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="leavetime" name="form[leavetime]" value="" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;"></td>
            <td class="tit">接待时长：</td>
            <td title="单位:分钟"><input type="text" name="form[reception]" value="" id="reception" readonly="readonly" placeholder="单位:分钟" /></td>
            <td class="tit">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
          <%}
          else if (Action == "提交到预订|成交" || Action == "编辑预订|成交" || Action == "提交到提车|回访" || Action == "编辑提车|回访")
          { %>
          <tr>
            <th rowspan="<%if (Action == "提交到提车|回访" || Action == "编辑提车|回访" || (Action == "提交到预订|成交" && !string.IsNullOrEmpty(CurrentCustomerInfo.PicupcarTime))){%>3<%}else {%>2<%} %>">选购信息</th><!--正在商谈->选购本牌-->
            <td class="tit"><span class="star">*</span> 选购品牌：</td>
            <td><select style="width:135px;height:26px;" name="form[ibrand]" id="Archive-Brand"></select></td>
            <td class="tit"><span class="star">*</span> 选购车系：</td>
            <td><select style="width:135px;height:26px;" name="form[iseries]" id="Archive-Series" disabled="disabled"></select></td>
            <td class="tit"><span class="star">*</span> 选购车型：</td>
            <td><select style="width:135px;height:26px;" name="form[ispec]" id="Archive-Spec" disabled="disabled"><option value="">请选择汽车车型</option></select></td>
        </tr>
        <tr <%if (Action != "提交到提车|回访" && Action != "编辑提车|回访"){%>class="new-line"<%} %>>
            <td class="tit"><span class="star">*</span>预订成交时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="endtime" name="form[endtime]" value="<%=CurrentCustomerInfo.PlaceOrderTime %>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;" /></td>
            <td class="tit">成交价：</td>
            <td><input name="form[strikeprice]" value="<%=CurrentCustomerInfo.KnockdownPrice %>" type="text" /></td>
            <td class="tit">订单号：</td>
            <td><input name="form[ordernum]" value="<%=CurrentCustomerInfo.OrderNumber %>" type="text" /></td>
        </tr>
            <%if (Action == "提交到提车|回访" || Action == "编辑提车|回访" || (Action == "提交到预订|成交" && !string.IsNullOrEmpty(CurrentCustomerInfo.PicupcarTime)))
              {%>
        <tr class="new-line">
            <td class="tit"><span class="star">*</span> 提车时间：</td>
            <td title="格式:yyyy-mm-dd hh:mm"><input class="easyui-datetimebox" id="delivertime" name="form[delivertime]" value="<%=CurrentCustomerInfo.PicupcarTime %>" data-options="showSeconds:false" title="格式:yyyy-mm-dd hh:mm" style="width:135px;height:26px;line-height:26px;"></td>
            <td colspan="4">&nbsp;</td>
        </tr>
            <%} %>
          <%}else if(Action=="提交到战败") {%>
          <tr class="new-line">
            <th rowspan="1">&nbsp;</th>
            <td class="tit"><span class="star">*</span> 战败原因：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[giveupcause]" id="giveupcause">
                  <option value="">请选择放弃原因</option>
                    <asp:Repeater runat="server" id="rptGiveupCause">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                                </select></td>
            <td class="tit"><span class="star">*</span> 原因分析：</td>
            <td colspan="3"><input name="form[failurereason]" value="" type="text" style="width:99.4%;" /></td>
        </tr>
          <%} %>
        <tr>
            <th rowspan="3">
                基本资料
            </th>
            <!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit">
                <span class="star">*</span> 客户姓名：
            </td>
            <td>
                <input name="form[uname]" type="text" value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.Name %>" />
            </td>
            <td class="tit">
                <span class="star">*</span> 客户电话：
            </td>
            <td>
                <input name="form[phone]" type="text" <%= CurrentCustomerInfo == null ? string.Empty : "disabled=\"disabled\""%> value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.Phone %>" title="" onchange="javascript:checkphone(this);" />
            </td>
            <td class="tit">
                备用电话：
            </td>
            <td>
                <input name="form[sparephone]" type="text" value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.BackupPhone %>" />
            </td>
        </tr>
        <tr>
            <td class="tit">
                客户地址：
            </td>
            <td colspan="5">
                <select name="form[province]" id="province" class="selec" style="width: 118px; height: 26px;">
                    <option value="">选择省份</option>
                </select>
                <select name="form[city]" id="city" class="selec" style="width: 118px; height: 26px;"
                    disabled="disabled">
                    <option value="">选择城市</option>
                </select>
                <select name="form[district]" id="district" class="selec" style="width: 118px; height: 26px;"
                    disabled="disabled">
                    <option value="">选择地区</option>
                </select>
                <input name="form[address]" placeholder="具体地址" type="text" value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.Address %>" style="width: 223px;" />
            </td>
        </tr>
        <tr class="new-line">
            <td class="tit">
                微信帐号：
            </td>
            <td>
                <input name="form[weixin]" value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.WeixinAccount %>" type="text" />
            </td>
            <td colspan="4">
                &nbsp;
            </td>
        </tr>
        <tr>
            <th rowspan="3">
                拟购信息
            </th>
            <!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit">
                <span class="star">*</span> 信息类型：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[infotype]"
                    id="infotype">
                    <option value="">请选择信息类型</option>
                    <asp:Repeater runat="server" id="rptInfoType">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>" <%# SetInfoTypeSel(Eval("ID")) %>><%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
            <td class="tit">
                <span class="star">*</span> 信息来源：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[infosource]"
                    id="infosource">
                    <option value="">请选择...</option>
                    <asp:Repeater runat="server" id="rptInfoSource">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>" <%# SetInfoSourceSel(Eval("ID")) %>><%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
            <td class="tit">
                支付方式：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[paymentway]"
                    id="paymentway">
                    <option value="">请选择...</option>
                    <asp:Repeater runat="server" id="rptPaymentway">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>" <%# SetPaymentwaySel(Eval("ID")) %>><%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
        </tr>
        <%if (Action == "提交到预订|成交" || Action == "编辑预订|成交" || Action == "提交到提车|回访" || Action == "编辑提车|回访")
          { %>
        <tr>
            <td class="tit">拟购品牌：</td>
            <td><input type="text" value="<%=CurrentCustomerInfo.IbuyCarBrand %>" disabled="disabled"></td>
            <td class="tit">拟购车系：</td>
            <td><input type="text" value="<%=CurrentCustomerInfo.IbuyCarSeries %>" disabled="disabled"></td>
            <td class="tit">拟购车型：</td>
            <td><input type="text" value="<%=CurrentCustomerInfo.IbuyCarModel %>" disabled="disabled"></td>
        </tr>
        <%}else{ %>
        <tr>
            <td class="tit">
                拟购品牌：
            </td>
            <td>
                <select style="width: 135px; height: 26px;" name="form[brand]" id="Archive-Brand">
                </select>
            </td>
            <td class="tit">
                拟购车系：
            </td>
            <td>
                <select style="width: 135px; height: 26px;" name="form[series]" id="Archive-Series"
                    disabled="disabled">
                    <option value="">选择汽车车系</option>
                </select>
            </td>
            <td class="tit">
                拟购车型：
            </td>
            <td>
                <select style="width: 135px; height: 26px;" name="form[spec]" id="Archive-Spec" disabled="disabled">
                    <option value="">请选择汽车车型</option>
                </select>
            </td>
        </tr>
        <%} %>
        <tr class="new-line">
            <td class="tit">
                拟购时间：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[ibuytime]"
                    id="ibuytime">
                    <option value="">请选择...</option>
                    <asp:Repeater runat="server" id="rptIbuytime">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>" <%# SetIbuytimeSel(Eval("ID")) %>><%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
        </tr>
        <tr>
            <th rowspan="2">
                沟通信息
            </th>
            <!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit">
                报价信息：
            </td>
            <td>
                <input name="form[price]" type="text" value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.QuotedpriceInfo %>" />
            </td>
            <td class="tit">
                促销内容：
            </td>
            <td colspan="3">
                <input name="form[slogan]" value="<%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.PromotionInfo %>" type="text" style="width: 99.4%;" />
            </td>
        </tr>
        <tr class="new-line">
            <td class="tit">
                备注说明：
            </td>
            <td colspan="5">
                <textarea name="form[content]"><%=CurrentCustomerInfo == null ? string.Empty : CurrentCustomerInfo.RemarkInfo%></textarea>
            </td>
        </tr>
        <tr>
            <th rowspan="2">
                &nbsp;
            </th>
            <!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit">
                线索所有人：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[owneruid]"
                    id="owneruid">
                    <option value="">请选择...</option>
                    <asp:Repeater runat="server" id="rptOwner">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>" <%# SetOwnerSel(Eval("ID")) %>><%#Eval("RealnameAndGroupname")%></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
            <td class="tit">
                线索状态：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[customerstatus]" id="customerstatus" disabled="disabled">
                    <option value="">请选择...</option>
                    <asp:Repeater runat="server" id="rptCustomerStatus">
                        <ItemTemplate>
                            <option value="<%#Eval("Value") %>" <%# SetCustomerStatusSel(Eval("Value")) %>><%#Eval("Text") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
            <td class="tit">
                客户性别：
            </td>
            <td>
                <select class="easyui-combobox" style="width: 135px; height: 26px;" name="form[sex]"
                    id="sex">
                    <option value="0"<%= (CurrentCustomerInfo != null && CurrentCustomerInfo.CustomerSex == 0) ? " selected=\"selected\"" : string.Empty %>>保密</option>
                    <option value="1"<%= (CurrentCustomerInfo != null && CurrentCustomerInfo.CustomerSex == 1) ? " selected=\"selected\"" : string.Empty %>>男</option>
                    <option value="2"<%= (CurrentCustomerInfo != null && CurrentCustomerInfo.CustomerSex == 2) ? " selected=\"selected\"" : string.Empty %>>女</option>
                </select>
            </td>
        </tr>
        <tr>
            <td class="tit">
                线索标签：
            </td>
            <td>
                <select id="getTags" style="width: 135px; height: 26px;">
                    <option value="">请选择...</option>
                    <asp:Repeater runat="server" id="rptTracktag">
                        <ItemTemplate>
                            <option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </td>
            <td colspan="6">
                <p id="mytags" class="mytags">
                    <asp:Repeater runat="server" id="rpttags">
                        <ItemTemplate>
                            <a data-key="<%#Eval("ID") %>" title="<%#Eval("Name") %>"><span><%#Eval("Name") %></span><i></i></a>
                        </ItemTemplate>
                    </asp:Repeater>
                </p>
            </td>
        </tr>
    </table>
    <div class="formTip fl">
        <p id="formTips">
            请将信息填写完整</p>
    </div>
    <div class="call_save_box fr" style="margin: -7px 20px 10px 0;">
        <a onclick="javascript:$('#dialog').dialog('close')" class="btn">取消关闭</a></div>
    <div class="call_save_box fr" style="margin: -7px 15px 10px 0;">
        <input type="button" class="btn" value="<%=SubmitText %>" onclick="return Core.Easyui.Post(this);"></div>
    </form>
</div>
<script type="text/javascript">
    Core.Parameter = {
        config: {
            "invitetime": { "id": "#invitetime", "type": "datetimebox" },
            "arrivetime": { "id": "#arrivetime", "type": "datetimebox" },
            "arrivepeoplenum": { "id": ":text[name='form[arrivepeoplenum]']", "type": "text" },
            "leavetime": { "id": "#leavetime", "type": "datetimebox" },
            "reception": { "id": ":text[name='form[reception]']", "type": "text" },
            "strikeprice": { "id": ":text[name='form[strikeprice]']", "type": "text" },
            "ordernum": { "id": ":text[name='form[ordernum]']", "type": "text" },
            "carnum": { "id": ":text[name='form[carnum]']", "type": "text" },
            "endtime": { "id": "#endtime", "type": "datetimebox" },
            "delivertime": { "id": "#delivertime", "type": "datetimebox" },
            "giveupcause": { "id": "#giveupcause", "type": "combobox" },
            "failurereason": { "id": ":text[name='form[failurereason]']", "type": "text" }
        },
        loader: function (form, p) {
            var row = null;
            for (key in Core.Parameter.config) {
                row = Core.Parameter.config[key];
                if (!form.find(row["id"]).length) continue;
                try {
                    if (row["type"] == "combobox") {
                        p[key] = form.find(row["id"]).combobox('getValue');
                    } else if (row["type"] == "datetimebox") {
                        p[key] = form.find(row["id"]).datetimebox('getValue');
                    } else {
                        p[key] = form.find(row["id"]).val();
                    }
                } catch (e) { }
            }
            return p;
        }
    };
    Core.Easyui.Post = function (form) {
        var form = $(form).parents("form");
        form.find('.formTip').html('');
        var p = {
            "uname": form.find(":text[name='form[uname]']").val(),
            "phone": form.find(":text[name='form[phone]']").val(),
            "sparephone": form.find(":text[name='form[sparephone]']").val(),
            "province": form.find("#province").combobox('getValue'),
            "city": form.find("#city").combobox('getValue'),
            "district": form.find("#district").combobox('getValue'),
            "address": form.find(":text[name='form[address]']").val(),
            "weixin": form.find(":text[name='form[weixin]']").val(),
            "infotype": form.find("#infotype").combobox('getValue'),
            "infosource": form.find("#infosource").combobox('getValue'),
            "paymentway": form.find("#paymentway").combobox('getValue'),
            "ibuytime": form.find("#ibuytime").combobox('getValue'),
            "price": form.find(":text[name='form[price]']").val(),
            "slogan": form.find(":text[name='form[slogan]']").val(),
            "content": form.find("textarea[name='form[content]']").val(),
            "owneruid": form.find("#owneruid").combobox('getValue'),
            "owner": form.find("#owneruid").combobox('getText'),
            "sex": form.find("#sex").combobox('getValue'),
            "brand": form.find("#Archive-Brand").combobox('getValue'),
            "series": form.find("#Archive-Series").combobox('getValue'),
            "spec": form.find("#Archive-Spec").combobox('getValue'),
            "customerstatus": form.find("#customerstatus").combobox('getValue'),
            "tracktag": "",
            "tracktagname": ""
        };

        p = Core.Parameter.loader(form, p);
        
        <%if (Action == "提交到到店|洽谈")
          {%>
        //意向客户->预约到店/预约到店编辑
        if (!Core.rule.isDatetime(p["invitetime"])) {
            form.find('.formTip').html('请设置预约来店时间!'); return false;
        }
        <%}
          else if (Action == "编辑到店|洽谈" || Action == "提交到追踪|促成")
          { %>
        if (!Core.rule.isDatetime(p["arrivetime"])) {
            form.find('.formTip').html('请设置来店时间!'); return false;
        }
        if (!Core.rule.isNumber('p.integer',p["arrivepeoplenum"])) {
            form.find('.formTip').html('请设置来店人数!'); return false;
        }
        if (!Core.rule.isDatetime(p["leavetime"])) {
            form.find('.formTip').html('请设置离店时间!'); return false;
        }
        <%}else if(Action == "新增到店|洽谈") {%>
        if (!Core.rule.isDatetime(p["arrivetime"])) {
            form.find('.formTip').html('请设置来店时间!'); return false;
        }
        if (!Core.rule.isNumber('p.integer',p["arrivepeoplenum"])) {
            form.find('.formTip').html('请设置来店人数!'); return false;
        }
        <%} else if(Action == "提交到预订|成交" || Action == "编辑预订|成交" || Action == "提交到提车|回访" || Action == "编辑提车|回访") {%>
        if (!Core.rule.isDatetime(p["endtime"])) {
            form.find('.formTip').html('请设置预订成交时间!'); return false;
        }else if (!Core.rule.isNumber('p.integer', p["brand"])) {
            form.find('.formTip').html('请选择品牌!'); return false;
        }else if (!Core.rule.isNumber('p.integer', p["series"])) {
            form.find('.formTip').html('请选择车系!'); return false;
        }else if (!Core.rule.isNumber('p.integer', p["spec"])) {
            form.find('.formTip').html('请选择车型!'); return false;
        }
        <%if(Action == "提交到提车|回访") {%>
        if (!Core.rule.isDatetime(p["delivertime"])) {
            form.find('.formTip').html('请设置提车时间!'); return false;
        }
        <%} %>
        <%}else if(Action == "提交到战败") {%>
        if (!Core.rule.isNumber('p.integer', p["giveupcause"])) {
            form.find('.formTip').html('请选择放弃原因!'); return false;
        }else if(!Core.rule.common('*', p["failurereason"])){
            form.find('.formTip').html('请输入原因分析!'); return false;
        }
        <%} %>
        if (!Core.rule.common('*', p["uname"])) {
            form.find('.formTip').html('请输入客户姓名!'); return false;
        } else if (!Core.rule.common('*', p["phone"])) {
            form.find('.formTip').html('请输入客户电话!'); return false;
        } else if (!Core.rule.isNumber('p.integer', p["infotype"])) {
            form.find('.formTip').html('请选择信息类型!'); return false;
        } else if (!Core.rule.isNumber('p.integer', p["infosource"])) {
            form.find('.formTip').html('请选择信息来源!'); return false;
        }
        if (form.find("input[name='form[phone]']").attr('title') != '') {
            form.find('.formTip').html('该客户电话已经存在或不符合规范!'); return false;
        }
        var o = form.find("#mytags a");
        var tag = [];
        var tagname = [];
        for (i = 0; i < o.length; i++) {
            tag.push($(o[i]).attr("data-key"));
            tagname.push($(o[i]).attr("title")); 
        }
        form.append('<input type="hidden" name="form[tracktag]" value="' + tag.join(',') + '">');
        form.append('<input type="hidden" name="form[tracktagname]" value="' + tagname.join(',') + '">');
        p["tracktag"] = tag.join(',');
        p["tracktagname"] = tagname.join(',');
        //return Core.submit()
        form.find(":button").attr('disabled', 'disabled');
        $.post('chanceedit.aspx?id=<%=GetInt("id") %>&state=<%= GetInt("state")%>&corpid=<%=GetInt("corpid") %>', { "form": p }, function (data) {
            if (data != 'y') {
                form.find(":button").removeAttr('disabled');
                form.find('.formTip').html("数据提交失败!");
            } else {
                Core.Easyui.reload(); $('#dialog').dialog('close');
            }
        })

    }
    Core.Easyui.ArriveTime = function () {
        $(":text#reception").val(0);
        var arrivetime = $('#arrivetime').datetimebox('getValue');
        var leavetime = $('#leavetime').datetimebox('getValue');
        if (!Core.rule.isDatetime(arrivetime) || !Core.rule.isDatetime(leavetime)) return false;
        arrivetime = new Date(arrivetime);
        leavetime = new Date(leavetime);
        var r = (leavetime.getTime() - arrivetime.getTime()) / (1000 * 60);

        if (!Core.rule.isNumber('p.integer', r)) return false;
        $(":text#reception").val((leavetime.getTime() - arrivetime.getTime()) / (1000 * 60));
        return true;
    }
    $(document).ready(function () {
        $(".datebox-button a").live('click',function(){
            if($(this).text()!='确定') return;
            Core.Easyui.ArriveTime();
        });

        Core.UnionCombobox('district', {
            "layer1": [$('#province'), "<%= ProvinceID %>", "../ajax/getdistrict.aspx?t=1", '选择省份'],
            "layer2": [$('#city'), "<%= CityID %>", "../ajax/getdistrict.aspx?t=2&pid=@pid", '选择城市'],
            "layer3": [$('#district'), "<%= DistrictID %>", "../ajax/getdistrict.aspx?t=3&pid=@pid", '选择地区']
        });
        Core.UnionCombobox('brand', {
            "layer1": [$("#Archive-Brand"), "<%= CarBrandID %>", "../ajax/getcarbrand.aspx?t=1&id=@id", '选择车辆品牌'],
            "layer2": [$("#Archive-Series"), "<%= CarSeriesID %>", "../ajax/getcarbrand.aspx?t=2&pid=@pid", '选择汽车车系'],
            "layer3": [$("#Archive-Spec"), "<%= CarModelID %>", "../ajax/getcarbrand.aspx?t=3&pid=@pid", '请选择汽车车型']
        });

        /*客户信息 增加线索标签选项*/
        $('#getTags').combobox({
            valueField: 'value',
            textField: 'text',
            onSelect: function (rec) {
                if (!Core.rule.isNumber('p.integer', rec.value)) return;
                if (!$("#mytags").find("a[data-key='" + rec.value + "']").length && $("#mytags").find("a").length >= 5) {
                    $.messager.alert('提示', '最多只能选中5个标签', 'info'); return;
                } else if ($("#mytags").find("a[data-key='" + rec.value + "']").length) {
                    $("#mytags").find("a[data-key='" + rec.value + "']").html('<span title="' + rec.text + '">' + rec.text + '</span><i></i>'); return;
                } else {
                    $("#mytags").append('<a data-key="' + rec.value + '" title="' + rec.text + '"><span>' + rec.text + '</span><i></i></a>');
                }
            }
        });
        /*点击删除某个线索标签*/
        $("#mytags a i").live("click", function () {
            $(this).parent("a").remove();
        });
    });

    function checkphone(o) {
        $.ajax({
            url: 'checkcustomerphone.axd?d=' + new Date(),
            async: false,
            dataType: "json",
            data: { phone: $(o).val(),corpid:<%= GetInt("corpid") %> },
            error: function (msg) {

            },
            success: function (data) {
                if (data.result == 'success') {
                }
                else {
                    var form = $(o).parents("form");
                    form.find('.formTip').html('该客户电话已经存在或不符合规范!'); 
                    $(o).attr("title", $(o).val());
                    if(data.isdel == "1"){
                        $.messager.confirm('激活','该客户电话已经存在于删除库中,是否激活?',function(r){
                            if(!r) return;
                            dialog('755','435',"激活已删除客户","chanceedit.aspx?id="+data.id+"&action=recover&state=2&r="+Math.random());return;
                        });
                    }
                }
            }
        });
    }
</script>
