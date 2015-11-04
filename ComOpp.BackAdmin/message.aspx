<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="message.aspx.cs" Inherits="ComOpp.BackAdmin.message" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统提示</title>
    <link rel="stylesheet" type="text/css" href="css/mycommon.css" />
    <script type="text/javascript" src="/js/jquery-1.8.2.min.js"></script>
    <script type="text/javascript" src="/js/Validform_v5.3.js"></script>
    <script type="text/javascript">
        var id = 'num';
        var n = 3;
        var resurl = "<%=ResolveClientUrl(ReUrl) %>";
        function showNum() {
            var idt = setTimeout(showNum, 1000);
            if (n < 0) {
                clearTimeout(idt);
                window.location.href = resurl;
                return;
            }
            var objTemp = document.getElementById(id);
            objTemp.innerHTML = n;
            n = n - 1;
        }

        $(function () {
            showNum();
        });
    </script>
    <style type="text/css">
        body
        {
            margin: 0;
            background: #fff;
            font-size: 12px;
            margin: 5px 15px;
        }
        h2
        {
            border-bottom: 2px solid #DBDFE0;
            font-size: 14px;
            height: 25px;
            line-height: 25px;
            margin-top: 3px;
            color: #2bb8aa;
            margin-bottom: 5px;
        }
        .timeout
        {
            margin-right: 2px;
        }
        .xtts
        {
            border-top: 5px solid #2BB8AA;
            border-bottom: 5px solid #2BB8AA;
            border-left: 0;
            border-right: 0;
            padding: 15px;
            margin: 5px 0;
            line-height: 30px;
            text-align: center;
        }
        .xtts span.lv_lj a
        {
            text-decoration: underline;
        }
        .dalv
        {
            font-size: 16px;
            font-weight: 700;
            color: #2BB8AA;
            line-height: 50px;
        }
    </style>
</head>
<body>
    <h2>
        <asp:Literal ID="lTitle" runat="server"></asp:Literal></h2>
    <div class="xtts">
        <span class="dalv">
            <asp:Literal ID="lContent" runat="server"></asp:Literal></span><br />
        <span>页面将在<em id='num'>3</em>秒后跳转...</span> <span>如果页面没有自动跳转，请点击<asp:HyperLink ID="hyreturn" style="color:#2BB8AA;"
            runat="server">这里</asp:HyperLink>
        </span>
    </div>
</body>
</html>
