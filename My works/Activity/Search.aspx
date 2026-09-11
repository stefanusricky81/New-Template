    <%@ Page Title="Bit By Bit Intranet - Activity History" Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Activity_Search" %>
<%@ Register TagPrefix="grid" TagName="SearchActivity" Src="~/UserControl/Grid/SearchActivity.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">

<h2>Activity History</h2>
 <asp:Panel ID="pnlSearchCriteria" runat="server">
    <table class="searchForm" border="0">
        <tr>
            <td colspan="4">
                <p style="text-align:center; padding-bottom:2px;"><b>SEARCH CRITERIA</b></p>
                <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="ValidationSummary" ValidationGroup="vgSearch" />
            </td>
        </tr>
        <tr>
            <th style="width:11%;">Employee:</th>
            <td style="width:39%;"><uc:EmployeeDDL ID="ddlEmployee" runat="server" UseEmployeeIdAsDataValue="true" CssClass="dropdown" Width="275" DefaultText="All"/></td>
            <th style="width:11%;">Team:</th>
            <td style="width:39%;"><uc:TeamDDL ID="ddlTeam" runat="server" Width="275" DefaultText="All" IsRequired="false" DisplayDefaultValue="true" /></td>
        </tr>
        <tr>
            <th style="width:11%;">Client:</th>
            <td style="width:39%;"><uc:ClientComboBox ID="cbClient" runat="server" Width="276"/></td>
            <th style="width:11%;">Tag:</th>
            <td style="width:39%;"><uc:UserTicketTagsDDL ID="ddlUserTicketTags" runat="server" DisplayDefaultValue="true" DefaultText="" DefaultValue="" IsRequired="false" Width="275"/></td>
        </tr>
        <tr>
            <th style="width:11%;">Date:</th>
            <td style="width:39%;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                             <telerik:RadDatePicker id="rdpStart" Runat="server" SharedCalendarID="sharedCalendar" Width="130px" EnableEmbeddedSkins="false" Skin="BitByBit" OnChildrenCreated="rdp_ChildrenCreated" />
                        </td>
                        <td>&nbsp;-&nbsp;</td>
                        <td>
                            <telerik:RadDatePicker id="rdpEnd" Runat="server" SharedCalendarID="sharedCalendar" Width="130px" EnableEmbeddedSkins="false" Skin="BitByBit" OnChildrenCreated="rdp_ChildrenCreated" />
                            <asp:CompareValidator ID="cvDates" runat="server" Visible="false" ControlToValidate="rdpEnd" ControlToCompare="rdpStart" Operator="GreaterThan" Type="Date" ErrorMessage="End Date must be greater than Start Date">
                                <span class="error">*</span>
                            </asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <telerik:RadCalendar ID="sharedCalendar" runat="server" EnableMultiSelect="false" EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                    <FastNavigationSettings EnableTodayButtonSelection="true" />
                </telerik:RadCalendar>
            </td>
            <th style="width:11%;">Clip Notes Text:</th>
            <td style="width:39%;"><asp:CheckBox ID="chkClipNotes" runat="server" /></td>
        </tr>
         <tr>
            <th style="width:11%;">&nbsp;</th>
            <td style="width:39%;">&nbsp;</td>
            <th style="width:11%;">Include User Check Ins:</th>
            <td style="width:39%;"><asp:CheckBox ID="chkIncludeUserCheckIns" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="4" style="text-align:center;">
                <asp:button id="btnSearch" text="SEARCH" runat="server" CssClass="actionbutton" OnClick="btnSearch_Click" ValidationGroup="vgSearch" />&nbsp;
                <asp:Button ID="bntClearSearchSetDate" Text="VIEW TODAY" runat="server" CssClass="actionbutton" OnClick="btnClearSearch_Click" CausesValidation="false" CommandName="SetDate" />&nbsp;
                <asp:button id="btnClearSearch" text="CLEAR" runat="server" CssClass="actionbutton2" OnClick="btnClearSearch_Click" CausesValidation="false" />
            </td>        
        </tr>
    </table>
</asp:Panel>

<br /><br />

<asp:Panel ID="pnlGrid" runat="server">
    <grid:SearchActivity ID="gridSearchActivity" runat="server" />
</asp:Panel>

<telerik:RadAjaxManagerProxy ID="rampActivity" runat="server"> 
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="pnlSearchCriteria">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpActivity"/>
                <telerik:AjaxUpdatedControl ControlID="pnlSearchCriteria" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="pnlGrid">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="pnlGrid" LoadingPanelID="ralpActivity"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<telerik:RadAjaxLoadingPanel ID="ralpActivity" runat="server" Transparency="25" BackColor="#E0E0E0">
    <img alt="Loading..." src="/Images/loading.gif" style="border: 0px; padding-top:90px;" />
</telerik:RadAjaxLoadingPanel>

</asp:Content>

