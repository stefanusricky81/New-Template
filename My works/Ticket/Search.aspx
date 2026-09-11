<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Ticket_Search" %>
<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Employee" Src="~/UserControl/DropDownList/Employee.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketStatus" Src="~/UserControl/DropDownList/TicketStatusEnumResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketDisposition" Src="~/UserControl/DropDownList/TicketDispositionResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketPriority" Src="~/UserControl/DropDownList/TicketPriority.ascx" %>
<%--<%@ Register TagPrefix="ddl" TagName="TicketTag" Src="~/UserControl/DropDownList/TicketTag.ascx" %>--%>
<%@ Register TagPrefix="ddl" TagName="TicketTag" Src="~/UserControl/ListBox/TicketTag.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Team" Src="~/UserControl/DropDownList/Team.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketView" Src="~/UserControl/DropDownList/TicketView.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCategory" Src="~/UserControl/DropDownList/TicketCategory.ascx"%>
<%@ Register TagPrefix="ddl" TagName="ClientLocation" Src="~/UserControl/DropDownList/ClientLocation.ascx"%>
<%@ Register TagPrefix="ddl" TagName="ClientContact" Src="~/UserControl/DropDownList/ClientContactResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="grid" TagName="Ticket" Src="~/UserControl/Grid/TicketResponsive.ascx" %>
<%@ Register TagPrefix="ddl" tagName="TimeDDL" src="~/UserControl/DropDownList/Time.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../RadControls/Skin/ToolTip/ToolTip.BitByBit.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">

<telerik:RadScriptBlock ID="rsbMain" runat="server">
    <script type="text/javascript">
        function OnRequestStart(e, sender) {
            var theRegexp = new RegExp("\.btnExport$|\.btnMerge$|\.btnTicketMerge$", "ig");
            if (sender.EventTarget.match(theRegexp))
                sender.EnableAjax = false;
        }
    </script>
</telerik:RadScriptBlock><!--prod not updated-->
<div class="content-header">
    <div class="header-section">
        <h1>Ticket Search</h1>
    </div>
