<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BackupServer.ascx.cs" Inherits="UserControl_DropDownList_BackupServer" %>
<asp:DropDownList ID="ddlBackupServer" runat="server" Width="100" Height="20" AutoPostBack="false" />

<asp:RequiredFieldValidator ID="rfvBackupServer" runat="server" ControlToValidate="ddlBackupServer" ErrorMessage="Backup Server is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>

<asp:Literal ID="litJs" runat="server" Visible="false"/>