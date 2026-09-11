<%@ Page Title="" Language="C#" MasterPageFile="~/Template/Responsive.master" AutoEventWireup="true" CodeFile="Detail2.aspx.cs" Inherits="Ticket_Detail2" %>
<%@ Register TagPrefix="ddl" TagName="TicketDisposition" Src="~/UserControl/DropDownList/TicketDispositionResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Employee" Src="~/UserControl/DropDownList/Employee.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ClientContact" Src="~/UserControl/DropDownList/ClientContactResponsive.ascx" %>
<%@ Register TagPrefix="ddl" TagName="ProjectTask" Src="~/UserControl/DropDownList/ProjectTask.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TaskAction" Src="~/UserControl/DropDownList/TaskAction.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TimesheetTime" Src="~/UserControl/DropDownList/TimesheetTime.ascx" %>
<%--<%@ Register TagPrefix="ddl" TagName="Client" Src="~/UserControl/DropDownList/Client.ascx" %>--%>
<%@ Register TagPrefix="ddl" TagName="ClientLocation" Src="~/UserControl/DropDownList/ClientLocation.ascx" %>
<%@ Register TagPrefix="ddl" TagName="Ticket" Src="~/UserControl/DropDownList/Ticket.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCheckListMaster" Src="~/UserControl/DropDownList/TicketCheckListMaster.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCategory" Src="~/UserControl/DropDownList/TicketCategoryDefault.ascx"%>
<%@ Register TagPrefix="ddl" TagName="TicketPriority" Src="~/UserControl/DropDownList/TicketPriority.ascx"%>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>
<%@ Register TagPrefix="lb" TagName="TicketTag" Src="~/UserControl/ListBox/TicketTag.ascx" %>
<%@ Register TagPrefix="lb" TagName="Ticket" Src="~/UserControl/ListBox/Ticket.ascx" %>
<%@ Register TagPrefix="lb" TagName="Employee" Src="~/UserControl/ListBox/Employee.ascx" %>
<%@ Register TagPrefix="lb" TagName="ClientContact" Src="~/UserControl/ListBox/lbClientContact.ascx" %>
<%@ Register TagPrefix="uc" TagName="DatePicker" Src="~/UserControl/DateTime/DatePicker.ascx" %>
<%@ Register TagPrefix="grid" TagName="TicketHistory" Src="~/UserControl/Grid/TicketHistoryResponsive.ascx" %>
<%@ Register TagPrefix="grid" TagName="TicketFile" Src="~/UserControl/Grid/TicketFileResponsive.ascx" %>
<%@ Register TagPrefix="grid" TagName="TicketContact" Src="~/UserControl/Grid/TicketContact.ascx" %>
<%@ Register TagPrefix="grid" TagName="TicketTag" Src="~/UserControl/Grid/TicketTagResponsive.ascx" %>
<%@ Register TagPrefix="grid" TagName="ActiveTicketHistory" Src="~/UserControl/Grid/ActiveTicketHistory.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpHeadBottom" Runat="Server">
    <script type="text/javascript" src="/js/clipboard.min.js"></script> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMainContent" Runat="Server">
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script>
        $(document).ready(function () {
            SetupControls();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            SetupControls();
        });
        function SetupControls() {
            //$("#<%=txtEditContactPhone.ClientID%>").mask('(999) 999-9999', { autoclear: false });
            //$("#<%=txtEditContactCellPhone.ClientID%>").mask('(999) 999-9999', { autoclear: false });
            //$("#<%=txtEditContactHomePhone.ClientID%>").mask('(999) 999-9999', { autoclear: false });
            var clipboard = new ClipboardJS('.copy-btn');
            clipboard.on('success', function (e) {
                console.log(e);
            });
            clipboard.on('error', function (e) {
                console.log(e);
            });
        }
        function conditionalPostback(e, sender) {
            var theRegexp = new RegExp("\.btnViewFile$|\.btnView$|\.btnSubmit$|\.btnSubmit2$|\.btnSubmit3$|\.btnUpload$", "ig");
            if (sender.EventTarget.match(theRegexp)) {
                sender.EnableAjax = false;
            }
        }
        function CloseEditClient() {
            $('#modal-edit-client').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function CloseEditContact() {
            $('#modal-edit-contact').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function CloseAddContact() {
            $('#modal-add-contact').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function CloseEditSummary() {
            $('#modal-edit-summary').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function CloseEditDescription() {
            $('#modal-edit-description').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function CloseEditLocation() {
            $('#modal-edit-location').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function CloseAddTag() {
            $('#modal-add-tag').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }
        function OnClientLoad(sender, args) {
            $telerik.$(sender.get_inputElement()).on('keypress', function (e) {
                if (sender.get_entries().get_count() > 0) {
                    // optionally, notify user that a single selection is allowed
                    e.preventDefault();
                }
            });
        }
    </script>
    
    <style>
        .checklistText label {font-weight:normal; }
    </style>
</telerik:RadCodeBlock>
<%--<telerik:RadAjaxPanel ID="rapTicket" runat="server" LoadingPanelID="ralpTicket" EnableAJAX="true" ClientEvents-OnRequestStart="conditionalPostback">--%>
    <div class="content-header">
        <div class="header-section">
            <h1><asp:Literal ID="litHeader" runat="server" Text="Ticket" /></h1>
        </div>
    </div>
    <div class="breadcrumb breadcrumb-top">
        <asp:HyperLink ID="hlLegacyDetail" runat="server" Text="Legacy Detail" /> | 
        <a href="Search.aspx">Ticket Search</a> |  
        <a href="List.aspx">My Views</a> | 
        <a href="Add2.aspx">Add Ticket</a>
    </div>
    <asp:Literal ID="litMessage" runat="server" />
    <asp:PlaceHolder ID="phRetryCalendar" runat="server" Visible="false">
        <div class="form-group form-actions" style="margin-top:10px; margin-bottom:40px;">
            <asp:LinkButton ID="btnUpdateCalendar2" runat="server" ValidationGroup="vgUpdateCalendar" OnClick="btnUpdateCalendar_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-calendar"></i> Retry Add To Calendar</asp:LinkButton>
        </div>
    </asp:PlaceHolder>
    <asp:Literal ID="litDebug" runat="server" />
    <asp:Panel ID="pnlContainer" runat="server" DefaultButton="btnSubmit">
        <asp:ValidationSummary ID="vsTicket" runat="server" CssClass="validationSummary" ValidationGroup="vgTicket" />
        <asp:ValidationSummary ID="vsAddRecipientBbb" runat="server" CssClass="validationSummary" ValidationGroup="vgAddRecipientBbb" />
        <asp:ValidationSummary ID="vAddRecipientClient" runat="server" CssClass="validationSummary" ValidationGroup="vgAddRecipientClient" />
        <asp:ValidationSummary ID="vsAddRecipientMasterClient" runat="server" CssClass="validationSummary" ValidationGroup="vgAddRecipientMasterClient" />
        <asp:ValidationSummary ID="vsUpload" runat="server" CssClass="validationSummary" ValidationGroup="vgUpload" />
        <asp:ValidationSummary ID="vsMergeTo" runat="server" CssClass="validationSummary" ValidationGroup="vgMergeTo" />
        <asp:ValidationSummary ID="vsMergeFrom" runat="server" CssClass="validationSummary" ValidationGroup="vgMergeFrom" />
        <asp:ValidationSummary ID="vsUpdateCalendar" runat="server" CssClass="validationSummary" ValidationGroup="vgUpdateCalendar" />
        
        <div class="row">
            <div class="col-md-12">
                <div class="block" style="padding-bottom:20px;">
                    <asp:Literal ID="ltNotes" runat="server" /> 
                </div>
            </div>
            
            <div class="col-md-6">
                <div class="block" style="padding-bottom:20px;">
                    <div class="block-title">
                        <h2><strong>Details : <asp:Literal ID="litId" runat="server" /> : <asp:Literal ID="litStatus" runat="server" /> : Time Spent <asp:HyperLink ID="hlTimeSpent" runat="server" /></strong></h2>
                    </div>
                    <div class="block-content">
                        <div class="row" style="margin-top:20px;">
                            <div class="form-group col-sm-12">
                                <div class="block-content">
                                    <asp:PlaceHolder ID="phInternalOnly" runat="server" Visible="false">
                                        <label style="color:red">Ticket Is Internal Only</label> 
                                    </asp:PlaceHolder>
                                </div>
                            </div>
                            <div class="form-group col-sm-6">
                                <div class="block-content">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label"><a href="#modal-edit-client" data-toggle="modal" style="padding-right:5px;"><i class="gi gi-pencil" title="Update Client"></i></a>Client</label>
                                        <div class="col-sm-7 form-control-static"><asp:Literal ID="litClient" runat="server" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label"><a href="#modal-edit-contact" data-toggle="modal" style="padding-right:5px;"><i class="gi gi-pencil" title="Update Contact"></i></a>Contact</label>
                                        <div class="col-sm-7 form-control-static">
                                            <asp:Literal ID="litContactFirst" runat="server" />&nbsp;<asp:Literal ID="litContactLast" runat="server" />&nbsp;<asp:Literal ID="ltNotification" runat="server" />
                                            <asp:Literal ID="litContactPhone" runat="server" /><asp:Literal ID="litCopyContactPhone" runat="server" />
                                            <asp:Literal ID="litContactCellPhone" runat="server" /><asp:Literal ID="litCopyCellPhone" runat="server" />
                                            <asp:Literal ID="litContactHomePhone" runat="server" /><asp:Literal ID="litCopyHomePhone" runat="server" />
                                            <asp:Literal ID="litContactEmail" runat="server" /><asp:Literal ID="litCopyEmail" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Reported</label>
                                        <div class="col-sm-7 form-control-static"><asp:Literal ID="litReported" runat="server" /></div>
                                    </div>
                                    <div class="form-group">
                                        <asp:PlaceHolder ID="phLocation" runat="server" Visible="false">
                                            <label class="col-sm-5 control-label"><a href="#modal-edit-location" data-toggle="modal"><i class="gi gi-pencil" title="Update Location"></i></a>Location</label>
                                            <div class="col-sm-7 form-control-static"><asp:Literal ID="litLocation" runat="server" /></div>
                                        </asp:PlaceHolder>
                                    </div>
                                </div>     
                            </div>
                            <div class="form-group col-sm-6">
                                <div class="block-content">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Sales</label>
                                        <div class="col-sm-7 form-control-static"><asp:Literal ID="litSales" runat="server" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Lead Tech</label>
                                         <div class="col-sm-7 form-control-static"><asp:Label ID="lblLeadTech" runat="server" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Tam</label>
                                        <div class="col-sm-7 form-control-static"><asp:Label ID="lblTam" runat="server" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">CSM</label>
                                        <div class="col-sm-7 form-control-static"><asp:Label ID="lblCSM" runat="server" /></div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Internal Only</label>
                                        <div class="col-sm-7 form-control-static"><asp:CheckBox ID="chkInternal" runat="server" CssClass="form-control form-control-borderless" AutoPostBack="true" OnCheckedChanged="chkInternal_CheckedChanged" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="margin-top:20px;">
                            <div class="form-group col-sm-12">
                                <label>Summary 
                                    <a href="#modal-edit-summary" data-toggle="modal" style="padding-left:5px;"><i class="gi gi-pencil" title="Update Summary"></i></a>
                                    <asp:Literal ID="litCopySummary" runat="server" />
                                </label>
                                <p class="form-control-static well well-sm"><asp:Literal ID="litSummary" runat="server" /></p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-sm-12">
                                <label>Description <a href="#modal-edit-description" data-toggle="modal" style="padding-left:5px;"><i class="gi gi-pencil" title="Update Description"></i></a></label>
                                <asp:TextBox ID="txtDescriptionReadOnly" runat="server" TextMode="MultiLine" Rows="18" Enabled="false" ReadOnly="true" CssClass="form-control"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom:20px;">
                    <div class="block-title">
                        <h2><strong>Check List</strong></h2>
                    </div>
                    <div class="block-content">
                         <div class="row">
                            <div class="form-group col-sm-12">
                                <ddl:TicketCheckListMaster ID="ddlTicketCheckListMaster" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" SetSize="false"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom:20px;">
                    <div class="block-title">
                        <h2><strong>Default Project</strong></h2>
                    </div>
                    <div class="block-content">
                        <div class="row">
                            <div class="form-group col-sm-12">
                                <ddl:ProjectTask ID="ddlDefaultProject" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" DefaultText="" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom:20px;">
                    <div class="block-title">
                        <h2><strong>Client Notifications<asp:Literal ID="litClientNotificationHeader" runat="server" /></strong></h2>
                    </div>
                    <div class="block-content">
                         <div class="row">
                            <div class="col-sm-7 col-xs-12">
                                <div class="input-group">
                                    <lb:ClientContact ID="lbClientContactRecipient" runat="server" DisplayChosenScript="true" UseEmailAsDataValue="true"  />
                                    <%--<ddl:ClientContact ID="ddlClientContactRecipient" runat="server" IsRequired="true" ValidationGroup="vgAddRecipientClient" DisplayChosenScript="true" DefaultValue="" UseEmailAsDataValue="true"
                                        RequiredErrorMessage="Client Contact is required" />--%>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnAddRecipientClient" runat="server" ValidationGroup="vgAddRecipientClient" OnClick="btnAddRecipientClient_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-plus"></i> Add</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <asp:PlaceHolder ID="phAddContactButton" runat="server">
                                <div class="col-sm-1 col-xs-12">
                                    <a href="#modal-add-contact" data-toggle="modal" style="padding-left:5px;"><i class="gi gi-user_add" title="Add New Contact" style="padding-top:10px;"></i></a>
                                    <div class="visible-xs"><br /><br /></div>
                                </div>
                            </asp:PlaceHolder>
                        </div>
                        <grid:TicketContact ID="gridTicketContactClient" runat="server" />
                    </div>
                </div>
                <asp:PlaceHolder ID="phMasterClientsNotification" runat="server" Visible="false">
                    <div class="block" style="padding-bottom:20px;">
                        <div class="block-title">
                            <h2><strong>Master Client<asp:Literal ID="litMasterClientNotificationHeader" runat="server" /></strong></h2>
                        </div>
                        <div class="block-content">
                            <div class="row">
                                <div class="col-sm-7 col-xs-12">
                                    <div class="input-group">
                                        <ddl:ClientContact ID="ddlMasterClientContactRecipient" runat="server" IsRequired="true" ValidationGroup="vgAddRecipientMasterClient" DisplayChosenScript="true" DefaultValue="" UseEmailAsDataValue="true"
                                            RequiredErrorMessage="Master Client Contact is required" />
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnAddRecipientMasterClient" runat="server" ValidationGroup="vgAddRecipientMasterClient" OnClick="btnAddRecipientMasterClient_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-plus"></i> Add</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <grid:TicketContact ID="gridTicketContactMasterClient" runat="server" />
                        </div>
                    </div>
                </asp:PlaceHolder>
                 <div class="block" style="padding-bottom:20px;">
                    <div class="block-title">
                        <h2><strong>Bit By Bit Notifications</strong></h2>
                    </div>
                    <div class="block-content">
                        <div class="row">
                            <div class="col-sm-7 col-xs-12">
                                <div class="input-group">
                                    <lb:Employee ID="lbEmployeeRecipient" runat="server" UseEmailAsDataValue="true" RequiredErrorMessage="Bit By Bit Employee is required" 
                                        DisplayChosenScript="true" DisplayErrorAsterisk="false" SetSize="false" HideQueueEmployees="true" />
                                    <%--<ddl:Employee ID="ddlEmployeeRecipient" runat="server" IsRequired="true" ValidationGroup="vgAddRecipientBbb" DisplayChosenScript="true" DefaultValue="" UseEmailAsDataValue="true"
                                        RequiredErrorMessage="Bit By Bit Employee is required" DisplayErrorAsterisk="false" SetSize="false" HideQueueEmployees="true" />--%>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnAddRecipientBbb" runat="server" ValidationGroup="vgAddRecipientBbb" OnClick="btnAddRecipientBbb_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-plus"></i> Add</asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <grid:TicketContact ID="gridTicketContactEmployee" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="block" style="padding-bottom:20px;">
                    <div class="block-title">
                        <h2><strong>Add Response</strong></h2>
                        <asp:LinkButton ID="btnSubmit2" runat="server" ValidationGroup="vgTicket" OnClick="btnSubmit_Click" CausesValidation="true" CssClass="btn btn-sm btn-info pull-right" style="margin:5px 20px 0 0;"><i class="hi hi-ok"></i> Update</asp:LinkButton>
                        <asp:LinkButton ID="btnMakeActive" runat="server" ToolTip="Make Active" OnClick="btnMakeActive_Click" OnClientClick="javascript:return confirm('Make Active?');" style="margin:5px 20px 0 0;" CssClass="btn btn-sm btn-primary pull-right">
                            <i class="gi gi-circle_arrow_right"></i> Make Active</asp:LinkButton>
                    </div>
                    <div class="block-content">
                        <div class="row">
                            <div class="form-group col-sm-4">
                                <label>
                                    Status <span class="text-danger">*</span>
                                </label>
                                <ddl:TicketDisposition ID="ddlTicketDisposition" runat="server" IsRequired="true" ValidationGroup="vgTicket" DisplayChosenScript="true" ChosenDisplayName="Status" DefaultValue="" DisplayDefaultValue="true" />
                            </div>
                            <div class="form-group col-sm-4">
                                <label>
                                    Ticket Type <span class="text-danger">*</span>
                                </label>
                                <ddl:TicketType ID="ddlTicketType" runat="server" IsRequired="true" ValidationGroup="vgTicket" DisplayChosenScript="true" />
                            </div>
                            <div class="form-group col-sm-4">
                                <label>Assigned To <span class="text-danger">*</span></label>
                                <ddl:Employee ID="ddlEmployeeAssignedTo" runat="server" IsRequired="true" ValidationGroup="vgTicket" DisplayChosenScript="true" DefaultValue="" RequiredErrorMessage="Assigned To is required" DisplayErrorAsterisk="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-sm-12">
                                <label>Client Viewable Notes</label>
                                <asp:TextBox ID="txtExternalNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" PlaceHolder="Enter Client Viewable Notes ..."/>
                            </div>
                        </div>
                        <div class="row">
                            <div class="form-group col-sm-12">
                                <label>Internal Only Notes</label>
                                <asp:TextBox ID="txtInternalNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" PlaceHolder="Enter Internal Only Notes .... "/>
                            </div>
                        </div>
                          <div class="row">
                            <div class="form-group col-sm-4">
                                <label>Ticket Category <asp:Literal ID="litTicketCategoryRequired" runat="server" Text="<span class='text-danger'>*</span>" Visible="false" /></label>
                                <ddl:TicketCategory ID="ddlTicketCategory" runat="server" IsRequired="false" ValidationGroup="vgTicket" DisplayChosenScript="true" DisplayDefaultValue="true"  DefaultText=" " DefaultValue="" />
                            </div>
                            <div class="form-group col-sm-5">
                                <label>Ticket Priority <a href="https://bitxbit.itglue.com/DOC-1106671-1572820" target="_blank" style="padding-left:10px;"><i class="gi gi-circle_question_mark" title="Priority Information"></i></a></label>
                                <ddl:TicketPriority ID="ddlTicketPriority" runat="server" IsRequired="false" ValidationGroup="vgTicket" DisplayChosenScript="true" DisplayDefaultValue="true"  DefaultText=" " DefaultValue="" />
                            </div>
                            <div class="form-group col-sm-3">
                                <asp:Panel ID="pnlDisplayOrder" runat="server" Visible="false">
                                    <label>Priority Order</label>
                                    <asp:TextBox ID="txtPriorityOrder" runat="server" PlaceHolder="Enter Priority Order .... " CssClass="form-control" />
                                    <%--<asp:RegularExpressionValidator ID="revPriorityOrder" runat="server" ControlToValidate="txtPriorityOrder" ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="^\d+$" />--%>
                                    <asp:CustomValidator ID="cvProrityOrder" runat="server" ControlToValidate="txtPriorityOrder" ValidateEmptyText="true" ValidationGroup="vgTicket" OnServerValidate="cvProrityOrder_ServerValidate" 
                                        Display="None" ErrorMessage="Please Enter Priority Order Only Numbers" />
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                                <div class="form-group col-sm-4">
                                <label>Add Timesheet</label> 
                                <asp:CheckBox ID="chkAddTimesheet" runat="server" CssClass="form-control form-control-borderless" />
                            </div>
                            <div class="form-group col-sm-8">
                                <label>Project</label> 
                                <ddl:ProjectTask ID="ddlTimesheetProject" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" DefaultText="" />
                                <asp:CustomValidator ID="cvTimesheet" runat="server" ControlToValidate="txtExternalNotes" ValidateEmptyText="true" ValidationGroup="vgTicket" OnServerValidate="cvTimesheet_ServerValidate" 
                                    Display="None" ErrorMessage="External Notes are required when adding a Timesheet entry" />
                            </div>
                        </div>
                        <div class="row" runat="server" id="divTimesheetStartTime" style="display:none;">
                            <div class="form-group col-sm-8"></div>
                            <div class="form-group col-sm-4">
                                <ddl:TimesheetTime ID="ddlTimesheetStartTime" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="false" HourHeader="Start Hour" MinuteHeader="Start Min" />
                            </div>
                        </div>
                        <div class="row">
                             <div class="form-group col-sm-4">
                                <label>Activity</label> 
                                <ddl:TaskAction ID="ddlTimesheetActivity" runat="server" IsRequired="false" DisplayChosenScript="true" DefaultValue="" DefaultText="" />
                            </div>
                            <div class="form-group col-sm-4">
                                <label>Date</label> 
                                <uc:DatePicker ID="ucTimesheetDate" runat="server" IsRequired="false" />
                            </div>
                            <div class="form-group col-sm-4">
                                <ddl:TimesheetTime ID="ddlTimesheetTime" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="false" />
                            </div>
                        </div>
                        <div class="form-group form-actions" style="margin-top:10px; margin-bottom:40px;">
                            <asp:LinkButton ID="btnSubmit" runat="server" ValidationGroup="vgTicket" OnClick="btnSubmit_Click" CausesValidation="true" CssClass="btn btn-sm btn-info pull-right"><i class="hi hi-ok"></i> Update</asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="block" style="padding-bottom:10px;">
                    <div class="block-title">
                        <h2><strong>Scheduling</strong></h2>
                    </div>
                    <div class="block-content">
                         <div class="row">
                            <div class="form-group col-sm-4">
                                <label>Date</label> 
                                <uc:DatePicker ID="ucScheduledDate" runat="server" IsRequired="false" />
                            </div>
                            <div class="form-group col-sm-4">
                                <ddl:TimesheetTime ID="ddlScheduledTime" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="false" HourHeader="Hour" UseMinuteIncrement="true" MinuteHeader="Min"/>
                            </div>
                             <div class="form-group col-sm-4">
                                <ddl:TimesheetTime ID="ddlDurationTime" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="false" HourHeader="Dur Hour" MinuteHeader="Dur Min"/>
                            </div>
                        </div>
                        <div class="form-group form-actions" style="margin-top:10px; margin-bottom:40px;">
                            <asp:LinkButton ID="btnUpdateCalendar" runat="server" ValidationGroup="vgUpdateCalendar" OnClick="btnUpdateCalendar_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary pull-right"><i class="hi hi-calendar"></i> Add To Calendar</asp:LinkButton>
                        </div>
                        <asp:CustomValidator ID="cvScheduled" runat="server" ControlToValidate="txtExternalNotes" ValidateEmptyText="true" ValidationGroup="vgTicket" OnServerValidate="cvScheduled_ServerValidate" Display="None" />
                        <asp:CustomValidator ID="cvScheduled2" runat="server" ControlToValidate="txtExternalNotes" ValidateEmptyText="true" ValidationGroup="vgUpdateCalendar" OnServerValidate="cvScheduled_ServerValidate" Display="None" />
                    </div>
                </div>
                <div class="block" style="padding-bottom:10px;">
                    <div class="block-title">
                        <h2><strong>Tags</strong></h2>
                    </div>
                    <div class="block-content">
                         <div class="row">
                            <div class="form-group col-sm-12">
                                <div class="row">
                                    <div class="form-group col-sm-10" style="padding-right:0;">
                                        <lb:TicketTag ID="lbTicketTag" runat="server" DisplayChosenScript="true" />
                                    </div>
                                    <div class="form-group col-sm-2" style="padding-left:0;">
                                        <a class="btn btn-sm btn-primary" href="#modal-add-tag" data-toggle="modal"><i class="hi hi-plus" title="Create Tag"></i> Create Tag</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="phCheckList" runat="server">
                    <div class="block">
                        <div class="block-title">
                            <h2><strong>Check List</strong></h2>
                            <a href="#modal-checklist-help" data-toggle="modal"><i class="gi gi-circle_question_mark" title="View Help for Check List" style="padding-bottom:8px;"></i></a>
                        </div>
                        <div class="block-content">
                            <asp:Repeater ID="rptCheckList" runat="server" OnItemDataBound="rptCheckList_ItemDataBound">
                                <ItemTemplate>
                                    <asp:PlaceHolder ID="phHeader" runat="server" Visible="false">
                                        <div class="row" style="margin-left:0;margin-top:10px;">
                                            <div class="col-sm-6"><h5 class="text-info"><strong><asp:Literal ID="litCheckListHeader" runat="server" /></strong></h5></div>
                                        </div>
                                    </asp:PlaceHolder>
                                    <div class="row" style="margin-left:0;">
                                        <div class="col-sm-12">
                                            <label class="checkbox-inline">
                                                <asp:CheckBox ID="chkCheckListCompleted" runat="server" CssClass="checklistText"/>
                                                <asp:Literal ID="litCheckListId" runat="server" Visible="false" />
                                            </label>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
        <div class="block">
            <div class="block-title">
                <h2><strong>History</strong></h2>
            </div>
            <div class="block-content" style="padding-bottom:10px;">
                <div class="row">
                    <div class="form-group col-sm-2">
                        <label>Exclude Alerts</label>
                        <asp:CheckBox ID="chkHistoryExcludeAlerts" runat="server" CssClass="form-control form-control-borderless"  AutoPostBack="true" OnCheckedChanged="chkHistoryView_CheckedChanged"/>
                    </div>
                     <div class="form-group col-md-2 col-sm-6">
                        <label>Exclude Non-Action Updates</label>
                        <asp:CheckBox ID="chkHistoryExcludeViews" runat="server" CssClass="form-control form-control-borderless"  AutoPostBack="true" OnCheckedChanged="chkHistoryView_CheckedChanged"/>
                    </div>
                </div>
                <grid:TicketHistory ID="gridTicketHistory" runat="server" />
            </div>
        </div>
        <div class="block">
            <div class="block-title">
                <h2><strong>Documents</strong></h2>
            </div>
            <div class="block-content" style="padding-bottom:10px;">
                <div class="row">
                    <div class="form-group col-sm-12">
                        <label>Select Files for Upload (<em><asp:Literal ID="litAllowedFileExtensions" runat="server" /></em>)</label> 
                        <asp:FileUpload ID="fuOne" runat="server" AllowMultiple="true" />
                        <asp:RequiredFieldValidator ID="rfvFileOne" runat="server" ErrorMessage="File is required" ControlToValidate="fuOne" ValidationGroup="vgUpload" Display="None" />
                        <asp:CustomValidator ID="cvFile" runat="server" ControlToValidate="txtValidator" ErrorMessage="File is required" ValidationGroup="vgUpload" Display="None"
                            ValidateEmptyText="true" OnServerValidate="cvFile_ServerValidate" />
                        <div style="display:none"><asp:TextBox ID="txtValidator" runat="server" Width="0" /></div>
                        <asp:CustomValidator ID="cvFile2" runat="server" ControlToValidate="txtValidator" ValidationGroup="vgTicket" Display="None"
                            ValidateEmptyText="true" OnServerValidate="cvFile2_ServerValidate" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-2 col-sm-4">
                        <asp:LinkButton ID="btnUpload" runat="server" ValidationGroup="vgUpload" OnClick="btnUpload_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-upload"></i> Upload</asp:LinkButton>
                    </div>
                    <div class="form-group col-md-2 col-sm-4">
                        <asp:LinkButton ID="lbDocumentViewer" runat="server" OnClientClick="aspnetForm.target ='_blank';" OnClick="lbDocumentViewer_Click" CssClass="btn btn-sm btn-primary"><i class="hi hi-open"></i> Document viewer</asp:LinkButton>
                    </div>
                </div>
                <grid:TicketFile ID="gridTicketFile" runat="server" />
            </div>
        </div>
        <div class="block">
            <div class="block-title">
                <h2><strong>Merge</strong></h2>
            </div>
            <div class="block-content" style="padding-bottom:10px;">
                <div class="row">
                    <div class="col-sm-6"><label>Merge this ticket into another master ticket (close this ticket)</label></div>
                    <div class="col-sm-6"><label>Merge selected tickets into this master ticket (selected tickets are closed)</label></div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <asp:MultiView ID="mvMergeTo" runat="server" ActiveViewIndex="0">
                            <asp:View ID="viewMergeToEdit" runat="server">
                                <div class="input-group">
                                    <ddl:Ticket ID="ddlTicketMergeTo" runat="server" IsRequired="true" ValidationGroup="vgMergeTo" DefaultValue="" />
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnMergeTo" runat="server" ValidationGroup="vgMergeTo" OnClick="btnMergeTo_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="gi gi-circle_arrow_right"></i> Merge To</asp:LinkButton>
                                    </span>
                                </div>
                            </asp:View>
                            <asp:View ID="viewMergeToComplete" runat="server">
                                <asp:Hyperlink ID="hlMergeCompleteTo" runat="server" Target="_blank" />
                            </asp:View>
                        </asp:MultiView>
                    </div>
                    <div class="col-sm-6">
                        <div class="input-group">
                            <lb:Ticket ID="lbTicketMergeFrom" runat="server" IsRequired="true" ValidationGroup="vgMergeFrom" DisplayChosenScript="true" DefaultValue="" />
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnMergeFrom" runat="server" ValidationGroup="vgMergeFrom" OnClick="btnMergeFrom_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="gi gi-circle_arrow_left"></i> Merge From</asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <div class="block">
            <div class="block-title">
                <h2><strong>Ticket Active History</strong></h2>
            </div>
            <div class="block-content" style="padding-bottom:10px;">
                <grid:ActiveTicketHistory ID="gridActiveHistory" runat="server" />
            </div>
         </div>
        <div class="block">
            <div class="form-group form-actions">
                <asp:LinkButton ID="btnSubmit3" runat="server" ValidationGroup="vgTicket" OnClick="btnSubmit_Click" CausesValidation="true" CssClass="btn btn-sm btn-info"><i class="hi hi-ok"></i> Update</asp:LinkButton>
            </div>
        </div>
        <asp:Label id="lblEnd" runat="server" />
    </asp:Panel>
    <div id="modal-edit-client" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Update Client</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsEditClient" runat="server" ValidationGroup="vgEditClient" CssClass="validationSummary" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Client<span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <%--<ddl:Client ID="ddlClient" runat="server" IsRequired="true" DisplayChosenScript="true" DefaultValue="" DefaultText="" DisplayDefaultValue="true" ValidationGroup="vgEditClient"/>--%>
                                <ddl:SmartClient ID="ddlSmartClient" runat="server" Width="100%" />
                            </div>
                        </div>
                         <div class="modal-footer">
                            <asp:LinkButton ID="btnSaveClient" runat="server" class="btn btn-sm btn-primary" OnClick="btnSaveClient_Click" CausesValidation="true" ValidationGroup="vgEditClient"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-edit-contact" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Update Contact</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsEditContact" runat="server" ValidationGroup="vgEditContact" CssClass="validationSummary" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Contact</label>
                            <div class="col-sm-9">
                                <ddl:ClientContact ID="ddlEditClientContact" runat="server" IsRequired="false" DisplayChosenScript="true" DisplayDefaultValue="true" DefaultText="" DefaultValue="" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">First <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEditContactFirst" runat="server" CssClass="form-control" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvEditContactFirst" runat="server" ControlToValidate="txtEditContactFirst" ValidationGroup="vgEditContact" ErrorMessage="First Name is required" Display="None" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Last <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEditContactLast" runat="server" CssClass="form-control" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvEditContactLast" runat="server" ControlToValidate="txtEditContactLast" ValidationGroup="vgEditContact" ErrorMessage="Last Name is required" Display="None" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Email <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEditContactEmail" runat="server" CssClass="form-control" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvEditContactEmail" runat="server" ControlToValidate="txtEditContactEmail" ValidationGroup="vgEditContact" ErrorMessage="Email is required" Display="None"/>
                                 <asp:CustomValidator ID="cvEditContactEmail" runat="server" ControlToValidate="txtEditContactEmail" ValidationGroup="vgEditContact" OnServerValidate="cvEditContactEmail_ServerValidate" Display="None" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Phone <span class="text-danger">*</span></label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtEditContactPhone" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RequiredFieldValidator ID="rfvEditContactPhone" runat="server" ControlToValidate="txtEditContactPhone" ValidationGroup="vgEditContact" ErrorMessage="Phone is required" Display="None"/>
                                <asp:RegularExpressionValidator ID="revEditContactPhone" runat="server" ControlToValidate="txtEditContactPhone" ValidationGroup="vgEditContact" Display="None"/>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtEditContactPhoneExtension" runat="server" CssClass="form-control" MaxLength="6" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cell Phone </label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEditContactCellPhone" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RegularExpressionValidator ID="revEditContactCellPhone" runat="server" ControlToValidate="txtEditContactCellPhone" ValidationGroup="vgEditContact" Display="None"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Home Phone </label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtEditContactHomePhone" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RegularExpressionValidator ID="revEditContactHomePhone" runat="server" ControlToValidate="txtEditContactHomePhone" ValidationGroup="vgEditContact" Display="None"/>
                            </div>
                        </div>
                        <asp:Panel ID="pnlLocation" runat="server">
                            <div class="form-group">
                            <label class="col-sm-3 control-label">Location </label>
                            <div class="col-sm-9">
                                <ddl:ClientLocation ID="ddlEditClientLocation" runat="server" DisplayChosenScript="true" ValidationGroup="vgEditContact" DefaultText="" DefaultValue="" DisplayDefaultValue="true"/>
                            </div>
                        </div>
                        </asp:Panel>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Action <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlEditClientContactAction" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Update Ticket and Update Contact" Value="U"></asp:ListItem>
                                <asp:ListItem Text="Update Ticket and Add Contact" Value="A"></asp:ListItem>
                            </asp:DropDownList>
                                <asp:Literal ID="litEditClientContactActionJs" runat="server" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnSaveContact" runat="server" class="btn btn-sm btn-primary" OnClick="btnSaveContact_Click" CausesValidation="true" ValidationGroup="vgEditContact"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-add-contact" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Add Contact</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsAddContact" runat="server" ValidationGroup="vgAddContact" CssClass="validationSummary" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">First <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAddContactFirst" runat="server" CssClass="form-control" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvAddContactFirst" runat="server" ControlToValidate="txtAddContactFirst" ValidationGroup="vgAddContact" ErrorMessage="First Name is required" Display="None" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Last <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAddContactLast" runat="server" CssClass="form-control" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvAddContactLast" runat="server" ControlToValidate="txtAddContactLast" ValidationGroup="vgAddContact" ErrorMessage="Last Name is required" Display="None" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Email <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAddContactEmail" runat="server" CssClass="form-control" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvAddContactEmail" runat="server" ControlToValidate="txtAddContactEmail" ValidationGroup="vgAddContact" ErrorMessage="Email is required" Display="None"/>
                                 <asp:CustomValidator ID="cvAddContactEmail" runat="server" ControlToValidate="txtAddContactEmail" ValidationGroup="vgAddContact" OnServerValidate="cvAddContactEmail_ServerValidate" Display="None" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Phone <span class="text-danger">*</span></label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtAddContactPhone" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RequiredFieldValidator ID="rfvAddContactPhone" runat="server" ControlToValidate="txtAddContactPhone" ValidationGroup="vgAddContact" ErrorMessage="Phone is required" Display="None"/>
                                <asp:RegularExpressionValidator ID="revAddContactPhone" runat="server" ControlToValidate="txtAddContactPhone" ValidationGroup="vgAddContact" Display="None"/>
                            </div>
                             <div class="col-sm-3">
                                <asp:TextBox ID="txtAddContactPhoneExtension" runat="server" CssClass="form-control" MaxLength="6" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Cell Phone </label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAddContactCellPhone" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RegularExpressionValidator ID="revAddContactCellPhone" runat="server" ControlToValidate="txtAddContactCellPhone" ValidationGroup="vgAddContact" Display="None"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Home Phone </label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAddContactHomePhone" runat="server" CssClass="form-control" MaxLength="20" />
                                <asp:RegularExpressionValidator ID="revAddContactHomePhone" runat="server" ControlToValidate="txtAddContactHomePhone" ValidationGroup="vgAddContact" Display="None"/>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnAddContact" runat="server" class="btn btn-sm btn-primary" OnClick="btnAddContact_Click" CausesValidation="true" ValidationGroup="vgAddContact"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-edit-summary" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Update Summary</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsEditSummary" runat="server" ValidationGroup="vgEditSummary" CssClass="validationSummary" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Summary<span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtSummary" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" PlaceHolder="Ticket Summary"/>
                                <asp:RequiredFieldValidator ControlToValidate="txtSummary" runat="server" ID="rfvSummary" ValidationGroup="vgEditSummary" Display="None" ErrorMessage="Summary is required" />
                            </div>
                        </div>
                         <div class="modal-footer">
                            <asp:LinkButton ID="btnSaveSummary" runat="server" class="btn btn-sm btn-primary" OnClick="btnSaveSummary_Click" CausesValidation="true" ValidationGroup="vgEditSummary"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-edit-description" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Update Description</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsEditDescription" runat="server" ValidationGroup="vgEditDescription" CssClass="validationSummary" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Description<span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" PlaceHolder="Ticket Description"/>
                                <asp:RequiredFieldValidator ControlToValidate="txtDescription" runat="server" ID="rfvDescription" ValidationGroup="vgEditDescription" Display="None" ErrorMessage="Description is required" />
                            </div>
                        </div>
                         <div class="modal-footer">
                            <asp:LinkButton ID="btnSaveDescription" runat="server" class="btn btn-sm btn-primary" OnClick="btnSaveDescription_Click" CausesValidation="true" ValidationGroup="vgEditDescription"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-add-tag" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Create Tag</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsAddTag" runat="server" CssClass="validationSummary" ValidationGroup="vgAddTag" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Value <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNewTag" runat="server" CssClass="form-control" PlaceHolder="Enter New Tag ... " MaxLength="50"/>
                                <asp:RequiredFieldValidator ID="rfvNewTag" runat="server" ControlToValidate="txtNewTag" ValidationGroup="vgAddTag" ErrorMessage="Tag Value is required" Display="None" />
                                <asp:CustomValidator ID="cvAddTag" runat="server" ControlToValidate="txtNewTag" ValidationGroup="vgAddTag" ErrorMessage="Tag already exists for ticket" OnServerValidate="cvAddTag_ServerValidate" ValidateEmptyText="true" Display="None"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Public </label>
                            <div class="col-sm-9">
                                <asp:CheckBox ID="chkTagPublic" runat="server" CssClass="form-control form-control-borderless" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Client Public </label>
                            <div class="col-sm-9">
                                <asp:CheckBox ID="chkTagClientPublic" runat="server" CssClass="form-control form-control-borderless" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="btnAddTag" runat="server" ValidationGroup="vgAddTag" OnClick="btnAddTag_Click" CausesValidation="true" CssClass="btn btn-sm btn-primary"><i class="hi hi-ok"></i> Create Tag</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-edit-location" class="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Update Location</h3>
                </div>
                <div class="modal-body">
                    <asp:ValidationSummary ID="vsEditLocation" runat="server" CssClass="validationSummary" ValidationGroup="vgEditLocation" />
                    <div class="form-horizontal form-bordered">
                        <div class="form-group">
                            <label class="col-sm-3 control-label">Location <span class="text-danger">*</span></label>
                            <div class="col-sm-9">
                                <ddl:ClientLocation ID="ddlClientLocation" runat="server" IsRequired="true" DisplayChosenScript="true" ValidationGroup="vgEditLocation"/>
                            </div>
                        </div>
                         <div class="modal-footer">
                            <asp:LinkButton ID="btnSaveLocation" runat="server" class="btn btn-sm btn-primary" OnClick="btnSaveLocation_Click" CausesValidation="true" ValidationGroup="vgEditLocation"><i class="hi hi-ok"></i> Save</asp:LinkButton>
                            <button type="button" class="btn btn-sm btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modal-checklist-help" class="modal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h3 class="modal-title">Check List Help</h3>
                </div>
                <div class="modal-body">
                    Use the following syntax in the "Client Viewable Notes" textbox:<br /><br />
                    [checklist]<br />
                    ~ task one<br />
                    ~ task two<br />
                    ~ task three<br />
                    ~ it doesn't matter can be anything<br />
                    [/checklist]

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


