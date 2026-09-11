<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BackupType.ascx.cs" Inherits="UserControl_DropDownList_BackupType" %>
<asp:DropDownList ID="ddlStatus" runat="server" Width="100" Height="20" AutoPostBack="false" />

<asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
    ErrorMessage="Backup Type is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>

<asp:Literal ID="litJs" runat="server" Visible="false"/>