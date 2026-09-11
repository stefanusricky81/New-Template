<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateAndTime.ascx.cs" Inherits="UserControl_DateTime_DateAndTime" %>

<telerik:RadDatePicker id="rdpMain" Runat="server" SharedCalendarID="sharedCalendar" Width="150px" EnableEmbeddedSkins="false" Skin="BitByBit" OnChildrenCreated="rdpMain_ChildrenCreated"></telerik:RadDatePicker>
<uc:TimeDDL ID="ucTime" runat="server" />
<asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="rdpMain" ErrorMessage="Date is required" Visible="false">
    <span class="error">*</span>
</asp:RequiredFieldValidator>
<asp:CustomValidator ID="cvTime" runat="server" ControlToValidate="rdpMain" ErrorMessage="Time is required" OnServerValidate="cvTime_ServerValidate" Visible="false">
    <span class="error">*</span>
</asp:CustomValidator>
<telerik:RadCalendar ID="sharedCalendar" runat="server" EnableMultiSelect="false" EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
    <FastNavigationSettings EnableTodayButtonSelection="true" />
</telerik:RadCalendar>
  