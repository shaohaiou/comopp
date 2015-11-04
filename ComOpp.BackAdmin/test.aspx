<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="ComOpp.BackAdmin.test" %>
<style type="text/css">
table.guestdata{margin:0 auto;top:-6px;}
table.guestdata tr th{font-size:14px;font-weight:bold;width:30px;padding-left:10px;}
table.guestdata tr.new-line{border-bottom:1px #bbbbbb dashed;}
table.guestdata tr.new-line td{padding-bottom:6px;}
table.guestdata tr td textarea{margin:0;padding:5px 0 0 0;height:33px;width:594px;width:595px\9;}
table.guestdata tr td input{height:24px;line-height:24px;}
.formTip{padding-left:40px;}
table.guestdata tr td.tit{padding-top:6px;width:96px;}
table.guestdata tr td{width:135px;padding-top:6px;}
table.guestdata tr td input{width:133px;}
.mytags{padding-left:9px;height:30px;}
</style>
<div id="TrackBox">
  <form class="myform" method="post" action="http://sales.new4s.com/chance/callcenter/index/set">
    <table class="guestdata">        
                <tr>
            <th rowspan="3">基本资料</th><!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit"><span class="star">*</span> 客户姓名：</td>
            <td><input name="form[uname]" type="text" value="" /></td>
            <td class="tit"><span class="star">*</span> 客户电话：</td>
            <td><input name="form[phone]" type="text" value="158882085459" title=""  /></td>
            <td class="tit">备用电话：</td>
            <td><input name="form[sparephone]" type="text" value="" /></td>
        </tr>
        <tr>
            <td class="tit">客户地址：</td>
            <td colspan="5"><select name="form[province]" id="province" class="selec" style="width:118px;height:26px;"><option value="">选择省份</option></select>
          <select name="form[city]" id="city" class="selec" style="width:118px;height:26px;" disabled="disabled"><option value="">选择城市</option></select>
          <select name="form[district]" id="district" class="selec" style="width:118px;height:26px;" disabled="disabled"><option value="">选择地区</option></select>
          <input name="form[address]" placeholder="具体地址" type="text" value="" style="width:223px;" />
            </td>
        </tr>
        <tr class="new-line">
            <td class="tit">微信帐号：</td>
            <td><input name="form[weixin]" value="" type="text"/></td>
            <td colspan="4">&nbsp;</td>
        </tr>
        <tr>
            <th rowspan="3">拟购信息</th><!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit"><span class="star">*</span> 信息类型：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[infotype]" id="infotype">
                  <option value="">请选择信息类型</option>
                                    <option value="756" >展厅</option>
                                    <option value="757" >前台电话</option>
                                    <option value="758" >网站订单</option>
                                    <option value="856" >400电话</option>
                                </select></td>
            <td class="tit"><span class="star">*</span> 信息来源：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[infosource]" id="infosource">
                  <option value="">请选择...</option>
                                    <option value="4" >汽车之家</option>
                                    <option value="5" >易车网</option>
                                    <option value="4228" >车云无线CPA</option>
                                    <option value="6" >太平洋汽车网</option>
                                    <option value="7" >网上车市</option>
                                    <option value="8" >爱卡汽车网</option>
                                    <option value="3" >车经纪</option>
                                    <option value="9" >搜狐汽车</option>
                                    <option value="10" >腾讯汽车</option>
                                    <option value="11" >新浪汽车</option>
                                    <option value="12" >凤凰汽车</option>
                                    <option value="1017" >前台来电</option>
                                    <option value="1018" >400电话</option>
                                    <option value="1019" >老客户介绍</option>
                                    <option value="2590" >户外广告/展示</option>
                                    <option value="2591" >触点</option>
                                    <option value="2592" >品牌效应</option>
                                    <option value="2593" >朋友推介</option>
                                    <option value="2594" >路过</option>
                                    <option value="2595" >老客户</option>
                                    <option value="2596" >之前来过</option>
                                    <option value="2597" >家住附近</option>
                                    <option value="2598" >其他</option>
                                </select></td>
            <td class="tit">支付方式：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[paymentway]" id="paymentway">
                  <option value="">请选择...</option>
                                    <option value="766" >按揭</option>
                                    <option value="767" >全款</option>
                                </select></td>
        </tr>
                <tr>
            <td class="tit">拟购品牌：</td>
            <td><select style="width:135px;height:26px;" name="form[brand]" id="Archive-Brand"></select></td>
            <td class="tit">拟购车系：</td>
            <td><select style="width:135px;height:26px;" name="form[series]" id="Archive-Series" disabled="disabled"></select></td>
            <td class="tit">拟购车型：</td>
            <td><select style="width:135px;height:26px;" name="form[spec]" id="Archive-Spec" disabled="disabled"><option value="">请选择汽车车型</option></select></td>
        </tr>
                <tr class="new-line">
            <td class="tit">拟购时间：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[ibuytime]" id="ibuytime">
                  <option value="">请选择...</option>
                                    <option value="768" >7天内</option>
                                    <option value="769" >15天内</option>
                                    <option value="770" >1个月内</option>
                                    <option value="771" >3个月内</option>
                                    <option value="772" >半年内</option>
                                    <option value="773" >半年以上</option>
                                </select></td>
        </tr>
        <tr>
            <th rowspan="2">沟通信息</th><!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit">报价信息：</td>
            <td><input name="form[price]" type="text" value="" /></td>
            <td class="tit">促销内容：</td>
            <td colspan="3"><input name="form[slogan]" value="" type="text" style="width:99.4%;" /></td>
        </tr>
        <tr class="new-line">
            <td class="tit">备注说明：</td>
            <td colspan="5"><textarea name="form[content]"></textarea></td>
        </tr>
        <tr>
            <th rowspan="2">&nbsp;</th><!--表格中横跨行的单元格 居左显示模块名称-->
            <td class="tit">线索所有人：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[owneruid]" id="owneruid">
                  <option value="">请选择...</option>
                                    <option value="117" >王玲彦</option>
                                    <option value="951" >黄隽瑶</option>
                                    <option value="150" >林新海</option>
                                    <option value="129" >沈蓉蓉</option>
                                    <option value="1032" >刘佳佳</option>
                                    <option value="126" >蔡静静</option>
                                    <option value="735" >戴晓武dcc</option>
                                    <option value="141" >林伟</option>
                                    <option value="992" >陈清锡</option>
                                    <option value="620" >林峰</option>
                                    <option value="619" >阮利明</option>
                                    <option value="618" >陈仁义</option>
                                    <option value="616" >高存飞</option>
                                    <option value="615" >沈锋</option>
                                    <option value="614" >曹阁</option>
                                    <option value="613" >陈洁</option>
                                    <option value="612" >曹高翔</option>
                                    <option value="610" >张青青</option>
                                    <option value="608" >李娟</option>
                                    <option value="607" >毛明珠</option>
                                    <option value="606" >蔡晓清</option>
                                    <option value="151" >蔡永久</option>
                                    <option value="125" >戴晓武</option>
                                    <option value="142" >卢子君</option>
                            </select></td>
            <td class="tit">线索状态：</td>	
            <td><select class="easyui-combobox" style="width:135px;height:26px;" disabled="disabled">
            <option value="">请选择...</option>
                        <option value="1" >导入|集客</option>
                        <option value="2"  selected="selected">清洗|邀约</option>
                        <option value="3" >到店|洽谈</option>
                        <option value="4" >追踪|促成</option>
                        <option value="5" >预订|成交</option>
                        <option value="10" >提车|回访</option>
                    </select></td>
            <td class="tit">客户性别：</td>
            <td><select class="easyui-combobox" style="width:135px;height:26px;" name="form[sex]" id="sex">
                                </select></td>
        </tr>
        <tr>
            <td class="tit">线索标签：</td>				
            <td><select id="getTags" style="width:135px;height:26px;">
            <option value="">请选择...</option>
                        <option value="774">团购活动报名</option>
                        <option value="775">团购活动到场</option>
                      </select></td>
            <td colspan="6"><p id="mytags" class="mytags">
                                                                                                    </p></td>		
        </tr>
    </table>
  <div class="formTip fl"><p id="formTips">请将信息填写完整</p></div>
  <div class="call_save_box fr" style="margin:-7px 20px 10px 0;"><a onClick="javascript:$('#dialog').dialog('close')" class="btn">取消关闭</a></div>
  <div class="call_save_box fr" style="margin:-7px 15px 10px 0;"><input type="button" class="btn" value="确定提交" onclick="return Core.Easyui.Post(this);"></div>
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
            "sex": form.find("#sex").combobox('getValue'),
            "brand": form.find("#Archive-Brand").combobox('getValue'),
            "series": form.find("#Archive-Series").combobox('getValue'),
            "spec": form.find("#Archive-Spec").combobox('getValue'),
            "tracktag": ""
        };

        p = Core.Parameter.loader(form, p);
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
        for (i = 0; i < o.length; i++) { tag.push($(o[i]).attr("data-key")); }
        form.append('<input type="hidden" name="form[tracktag]" value="' + tag.join(',') + '">');
        p["tracktag"] = tag.join(',');
        //return Core.submit()
        form.find(":button").attr('disabled', 'disabled');
        $.post("http://sales.new4s.com/chance/callcenter/index/set", { "form": p }, function (data) {
            if (data != 'y') {
                form.find(":button").removeAttr('disabled');
                form.find('.formTip').html("数据提交失败!");
            } else {
                Core.Easyui.reload(); $('#dialog').dialog('close');
            }
        })

    }
    Core.Easyui.box["Repeat"] = true;
    Core.Easyui.Repeat = function () {
        if (!Core.Easyui.box["Repeat"]) return false;
        Core.Easyui.box["Repeat"] = false;
        var form = $(":text[name='form[phone]']").parents("form");
        var phone = form.find(":text[name='form[phone]']").val();
        form.find(":text[name='form[phone]']").css({ "border-color": "#bbb" });
        form.find('.formTip').html('');
        form.find(":text[name='form[phone]']").attr("title", '');
        if (!Core.rule.common('*', phone)) {
            Core.Easyui.box["Repeat"] = true;
            form.find('.formTip').html('请输入客户电话!'); return false;
        }
        $.getJSON("http://sales.new4s.com/chance/ajax/repeatphone?phone=" + phone + "&r=" + Math.random(), function (data) {
            Core.Easyui.box["Repeat"] = true;
            try {
                if (data['repeat'] == '1') return;
                form.find('.formTip').html(data['info']);
                form.find(":text[name='form[phone]']").css({ "border-color": "#f76120" });
                form.find(":text[name='form[phone]']").attr("title", data['info']);
                var t = ($.inArray(data['state'], ["31", "32", "33"]) != -1 ? '激活潜客数据' : '转移线索数据');

                if ($.inArray("", ["appointment"]) != -1 && $.inArray(data['state'], ["4", "5"]) != -1) {//正在商谈、选购本牌
                    $.messager.confirm("线索标签编辑", data['info'] + ',是否编辑线索标签?', function (r) {
                        if (!r) return;
                        dialog('450', '200', '线索标签', "http://sales.new4s.com/chance/callcenter/common/tracktag///0/3?code=" + data['code'] + "&r=" + Math.random()); return;
                    });
                } else if ($.inArray("", ["purge"]) != -1 && $.inArray(data['state'], ["1"]) != -1) {//
                    $.messager.confirm(t, data['info'] + ',是否' + t + '?', function (r) {
                        if (!r) return;
                        dialog('755', '435', t, "http://sales.new4s.com/chance/callcenter/common/activation///0/2?code=" + data['code'] + "&r=" + Math.random()); return;
                    });
                } else if ($.inArray("", ["appointment"]) != -1 && $.inArray(data['state'], ["3", "10"]) == -1) {//潜客数据
                    $.messager.confirm(t, data['info'] + ',是否' + t + '?', function (r) {
                        if (!r) return;
                        dialog('755', '480', t, "http://sales.new4s.com/chance/callcenter/common/activation///0/3?code=" + data['code'] + "&r=" + Math.random()); return;
                    });
                } else if ($.inArray("", ["other"]) == -1 && $.inArray(data['state'], ["31", "32", "33"]) != -1) {
                    $.messager.confirm('激活', '该客户电话已经存在(潜客数据库),是否激活?', function (r) {
                        if (!r) return;
                        dialog('755', '435', t, "http://sales.new4s.com/chance/callcenter/common/activation///0/2?code=" + data['code'] + "&r=" + Math.random()); return;
                    });
                }
            } catch (e) { }
        });
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
        Core.UnionCombobox('district', {
            "layer1": [$('#province'), "0", "http://sales.new4s.com/ajax/district/layer/1/0/@id", '选择省份'],
            "layer2": [$('#city'), "0", "http://sales.new4s.com/ajax/district/layer/2/@pid/@id", '选择城市'],
            "layer3": [$('#district'), "0", "http://sales.new4s.com/ajax/district/layer/3/@pid/@id", '选择地区']
        });
        Core.UnionCombobox('brand', {
            "layer1": [$("#Archive-Brand"), "12", "http://sales.new4s.com/ajax/brand/1/0/@id", '选择车辆品牌'],
            "layer2": [$("#Archive-Series"), "0", "http://sales.new4s.com/ajax/brand/3/@pid/@id", '选择汽车车系', true],
            "layer3": [$("#Archive-Spec"), "0", "http://sales.new4s.com/ajax/brand/5/@pid/@id", '请选择汽车车型', true]
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
        $(":text[name='form[phone]']").change(function () { Core.Easyui.Repeat(); });
        $(":text[name='form[phone]']").blur(function () { Core.Easyui.Repeat(); });
    });
</script>