<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectTask.ascx.cs" Inherits="UserControl_DropDownList_ProjectTask" %>
<asp:DropDownList ID="ddlProjectTask" runat="server" />
<asp:RequiredFieldValidator ID="rfvProjectTask" runat="server" ControlToValidate="ddlProjectTask" ErrorMessage="Project is required" Visible="false">
    <asp:Literal ID="litError" runat="server" Text="<span class='error'>*</span>" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />