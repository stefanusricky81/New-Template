<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientEmailCategory.ascx.cs" Inherits="UserControl_DropDownList_ClientEmailCategory" %>

<asp:DropDownList ID="ddlClientEmailCategory" runat="server" />
<asp:RequiredFieldValidator ID="rfvClientEmailCategory" runat="server" ControlToValidate="ddlClientEmailCategory" ErrorMessage="Client Email Category is required" Visible="false" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>