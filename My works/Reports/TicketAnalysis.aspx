<%@ Page Title="Bit By Bit Intranet - Ticket Analysis" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="TicketAnalysis.aspx.cs" Inherits="Reports_TicketAnalysis" %>
<%@ Register TagPrefix="uc" TagName="DateToFrom" Src="~/UserControl/DateTime/ToFrom.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketDesignation" Src="~/UserControl/DropDownList/NewTicketDesignation.ascx"%>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx"%>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="dg" TagName="DataGrid" Src="~/UserControl/Grid/TicketAnalysis.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../RadControls/Skin/Calendar/Calendar.BitByBit.css" />
    <link rel="Stylesheet" type="text/css" href="../RadControls/Skin/ComboBox/ComboBox.BitByBit.css" />
    <link rel="Stylesheet" type="text/css" href="../RadControls/Skin/Grid/Grid.BitByBit.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <div class="content-header">
        <div class="header-section">
            <h1>Ticket Analysis</h1>
        </div>
    </div>
    <table>
        <tr>
            <td style="font-family:Arial;font-size:small"></td>
        </tr>
    </table>
<%--    <telerik:RadAjaxPanel ID="rapTicketAnalysis" runat="server" LoadingPanelID="ralpTicketAnalysis" EnableAJAX="true" ClientEvents-OnRequestStart="OnRequestStart">--%>
        <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
            <asp:ValidationSummary ID="vsTicketAnalysis" runat="server" CssClass="validationSummary" ValidationGroup="vgTicketAnalysis" />
            <div class="block">
                <div class="block-title">
                    <h2><strong>Search Criteria</strong></h2>
                </div>
                <div class="row">
                    <div class="form-group col-sm-1">
                        <label>Date Range</label>
                    </div>
                    <div class="form-group col-sm-5">
                            <div class="col-md-4">
                                <asp:Literal ID="litDivStartDate" runat="server" Text='<div class="input-group date">' />
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10" PlaceHolder="mm/dd/yyyy" />
                                    <asp:PlaceHolder ID="phButton" runat="server">
                                        <span class="input-group-btn">
                                            <a href="javascript:void(0)" class="btn btn-primary"><i class="fa fa-calendar-plus-o"></i></a>
                                        </span>
                                    </asp:PlaceHolder>
                                <asp:Literal ID="litDivEnd" runat="server" Text="</div>" />
                                <asp:Literal ID="litJs" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" ForeColor="Red" ErrorMessage="Start Date is required" ValidationGroup="vgTicketAnalysis" Display="None">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-1"><label>-</label></div>
                            <div class="col-md-4">
                                <asp:Literal ID="litDivEndDate" runat="server" Text='<div class="input-group date">' />
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" MaxLength="10" PlaceHolder="mm/dd/yyyy" />
                                    <asp:PlaceHolder ID="phButton2" runat="server">
                                        <span class="input-group-btn">
                                            <a href="javascript:void(0)" class="btn btn-primary"><i class="fa fa-calendar-plus-o"></i></a>
                                        </span>
                                    </asp:PlaceHolder>
                                <asp:Literal ID="litDivEnd2" runat="server" Text="</div>" />
                                <asp:Literal ID="litJs2" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" ForeColor="Red" ErrorMessage="End Date is required" ValidationGroup="vgTicketAnalysis" Display="None">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvDates" runat="server" ControlToValidate="txtEndDate" ValidationGroup="vgTicketAnalysis" ControlToCompare="txtStartDate" Operator="GreaterThanEqual" Type="Date" ErrorMessage="End Date must be greater than Start Date" Display="None"><span class="error">*</span></asp:CompareValidator>
                                <asp:CustomValidator ID="customvDates" runat="server" ControlToValidate="txtEndDate" ValidationGroup="vgTicketAnalysis" OnServerValidate="customvDates_ServerValidate" ErrorMessage="Maximum Date Range is 365 days" Display="None"><span class="error">*</span></asp:CustomValidator>
                            </div>
                    </div>
                     <div class="form-group col-sm-1">
                         <label>Designation</label>
                    </div>
                    <div class="form-group col-sm-4">
                        <ddl:TicketDesignation ID="ddlTicketDesignationDDL" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-1">
                        <label>Client</label>
                    </div>
                    <div class="form-group col-sm-5">
                        <ddl:Client ID="ucClientComboBox" runat="server" />
                    </div>
                    <div class="form-group col-sm-1">
                        <label>MSP Only</label>
                    </div>
                    <div class="form-group col-sm-5">
                        <asp:CheckBox ID="chkMsp" runat="server" CssClass="checkHome"/>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-sm-1">
                        <label>Ticket Type</label>
                    </div>
                    <div class="form-group col-sm-5">
                        <ddl:TicketType ID="ddlTicketType" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group form-actions" style="padding-top:20px;">
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgTicketAnalysis"><i class="hi hi-search"></i> Search</asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                    </div>
                </div>
            </div>

            <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
                <asp:Literal ID="litDebug" runat="server" />
                <asp:Literal ID="litMessage" runat="server" />
                <div class="block" style="padding-bottom: 20px;">
                    <div class="block-title">
                        <h2><strong>Search Results</strong></h2>
                    </div>
                    <dg:DataGrid ID="ucTicketAnalysisGrid" runat="server" />
                </div>
            </asp:PlaceHolder>
        </asp:Panel>
 <%--   </telerik:RadAjaxPanel>--%>
</asp:Content>

