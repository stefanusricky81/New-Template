<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientContact.ascx.cs" Inherits="UserControl_DropDownList_ClientContact" %>

<asp:DropDownList ID="ddlClientContact" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvClientContact" runat="server" ControlToValidate="ddlClientContact"
    ErrorMessage="Client Contact is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>