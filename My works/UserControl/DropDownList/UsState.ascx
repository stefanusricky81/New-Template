<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UsState.ascx.cs" Inherits="UserControl_DropDownList_UsState" %>
<asp:DropDownList ID="ddlUsState" runat="server" />
<asp:RequiredFieldValidator ID="rfvUsState" ControlToValidate="ddlUsState" ErrorMessage="State is required" ForeColor="red" runat="server" Display="Dynamic">
    <asp:Literal ID="litError" runat="server" Text="*" />
</asp:RequiredFieldValidator>
<asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />