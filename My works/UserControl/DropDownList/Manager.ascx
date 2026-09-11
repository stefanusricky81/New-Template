<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Manager.ascx.cs" Inherits="UserControl_DropDownList_Manager" %>
<asp:DropDownList ID="ddlManager" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvManager" runat="server" ControlToValidate="ddlManager"
    ErrorMessage="Manager is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>