<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ToFrom.ascx.cs" Inherits="UserControl_DateTime_ToFrom" %>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
             <telerik:RadDatePicker id="rdpStart" Runat="server" SharedCalendarID="sharedCalendar" 
                Width="130px" EnableEmbeddedSkins="false" Skin="BitByBit" 
                OnChildrenCreated="rdp_ChildrenCreated">
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="rdpStart"
                ErrorMessage="Start Date is required">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
        </td>
        <td>&nbsp;-&nbsp;</td>
        <td>
            <telerik:RadDatePicker id="rdpEnd" Runat="server" SharedCalendarID="sharedCalendar" 
                Width="130px" EnableEmbeddedSkins="false" Skin="BitByBit"
                OnChildrenCreated="rdp_ChildrenCreated">
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="rdpEnd"
                ErrorMessage="End Date is required">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvDates" runat="server"
                ControlToValidate="rdpEnd" ControlToCompare="rdpStart" 
                Operator="GreaterThanEqual" Type="Date"
                ErrorMessage="End Date must be greater than Start Date">
                <span class="error">*</span>
            </asp:CompareValidator>
            <asp:CustomValidator ID="customvDates" runat="server" ControlToValidate="rdpEnd" 
                OnServerValidate="customvDates_ServerValidate" Visible="false" ErrorMessage="Maximum Date Range is 31 days">
                <span class="error">*</span>
            </asp:CustomValidator>
        </td>
    </tr>
</table>

<telerik:RadCalendar ID="sharedCalendar" runat="server" EnableMultiSelect="false" EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
    <FastNavigationSettings EnableTodayButtonSelection="true" />
</telerik:RadCalendar>