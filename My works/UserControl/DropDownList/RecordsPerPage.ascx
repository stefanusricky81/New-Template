<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecordsPerPage.ascx.cs" Inherits="UserControl_DropDownList_RecordsPerPage" %>

 <asp:DropDownList ID="ddlRecordsPerPage" runat="server" AutoPostBack="true" Width="50">
    <asp:ListItem Text="10" Value="10"></asp:ListItem>
    <asp:ListItem Text="25" Value="25"></asp:ListItem>
    <asp:ListItem Text="50" Value="50"></asp:ListItem>
    <asp:ListItem Text="100" Value="100"></asp:ListItem>
    <asp:ListItem Text="250" Value="250"></asp:ListItem>
    <asp:ListItem Text="500" Value="500"></asp:ListItem>
</asp:DropDownList>