</div>
<%--<telerik:RadAjaxPanel ID="rapTicket" runat="server" LoadingPanelID="ralpTicket" EnableAJAX="true" ClientEvents-OnRequestStart="OnRequestStart">--%>
    <div class="breadcrumb breadcrumb-top">
        <a href="Default.aspx">Legacy Search</a> | 
        <a href="List.aspx">My Views</a> | 
        <a href="Add2.aspx">Add Ticket</a>
    </div>
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:ValidationSummary ID="vsTicket" runat="server" CssClass="validationSummary" ValidationGroup="vgTicket" />
        <div class="block">
            <div class="block-title">
                <h2><strong>Views</strong></h2>
                <asp:ValidationSummary ID="vsView" runat="server" CssClass="validationSummary" ValidationGroup="vgView" />
                <asp:CustomValidator ID="cvCriteriaEntered" runat="server" ControlToValidate="txtTicketNumber" ValidateEmptyText="true" ValidationGroup="vgTicket" Display="None" 
                    ErrorMessage="Minimum of 1 search field is required" OnServerValidate="cvCriteriaEntered_ServerValidate" />
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="row topMargin5">
                        <div class="col-md-1" style="padding-right:unset">
                            <label>My Views</label>
                        </div>
                        <div class="col-md-2" style="padding-right:unset;padding-left:unset">
                            <ddl:TicketView ID="ddlTicketViewUser" runat="server" IsRequired="false" DisplayChosenScript="true" ResponsivePage="true" SetSize="false"/>
                        </div>
                        <div class="col-md-2" style="padding-right:unset;width:10%">
                            <label>Public Views</label>
                        </div>
                        <div class="col-md-2" style="padding-right:unset;padding-left:unset">
                            <ddl:TicketView ID="ddlTicketViewPublic" runat="server" IsRequired="false" DisplayChosenScript="true" ResponsivePage="true" ViewType="Public" SetSize="false"/>
                        </div>
                        <div class="col-md-2" style="padding-right:unset;width:10%">
                             <label>Save View</label>
                        </div>
                        <div class="col-md-2" style="padding-right:unset;padding-left:unset">
                            <div class="col-sm-2" style="padding-left:unset">
                                <asp:CheckBox ID="chkSaveView" runat="server" CssClass="form-control-borderless" />
                            </div>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txtViewName" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter View Name ...." />
                                <asp:CustomValidator ID="cvView" runat="server" ValidateEmptyText="true" ControlToValidate="txtViewName" ValidationGroup="vgTicket" OnServerValidate="cvView_ServerValidate" Display="None"/>
                            </div>
                        </div>
                        <div class="col-md-2" style="padding-right:unset;width:10%">
                            <label>Public View</label>
                        </div>
                        <div class="col-md-1" style="padding-right:unset">
                            <asp:CheckBox ID="chkIsPublicView" runat="server" CssClass="form-control-borderless" />
                        </div>
                    </div>
                    
                    <div class="row topMargin5">
                        <asp:PlaceHolder ID="phDeleteView" runat="server" Visible="false">
                            <div class="form-group col-md-1 col-sm-6">
                                <asp:LinkButton ID="btnDeleteView" runat="server" OnClick="btnDeleteView_Click" Text="Delete" CausesValidation="false" OnClientClick="if (!confirm('Delete View?')) return false;" 
                                    CssClass="btn btn-sm btn-danger" style="margin-top:10px;"><i class="hi hi-remove"></i> Delete</asp:LinkButton>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>
        <div class="block">
            <div class="block-title">
                <h2><strong>Search Criteria</strong></h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Ticket Number</label>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtTicketNumber" runat="server" MaxLength="10" CssClass="form-control" PlaceHolder="Enter Ticket Number" />
                            <asp:RangeValidator ID="rvTicketNumber" runat="server" ControlToValidate="txtTicketNumber" Type="Integer" MinimumValue="1" MaximumValue="1000000" ErrorMessage="Invalid Ticket Number" ValidationGroup="vtTicket" Display="None" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Client</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <%--<ddl:Client ID="ddlClient" runat="server" IsRequired="false" DisplayChosenScript="true" />--%>
                            <ddl:SmartClient ID="ddlSmartClient" runat="server" Width="100%" />
                        </div>
                    </div>
                    
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Created Start</label>
                            </div>                            
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <uc:DatePicker ID="ucCreatedStart" runat="server" IsRequired="false" ValidationGroup="vgTicket"  />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>End</label>
                            </div>                            
                        </div>
                        <div class="col-md-3" style="padding-left:unset"> 
                            <uc:DatePicker ID="ucCreatedEnd" runat="server" IsRequired="false" ValidationGroup="vgTicket"  />
                            <asp:CustomValidator ID="cvCreatedDates" runat="server" ControlToValidate="txtTicketNumber" ValidateEmptyText="true" ValidationGroup="vgTicket" 
                            ErrorMessage="Created End Date must be greater than Created Start Date" Display="None" OnServerValidate="cvCreatedDates_ServerValidate" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Last Updated Start</label>
                            </div>                            
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <uc:DatePicker ID="ucLastUpdatedStart" runat="server" IsRequired="false" />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>End</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-left:unset">
                            <uc:DatePicker ID="ucLastUpdatedEnd" runat="server" IsRequired="false" />
                            <asp:CustomValidator ID="cvLastUpdatedDates" runat="server" ControlToValidate="txtTicketNumber" ValidateEmptyText="true" ValidationGroup="vgTicket" 
                                ErrorMessage="Last Updated End Date must be greater than Last Updated Start Date" Display="None" OnServerValidate="cvLastUpdatedDates_ServerValidate" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Status</label>
                            </div>                                
                        </div>
                        <div class="col-md-8">
                            <ddl:TicketStatus ID="ddlTicketStatus" runat="server" IsRequired="false" DisplayChosenScript="true" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Priority</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <ddl:TicketPriority ID="ddlTicketPriority" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultText="" SetSize="false" />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>Type</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-left:unset">
                            <ddl:TicketType ID="ddlTicketType" runat="server" CssClass="dropdown" IsRequired="false" DefaultValue="" DefaultText="" DisplayDefaultValue="true" SetSize="false" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Client Awaiting Response</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <asp:CheckBox ID="chkAwaitingResponse" runat="server" CssClass="form-control-borderless" />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>Category</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-left:unset">
                            <ddl:TicketCategory ID="ddlTicketCategory" runat="server" IsRequired="false" ValidationGroup="vgTicket" DisplayChosenScript="true" DisplayDefaultValue="true"  DefaultText=" " DefaultValue="" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Exclude Helpdesk</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <asp:CheckBox ID="cbExcludeHelpdesk" runat="server" Checked="true" CssClass="form-control-borderless" />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>Exclude Views</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <asp:CheckBox ID="cbExcludeViews" runat="server" CssClass="form-control-borderless" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>VIP</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <asp:CheckBox ID="cbVIP" runat="server" CssClass="form-control-borderless" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Time spent</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:5px">
                            <ddl:TimeDDL ID="ddlTimeSpent" runat="server" StartHourAtZero="true" ThirtyMinuteIncrements="true" DisplayDefaultValue="false" />
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Description</label>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Description" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Assigned To</label>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <ddl:Employee ID="ddlEmployeeAssignedTo" runat="server" DisplayChosenScript="true" DefaultValue="" SetSize="false" />
                        </div>
                    </div>
                    
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Created By</label>
                            </div>                            
                        </div>
                        <div class="col-md-3" style="padding-right:unset">
                            <ddl:Employee ID="ddlEmployeeCreatedBy" runat="server" DisplayChosenScript="true" DefaultValue="" SetSize="false" />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>Last Days</label>
                            </div>                            
                        </div>
                        <div class="col-md-3" style="padding-left:unset">
                            <asp:TextBox ID="txtCreatedLastDays" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Created Last Days" />
                            <asp:RangeValidator ID="rvCreatedLastDays" runat="server" Type="Integer" ControlToValidate="txtCreatedLastDays" MinimumValue="0" MaximumValue="500" 
                                ErrorMessage="Invalid Created Last Days" ValidationGroup="vgTicket" Display="None" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Updated By</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-right:unset">
                            <ddl:Employee ID="ddlEmployeeUpdatedBy" runat="server" DisplayChosenScript="true" DefaultValue="" SetSize="false" />
                        </div>
                        <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                            <div style="float:right">
                                <label>Last Days</label>
                            </div>
                        </div>
                        <div class="col-md-3" style="padding-left:unset">
                            <asp:TextBox ID="txtUpdatedLastDays" runat="server" MaxLength="50" CssClass="form-control" PlaceHolder="Enter Updated Last Days" />
                            <asp:RangeValidator ID="rvUpdatedLastDays" runat="server" Type="Integer" ControlToValidate="txtUpdatedLastDays" MinimumValue="0" MaximumValue="500" 
                                ErrorMessage="Invalid Updated Last Days" ValidationGroup="vgTicket" Display="None" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Client Location</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <ddl:ClientLocation ID="ddlClientLocation" DisplayChosenScript="true" IsRequired="false" runat="server" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Reported by</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <ddl:ClientContact ID="ddlReportedBy" runat="server" DisplayChosenScript="true"  />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Client Ticket Contact</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <ddl:ClientContact ID="ddlClientContact" runat="server" DisplayChosenScript="true"  />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Disposition</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <ddl:TicketDisposition ID="ddlTicketDisposition" runat="server" IsRequired="false" DisplayChosenScript="true" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Tag</label>
                            </div>                            
                        </div>
                        <div class="col-md-8">
                            <ddl:TicketTag ID="ddlTicketTag" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" DefaultText="" DisplayDefaultValue="true" />
                        </div>
                    </div>
                    <div class="row topMargin5" style="margin-left:-35px">
                        <div class="col-md-4">
                            <div style="float:right">
                                <label>Team</label>
                            </div>                            
                        </div>
                        <div class="col-md-3" style="padding-right:unset">
                            <ddl:Team ID="ddlTeam" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" DefaultText="" DisplayDefaultValue="true" SetSize="false" />
                        </div>
                         <div class="col-md-2" style="padding-left:unset;padding-right:5px">
                             <div style="float:right">
                                 <label>Sales</label>
                             </div>                            
                        </div>
                        <div class="col-md-3" style="padding-left:unset">
                            <ddl:Employee ID="ddlEmployeeSales" runat="server" DisplayChosenScript="true" DefaultValue="" SetSize="false" UseEmployeeIdAsDataValue="true" EmployeeTypeToDisplay="Sales" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                    <div class="form-group form-actions" style="margin-left:8px">
                        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" ValidationGroup="vgTicket"><i class="hi hi-search"></i> Search</asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-sm btn-warning" OnClick="btnClear_Click" CausesValidation="false"><i class="hi hi-repeat"></i> Clear</asp:LinkButton>
                    </div>
                </div>  
        </div>
        <asp:PlaceHolder ID="phSearchResults" runat="server" Visible="false">
            <asp:Literal ID="litDebug" runat="server" />
            <asp:Literal ID="litMessage" runat="server" />
            <div class="block" style="padding-bottom: 20px; overflow-x:scroll">
                <div class="block-title">
                    <h2><strong>Search Results</strong></h2>
                    <a href="#modal-search-help" data-toggle="modal"><i class="gi gi-circle_question_mark" title="View Help Search Results" style="padding-bottom:8px;"></i></a>
                    <asp:PlaceHolder ID="phScrollNotes" runat="server"></asp:PlaceHolder>
                </div>
                 <div class="table-responsive scroll">
                     <grid:Ticket ID="gridTicket" runat="server" MergeTicketsActive="true" />
                 </div>
                
            </div>
        </asp:PlaceHolder>
    </asp:Panel>
    <div id="modal-search-help" class="modal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Search Results Help</h3>
                </div>
                <div class="modal-body">
                    Green Arrow - Make Ticket Active<br />
                    Blue BG # - Awaiting Response. Last update by Client.<br />
                    Red BG # - Awaiting Response. Last update by Bbb.<br />
                    "-" sign - Clear Awaiting Response Flag<br />
                    P # - Ticket Priority 0 = Top Priority 5=Low Priority<br />
                    Yellow star - Priority Client<br />
                    Red star - Helpdesk Client<br /><br />
                    Mouse Overs<br />
                    Summary - Display summary and description<br />
                    Status - Display status and last note entered<br />
                    Description - display entire description<br />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
<%--</telerik:RadAjaxPanel>--%>
<telerik:RadAjaxLoadingPanel ID="ralpTicket" runat="server" Transparency="70" BackColor="#69b899">
    <i class="fa fa-spinner fa-4x fa-spin"></i>
</telerik:RadAjaxLoadingPanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphEnd" Runat="Server"></asp:Content>
