<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Days.ascx.cs" Inherits="UserControl_ListBox_Days" %>
<asp:ListBox ID="lbDays" runat="server" SelectionMode="Multiple"/>
<asp:RequiredFieldValidator ID="rfvDays" runat="server" ControlToValidate="lbDays" ErrorMessage="Days is required" Visible="false">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>