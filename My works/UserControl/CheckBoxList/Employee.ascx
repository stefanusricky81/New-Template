<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Employee.ascx.cs" Inherits="UserControl_CheckBoxList_Employee" %>

<asp:CheckBoxList ID="cblEmployee" runat="server" RepeatColumns="5" Width="100%"
    RepeatLayout="Table" RepeatDirection="Vertical" CssClass="chkbox">
</asp:CheckBoxList>
<asp:Literal ID="litDebug" runat="server" />