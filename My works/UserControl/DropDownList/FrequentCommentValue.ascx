<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FrequentCommentValue.ascx.cs" Inherits="UserControl_DropDownList_FrequentCommentValue" %>
<asp:DropDownList ID="ddlFrequentCommentValue" runat="server" />
<asp:RequiredFieldValidator ID="rfvFrequentCommentValue" ControlToValidate="ddlFrequentComment" ErrorMessage="Frequent Comment  is required" runat="server" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>