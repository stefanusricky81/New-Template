<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalesPerson.ascx.cs" Inherits="UserControl_DropDownList_SalesPerson" %>

<asp:DropDownList ID="ddlSalesPerson" runat="server" Width="100" Height="20" />
<asp:RequiredFieldValidator ID="rfvSalesPerson" runat="server" ControlToValidate="ddlSalesPerson" ErrorMessage="Sales Person is required" Visible="false" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>