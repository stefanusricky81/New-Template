<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Employee.ascx.cs" Inherits="UserControl_DropDownList_Employee" %>
<asp:DropDownList ID="ddlEmployee" runat="server" />
<asp:RequiredFieldValidator ID="rfvEmployee" runat="server" ControlToValidate="ddlEmployee" ErrorMessage="Employee is required" Visible="false">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />