<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tier.ascx.cs" Inherits="UserControl_DropDownList_Tier" %>
<asp:DropDownList ID="ddlTier" runat="server" Width="100" Height="20" />

<asp:RequiredFieldValidator ID="rfvTier" runat="server" ControlToValidate="ddlTier"
    ErrorMessage="Tier is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>