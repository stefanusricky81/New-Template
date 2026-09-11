<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserLocation.ascx.cs" Inherits="UserControl_DropDownList_UserLocation" %>

<asp:DropDownList ID="ddlUserLocation" runat="server" Width="100" Height="20" />
<asp:RequiredFieldValidator ID="rfvUserLocation" runat="server" ControlToValidate="ddlUserLocation" ErrorMessage="Location is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>