<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimesheetTime.ascx.cs" Inherits="UserControl_DropDownList_TimesheetTime" %>

<div class="row">
    <div class="col-xs-6" style="padding-right:0;">
        <asp:PlaceHolder ID="phHourHeader" runat="server"><label><asp:Literal ID="litHourHeader" runat="server" Text="Hours" /></label></asp:PlaceHolder>
        <asp:DropDownList ID="ddlHour" runat="server" />
    </div>
    <div class="col-xs-6" style="padding-left:0;">
        <asp:PlaceHolder ID="phMinuteHeader" runat="server"><label><asp:Literal ID="litMinuteHeader" runat="server" Text="Mins" /></label></asp:PlaceHolder>
        <asp:DropDownList ID="ddlMinute" runat="server" />
    </div>
</div>
<asp:RequiredFieldValidator ID="rfvHour" runat="server" ControlToValidate="ddlHour" ErrorMessage="Hour is required" Visible="false" Display="None" />
<asp:RequiredFieldValidator ID="rfvMinute" runat="server" ControlToValidate="ddlMinute" rrorMessage="Minute is required" Visible="false" Display="None" /><asp:Literal ID="litJs" runat="server" Visible="false"/>
<asp:Literal ID="litDebug" runat="server" />