<%@ Page Language="C#" MasterPageFile="~/Template/Base.master" AutoEventWireup="true" CodeFile="ClientEmailBroadcast.aspx.cs" Inherits="Reports_ClientEmailBroadcast" Title="" %>
<%@ Register TagPrefix="uc" TagName="ClientCompany" Src="~/UserControl/ComboBox/Company.ascx" %>
<%@ Register TagPrefix="uc" TagName="Product" Src="~/UserControl/ComboBox/Product.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmailBroadcastLogGrid" Src="~/UserControl/Grid/ClientEmailBroadcastLog.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server" />

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <style type="text/css">
        /*
        .multipleRowsColumns .rcbItem, 
        .multipleRowsColumns .rcbHovered
        {
            float: left;
            margin: 0 1px;
            min-height: 13px;
            overflow: hidden;
            padding: 2px 0 2px 0;
            width: 220px;
        }
        label
        {
            display: inline;
            width: 250px;            
            padding-right: 5px;
            margin: 0;
        }
        input
        {
            display: inline;
            width: 10px;
            padding-right: 5px;
            margin: 0;
        }
        */
    </style>
<asp:Panel ID="pnlSearch" runat="server">
        <h2>Client Email Broadcast Transaction Report</h2>
        
        <table class="searchForm" border="0">
            <tr>
                <td colspan="4">
                    <p style="text-align: center; padding-bottom: 2px;">
                        <b>SEARCH CRITERIA</b>
                    </p>
                    <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="ValidationSummary" ValidationGroup="vgSearch" />
                </td>
            </tr>
            <tr>
                <th style="width:15%;">Company:</th>
                <td style="width:85%;">
                    <uc:ClientCompany ID="ucClientCompanyComboBox" runat="server" />
                    <asp:Button ID="btnSelectCompany" Width="60" runat="server" Text="Select" OnClick="btnSelectCompany_Click" />
                </td>
            </tr>
            <tr>
                <th style="width:15%;"></th>
                <td style="width:80%;">
                    <asp:Label ID="lblSelectedCompanies" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width:15%;">Product:</th>
                <td style="width:25%;">
                    <uc:Product ID="ucClientProductComboBox" runat="server" />    
                </td>
                <td style="width:60%;">
                </td>
            </tr>
            <tr>
                <th style="width:12%;">Start:</th>
                <td style="width:88%;" colspan="3">
                    <telerik:RadDatePicker id="rdpStartDate" Runat="server" SharedCalendarID="sharedCalendarStartDate" 
                        Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
                    </telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Start Date is required" 
                        ControlToValidate="rdpStartDate" ValidationGroup="vgSearch">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <telerik:RadCalendar ID="sharedCalendarStartDate" runat="server" EnableMultiSelect="false" 
                        EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                    </telerik:RadCalendar> 
                    End: 
                    <telerik:RadDatePicker id="rdpEndDate" Runat="server" SharedCalendarID="sharedCalendarEndDate" 
                        Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
                    </telerik:RadDatePicker>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="End Date is required" 
                        ControlToValidate="rdpEndDate" ValidationGroup="vgSearch">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <telerik:RadCalendar ID="sharedCalendarEndDate" runat="server" EnableMultiSelect="false" 
                        EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                    </telerik:RadCalendar>  
                    <asp:CompareValidator ID="cvDates" runat="server"
                                ControlToValidate="rdpEndDate" ControlToCompare="rdpStartDate" 
                                Operator="GreaterThanEqual" Type="Date" ValidationGroup="vgSearch" 
                                ErrorMessage="End Date must be greater than or equal to Start Date">
                                <span class="error">*</span>
                    </asp:CompareValidator>                                
                </td>            
            </tr>
            
            <tr><td colspan="4">&nbsp;</td></tr>
            <tr>
                <td colspan="4" style="text-align:center;">
                    <asp:Button ID="btnSearch" Text="SEARCH" runat="server" CssClass="actionbutton" 
                        OnClick="btnSearch_Click" ValidationGroup="vgSearch"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btnClearSearch" Text="CLEAR" runat="server" CssClass="actionbutton2"
                        OnClick="btnClearSearch_Click" CausesValidation="false"></asp:Button>
                </td>
            </tr>
        </table>
        
        <br /><br />
        
    </asp:Panel>

    <asp:Panel ID="pnlGrid" runat="server">
        <uc:EmailBroadcastLogGrid ID="ucEmailBroadcastLogGrid" runat="server" Visible="true" />
    </asp:Panel>

</asp:Content>
