<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FrequentCommentType.ascx.cs" Inherits="UserControl_DropDownList_FrequentCommentType" %>
<asp:DropDownList ID="ddlFrequentCommentType" runat="server" />
<asp:RequiredFieldValidator ID="rfvFrequentCommentType" ControlToValidate="ddlFrequentCommentType" ErrorMessage="Frequent Comment Type is required" runat="server" Display="None" />
<asp:Literal ID="litJs" runat="server" Visible="false"/>