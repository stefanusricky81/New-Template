<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Employee.ascx.cs" Inherits="UserControl_ListBox_Employee" %>
<asp:ListBox ID="lbEmployee" runat="server" SelectionMode="Multiple"/>
<asp:RequiredFieldValidator ID="rfvEmployee" runat="server" ControlToValidate="lbEmployee" ErrorMessage="Employee is required" Visible="false">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>