<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logdetail.aspx.cs" Inherits="ComOpp.BackAdmin.system.logdetail" %>

<div>
    <%= CurrentLogEntry == null ? string.Empty : CurrentLogEntry.Message%>
</div>
