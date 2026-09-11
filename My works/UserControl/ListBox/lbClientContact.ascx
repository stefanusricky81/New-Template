<%@ Control Language="C#" AutoEventWireup="true" CodeFile="lbClientContact.ascx.cs" Inherits="UserControl_ListBox_lbClientContact" %>
<asp:ListBox ID="lbClientContact" runat="server" SelectionMode="Multiple"/>
<asp:RequiredFieldValidator ID="rfvClientContact" runat="server" ControlToValidate="lbClientContact" ErrorMessage="Client Contact is required" Visible="false">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>