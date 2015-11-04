<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getpowergroupbycorp.aspx.cs" Inherits="ComOpp.BackAdmin.ajax.getpowergroupbycorp" %>
<asp:Repeater runat="server" ID="rptData">
<ItemTemplate>
<option value="<%#Eval("ID") %>"><%#Eval("GroupName")%></option>
</ItemTemplate>
</asp:Repeater>