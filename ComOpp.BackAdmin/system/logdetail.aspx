<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logdetail.aspx.cs" Inherits="ComOpp.BackAdmin.system.logdetail" %>

<div style="padding: 0 15px;">
    <%= CurrentLogEntry == null ? string.Empty : CurrentLogEntry.Message.Replace("\r\n","<br />")%>
</div>
