<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="noticeedit.aspx.cs" ValidateRequest="false" Inherits="ComOpp.BackAdmin.support.noticeedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>话术管理-红旭集团销售客户管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
    <link rel="stylesheet" type="text/css" href="../umeditor1_2_2/themes/default/css/umeditor.min.css" />
    <script type="text/javascript" src="../umeditor1_2_2/third-party/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../umeditor1_2_2/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../umeditor1_2_2/umeditor.min.js"></script>
    <script type="text/javascript" src="../umeditor1_2_2/lang/zh-cn/zh-cn.js"></script>
    <style type="text/css">
        span.tag
        {
            border: 1px solid #e2e2e2;
            line-height: 24px;
            display: inline-block;
            margin-top: 1px;
            padding-left: 5px;
            padding-right: 5px;
            cursor: pointer;
        }
        span.tag:hover
        {
            border: 1px solid #A3BDE3;
            background-color: #D5E1F2;
        }
    </style>
</head>
<body>
    <div class="Tab">
        <a href="talkmg.aspx">话术列表</a> <a class="selected">话术编辑</a>
    </div>
    <div>
        <form runat="server" onsubmit="return Act.Form(this);">
        <ul class="tabs_box_one">
            <li>
                <label class="llabel">
                    标题：</label><input runat="server" id="txtTitle" value="" class="traceInp textbox"
                        style="width: 500px !important" type="text" />
                <span class="Validform_checktip ml10"></span></li>
            <li>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="110" valign="top">
                            <label class="llabel">
                                内容：</label>
                        </td>
                        <td>
                            <script type="text/plain" id="editor" style="width:800px;height:260px;"><%=CurrentNotice == null ? "" : CurrentNotice.Content%></script>
                        </td>
                    </tr>
                </table>
            </li>
        </ul>
        <div style="margin-top: 10px; padding-left: 110px">
            <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" CssClass="btn" Text="保存设置" />
            <input type="reset" value="清除重置" class="btn">
        </div>
        </form>
    </div>
</body>
<script type="text/javascript">
    var serverPath = '/att/',

    um = UM.getEditor('editor', {
        imagePath: serverPath,
        textarea: 'txtContent',
        focus: true
    });
    var Act = {
        Form: function (form) {
            var form = $(form);

            var o = $("#txtTitle");
            o.next("span").removeClass('Validform_wrong');
            if (o.val() == "") {
                o.next("span").html('<i></i>请输入标题');
                o.next("span").addClass('Validform_wrong');
                o.focus()
                return false;
            }
            if (!UM.getEditor('editor').hasContents()) {
                alert("请输入内容!");
                return false;
            }
        }
    }
</script>
</html>
