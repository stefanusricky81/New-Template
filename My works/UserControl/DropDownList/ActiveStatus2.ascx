<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ActiveStatus2.ascx.cs" Inherits="UserControl_DropDownList_ActiveStatus2" %>
<asp:DropDownList ID="ddlActiveStatus" runat="server" RenderMode="Native">
    <asp:ListItem Text="All" Value=""></asp:ListItem>
    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
</asp:DropDownList>
<asp:RequiredFieldValidator ID="rfvActiveStatus" runat="server" ControlToValidate="ddlActiveStatus" ErrorMessage="Active Status is required" Display="None" />
<asp:Literal ID="litJs" runat="server"/>