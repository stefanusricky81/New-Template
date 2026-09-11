<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Team.ascx.cs" Inherits="UserControl_DropDownList_Team" %>
<asp:DropDownList ID="ddlTeam" runat="server"/>
<asp:RequiredFieldValidator ID="rfvTeam" runat="server" ControlToValidate="ddlTeam" ErrorMessage="Team is required" Visible="false">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>