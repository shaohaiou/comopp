<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getcarseriesbybrand.aspx.cs" Inherits="ComOpp.BackAdmin.ajax.getcarseriesbybrand" %>
<asp:Repeater runat="server" ID="rptData">
<ItemTemplate>
<option value="<%#Eval("ID") %>"><%#Eval("Name")%></option>
</ItemTemplate>
</asp:Repeater>
