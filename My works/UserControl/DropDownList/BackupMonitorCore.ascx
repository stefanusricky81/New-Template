<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BackupMonitorCore.ascx.cs" Inherits="UserControl_DropDownList_BackupMonitorCore" %>
<asp:DropDownList ID="ddlCore" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvCore" runat="server" ControlToValidate="ddlCore" ErrorMessage="Core is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>