<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketAdd.ascx.cs" Inherits="UserControl_Grid_EditForm_TicketAdd" %>
<%@ Register TagPrefix="ddl" TagName="TicketCheckListMaster" Src="~/UserControl/DropDownList/TicketCheckListMaster.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCategory" Src="~/UserControl/DropDownList/TicketCategory.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmployeeDDL" Src="~/UserControl/DropDownList/Employee.ascx" %>

<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        function ShowAll() {
            $find('cpeBehaviorGeneral').expandPanel();
            $find('cpeBehaviorDetails').expandPanel();
            $find('cpeBehaviorEmployeeEmail').expandPanel();
            $find('cpeBehaviorClientEmail').expandPanel();
        }

        function HideAll() {
            $find('cpeBehaviorGeneral').collapsePanel();
            $find('cpeBehaviorDetails').collapsePanel();
            $find('cpeBehaviorEmployeeEmail').collapsePanel();
            $find('cpeBehaviorClientEmail').collapsePanel();
        }
</script>
</telerik:radscriptblock>

<div id="divMain" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;">

<asp:PlaceHolder ID="phTopNavigation" runat="server">
    <table border="0" class="gridEditForm">
        <tr>
            <td style="width:100%; text-align:center">
                <asp:MultiView ID="mvTopButtons" runat="server">
                
                    <asp:View ID="viewTopGrid" runat="server">
                    
                        <asp:button id="btnAddTop" text="ADD" runat="server" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" CommandName="PerformInsert" />
                        &nbsp;
                        <asp:Button ID="btnCloseTop" Text="CLOSE" runat="server" CssClass="actionbutton2" 
                            CommandName="Cancel" />
                        &nbsp;
                            
                    </asp:View>
                    
                    <asp:View ID="viewTopPage" runat="server">
                    
                        <asp:Button ID="btnAddPageTop" runat="server" Text="ADD" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" OnClick="btnAddPage_Click" />
                        &nbsp;
                        
                    </asp:View>
                    
                </asp:MultiView>
                
                <asp:Button ID="btnShowAllTop" runat="server" Text="SHOW ALL" CausesValidation="false"
                    CssClass="actionbutton2" OnClientClick="ShowAll(); return false;"/>
                &nbsp;    
                <asp:Button ID="btnHideAllTop" runat="server" Text="HIDE ALL" CausesValidation="false"
                    CssClass="actionbutton2" OnClientClick="HideAll(); return false;"/>
            </td>
        </tr>
    </table>                
</asp:PlaceHolder>

<asp:ValidationSummary ID="vsTicket" runat="server" 
    CssClass="ValidationSummary" ValidationGroup="vgTicket" />
    
<asp:PlaceHolder ID="phGeneral" runat="server">
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeGeneral" 
        runat="Server"
        TargetControlID="pnlGeneral"
        ExpandControlID="pnlShowHideGeneral"
        CollapseControlID="pnlShowHideGeneral" 
        Collapsed="True"
        BehaviorID="cpeBehaviorGeneral"
        TextLabelID="lblGeneralHeader"
        ImageControlID="imgGeneralHeader"    
        ExpandedText="GENERAL (click to hide)"
        CollapsedText="GENERAL (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideGeneral" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblGeneralHeader" runat="server">Show General</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgGeneralHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="GENERAL (click to show)"/>
            </div>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlGeneral" runat="server">
        
         <table border="0" class="gridEditForm" width="100%" style="width:100%">
            <asp:PlaceHolder ID="phClientOnHoldWarning" runat="server" Visible="false">
                <tr>
                    <th style="width:12%;" class="Required">&nbsp;</th>
                    <td colspan="3" style="font-size:16px; font-weight:bold; color:Red;">Client Status = On Hold.  Please Refer Client To Accounting.</td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <th style="width:12%;" class="Required">Summary:</th>
                <td colspan="3">
                    <!--bug with IE expanding table cell if textbox has fixed width-->
                    <asp:TextBox ID="tbSummary" runat="server" CssClass="textbox" MaxLength="200" Width="60%" />
                    <asp:RequiredFieldValidator ID="rfvSummary" runat="server" ControlToValidate="tbSummary" 
                        ErrorMessage="Summary is required" ValidationGroup="vgTicket">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th style="width:12%;" class="Required">Client Name:</th>
                <td colspan="3">
                    <uc:ClientComboBox ID="ucClient" runat="server" 
                        IsRequired="true" ValidationGroup="vgTicket" 
                        ValidationErrorMessage="Client Name is required"/>
                </td>
            </tr>
            <tr>
                <th style="width:12%;" class="Required">Client Contact:</th>
                <td colspan="3" style="padding:0; margin:0;">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:240px;">
                                <uc:ClientContactDDL ID="ucClientContact" runat="server" 
                                    CssClass="dropdown" Width="210"/>
                            </td>
                            <th style="width:50px;">                    
                                Action: 
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlClientContactAction" runat="server">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Update" Value="U"></asp:ListItem>
                                    <asp:ListItem Text="Add" Value="A"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th style="width:12%;" class="Required">First:</th>
                <td colspan="3" style="padding:0; margin:0;">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:240px;">
                                <asp:TextBox ID="tbReportedByFirstName" runat="server" MaxLength="50" CssClass="textbox"/>
                                <asp:RequiredFieldValidator ID="rfvReportedByFirstName" runat="server" 
                                    ControlToValidate="tbReportedByFirstName" ValidationGroup="vgTicket"
                                    ErrorMessage="Client Contact First Name is required">
                                    <span class="error">*</span>
                                </asp:RequiredFieldValidator>
                            </td>
                            <th style="width:50px;" class="Required">Last:</th>
                            <td>
                                <asp:TextBox ID="tbReportedByLastName" runat="server" MaxLength="50" CssClass="textbox"/>
                                <asp:RequiredFieldValidator ID="rfvReportedByLastName" runat="server" 
                                    ControlToValidate="tbReportedByLastName" ValidationGroup="vgTicket"
                                    ErrorMessage="Client Contact Last Name is required">
                                    <span class="error">*</span>
                                </asp:RequiredFieldValidator>
                                &nbsp;
                                Notify Client:
                                <asp:CheckBox ID="chkEnteredByRecieveTicket" runat="server" />
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th style="width:12%;" class="Required">Email:</th>
                <td colspan="3" style="padding:0; margin:0;">
                     <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:240px;">
                                <asp:TextBox ID="tbReportedByEmail" runat="server" MaxLength="75" CssClass="textbox"/>
                                <asp:RequiredFieldValidator ID="rfvReportedByEmail" runat="server" 
                                    ControlToValidate="tbReportedByEmail" ValidationGroup="vgTicket"
                                    ErrorMessage="Client Contact Email is required">
                                    <span class="error">*</span>
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvClientContactEmail" runat="server"
                                    ControlToValidate="tbReportedByEmail" ValidationGroup="vgTicket"
                                    ErrorMessage="Client Contact Email is already in use" 
                                    OnServerValidate="cvClientContactEmail_ServerValidate">
                                    <span class="error">*</span>
                                </asp:CustomValidator>
                            </td>
                            <th style="width:50px;">Phone:</th>
                            <td>
                                <asp:TextBox ID="tbReportedByPhone" runat="server" 
                                    MaxLength="17" CssClass="textbox"/>
                                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="tbReportedByPhone" ValidationGroup="vgTicket"><span class="error">*</span></asp:RegularExpressionValidator>
                             </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr>
                <th style="width:12%;">Cell:</th>
                <td colspan="3" style="padding:0; margin:0;">
                     <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:240px;">
                                <asp:TextBox ID="txtCellPhone" runat="server" MaxLength="17" CssClass="textbox"/>
                                <asp:RegularExpressionValidator ID="revCellPhone" runat="server" ControlToValidate="txtCellPhone" ValidationGroup="vgTicket"><span class="error">*</span></asp:RegularExpressionValidator>
                            </td>
                            <th style="width:50px;">Home:</th>
                            <td>
                                <asp:TextBox ID="txtHomePhone" runat="server" MaxLength="17" CssClass="textbox"/>
                                <asp:RegularExpressionValidator ID="revHomePhone" runat="server" ControlToValidate="txtHomePhone" ValidationGroup="vgTicket"><span class="error">*</span></asp:RegularExpressionValidator>
                             </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th style="width:12%; vertical-align:top;">Priority:</th>
                <td colspan="3">
                    <uc:TicketPriorityDDL ID="ucTicketPriority" runat="server" Width="300" DefaultText="" DefaultValue="-1" CssClass="dropdown"/>
                    &nbsp;<a href="https://bitxbit.itglue.com/DOC-1106671-1572820" target="_blank"><img src="/Images/question.png" alt="Help" style="vertical-align:middle;" /></a>
                </td>
            </tr>
            <tr>
                <th style="width:12%; vertical-align:top;" class="Required">Assigned To:</th>
                <td colspan="3">
                    <uc:EmployeeDDL ID="ucEmployeeAssignedTo" runat="server" 
                        RequiredErrorMessage="Assigned To is required" CssClass="dropdown"
                        ValidationGroup="vgTicket" Width="300" DefaultValue="" EmployeeTypeToDisplay="AssignTo" />
                </td>
            </tr>
            <tr>
                <th style="width:145px;" class="Required">Status:</th>
                <td style="width:120px;" colspan="3">
                    <uc:TicketDispositionDDL ID="ucTicketDisposition" runat="server" DefaultValue=""
                        DefaultText="" CssClass="dropdown" Width="300" ValidationGroup="vgTicket"
                        RequiredErrorMessage="Status is required" />
                </td>
            </tr>
            <tr>
                <th style="width:145px;" class="Required">Type:</th>
                <td style="width:120px;" colspan="3">
                    <ddl:TicketType ID="ddlTicketType" runat="server" DefaultValue="" DefaultText="" CssClass="dropdown" Width="300" ValidationGroup="vgTicket" IsRequired="true" DisplayChosenScript="false" />
                </td>
            </tr>
             <tr>
                <th style="width:145px;"><asp:Literal ID="litCategoryHeader" runat="server" Text="Category" />:</th>
                <td style="width:120px;" colspan="3">
                    <ddl:TicketCategory ID="ddlTicketCategory" runat="server" DefaultValue="" DefaultText="" CssClass="dropdown" Width="300" ValidationGroup="vgTicket" IsRequired="false" DisplayChosenScript="false"/>
                </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Product:</th>
                <td style="width:30%">
                    <uc:ProductDDL ID="ucTicketProduct" runat="server" DefaultText="" DefaultValue="" IsRequired="false" ValidationGroup="vgTicket" CssClass="dropdown" Width="300"/>
                </td>
              <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Check List:</th>
                <td colspan="3">
                    <ddl:TicketCheckListMaster ID="ddlTicketCheckListMaster" runat="server" DefaultText="" DefaultValue="" IsRequired="false" CssClass="dropdown" Width="300"/>
                    <asp:LinkButton ID="btnCopyCheckList" runat="server" Text="Copy To Details" CausesValidation="false" OnClick="btnCopyCheckList_Click" style="padding-left:15px;"/>
              </td>
            </tr>
            <tr>
                <th style="width:12%; vertical-align:top;">Estimate:</th>
                <td colspan="3">
                    <asp:TextBox ID="txtHoursToFix" runat="server" MaxLength="10" Width="95"></asp:TextBox>
                    <asp:Literal ID="litDebug" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width:12%; vertical-align:top;">Tag:</th>
                <td colspan="3">
                    <asp:CheckBox ID="chkTag" runat="server" />&nbsp;
                    <uc:UserTicketTagsDDL ID="ucUserTicketTagsDDL" runat="server" 
                        DisplayDefaultValue="true" DefaultText="-- select --"/>
                    &nbsp;<em>or</em>&nbsp;
                    <asp:TextBox ID="tbTagText" runat="server" Width="200" />
                    &nbsp;<asp:Label ID="lblPublic" runat="server" Text="Public" ToolTip="Public - tag is available for all employees to use"/>&nbsp;<asp:CheckBox ID="chkTagPublic" runat="server" />
                    &nbsp;<asp:Label ID="lblClientPublic" runat="server" Text="Client Public" ToolTip="Client And Public - tag is available for all clients and employees to use"/>&nbsp;<asp:CheckBox ID="chkTagClientPublic" runat="server" />
                    <asp:CustomValidator ID="cvAddTag" runat="server"
                        ControlToValidate="tbTagText" ValidationGroup="vgTicket"
                        ErrorMessage="Tag already exists for ticket" 
                        OnServerValidate="cvAddTag_ServerValidate" ValidateEmptyText="true">
                        <span class="error">*</span>
                    </asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <th style="width:12%; vertical-align:top;" class="Required">Details:</th>
                <td colspan="3">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                        <tr>
                            <td style="width:60%;  vertical-align:top;">
                                <!--bug with IE expanding table cell if textbox has fixed width-->
                                <asp:TextBox ID="tbNotes" runat="server" TextMode="MultiLine" Rows="12" Width="95%"/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" if="rfvNotes" runat="server"
                                    ControlToValidate="tbNotes" ValidationGroup="vgTicket"
                                    ErrorMessage="Details are required">
                                    <span class="error">*</span>
                                </asp:RequiredFieldValidator>
                            </td>
                            <td style="width:40%; vertical-align:top;">
                                <!--begin timesheet entry box--> 
                                <table style="border:solid 1px gray; width:460px;">
                                    <tr>
                                        <th style="width:130px;">Add Timesheet:</th>
                                        <td style="width:325px;">
                                            <asp:CheckBox ID="chkAddTimesheet" runat="server" />
                                            <asp:CustomValidator ID="cvTimesheet" runat="server"
                                                ControlToValidate="rdpTimesheet" ValidationGroup="vgTicket"
                                                ErrorMessage="Missing Timesheet values" 
                                                OnServerValidate="cvTimesheet_ServerValidate">
                                                <span class="error">*</span>
                                            </asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Date:</th>
                                        <td>
                                            <telerik:RadDatePicker id="rdpTimesheet" Runat="server" SharedCalendarID="sharedCalendar" 
                                                Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
                                            </telerik:RadDatePicker>
                                            <telerik:RadCalendar ID="sharedCalendar" runat="server" EnableMultiSelect="false" EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                                                <FastNavigationSettings EnableTodayButtonSelection="true" />
                                            </telerik:RadCalendar>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Start Time:</th>
                                        <td>
                                            <uc:TimeDDL ID="ucTimesheetStartDDL" runat="server" 
                                                FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Time Spent:</th>
                                        <td>
                                            <uc:TimeDDL ID="ucTimesheetSpentDDL" runat="server" StartHourAtZero="true"
                                                FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder ID="phOvertime" runat="server" Visible="false">
                                        <tr>
                                            <th style="width:130px;">Is Overtime:</th>
                                            <td><asp:CheckBox ID="chkTimesheetIsOvertime" runat="server" /></td>
                                        </tr>
                                    </asp:PlaceHolder>
                                    <tr>
                                        <th style="width:130px;">Project:</th>
                                        <td>
                                            <uc:ProjectTaskDDL id="ucTimesheetProject" runat="server" Width="200"
                                                CssClass="dropdown" DefaultValue="" DefaultText=""/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Activity:</th>
                                        <td>
                                            <uc:TaskActionDDL ID="ucTimesheetActivity" runat="server" Width="200"
                                                CssClass="dropdown" DefaultValue="-1" DefaultText=""/>
                                        </td>
                                    </tr>
                                </table>
                                <!--end timesheet entry box--> 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <th style="width:12%; vertical-align:top;">Internal Notes:</th>
                <td colspan="3">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                        <tr>
                            <td style="width:60%;  vertical-align:top;">
                                <asp:TextBox ID="tbInternalNotes" runat="server" TextMode="MultiLine" 
                                    Rows="12" Width="95%"/>
                            </td>
                            <td style="width:40%; vertical-align:top;">
                                <!--begin scheduled / received box--> 
                                <table style="border:solid 1px gray; width:460px;">
                                   <tr>
                                        <th style="width:130px;">Scheduled:</th>
                                        <td style="width:325px;">
                                            <uc:DateAndTimeCalendar ID="ucDateTimeScheduled" runat="server" TimeCssClass="dropdown" IsRequired="false" ValidationGroup="vgTicket" 
                                                TimeRequiredIfDateSelected="true" TimeRequiredIfDateSelectedErrorMessage="Scheduled Time required when selecting Scheduled Date" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Scheduled Duration:</th>
                                        <td style="width:325px;">
                                            <uc:TimeDDL ID="ucTimeDDLScheduledDuration" runat="server" IsRequired="false" CssClass="dropdown" FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
                                            <asp:CustomValidator ID="cvScheduledDuration" runat="server"  ControlToValidate="rdpTimesheet" ValidationGroup="vgTicket" ErrorMessage="Scheduled Duration is required" OnServerValidate="cvScheduledDuration_ServerValidate">
                                                <span class="error">*</span>
                                            </asp:CustomValidator>
                                            &nbsp;
                                            Send Invite: <asp:CheckBox ID="chkScheduledSendInvite" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Need Completed:</th>
                                        <td style="width:325px;">
                                            <uc:DateAndTimeCalendar ID="ucDateTimeNeedCompletedBy" runat="server" TimeCssClass="dropdown" IsRequired="false" ValidationGroup="vgTicket"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width:130px;">Received:</th>
                                        <td style="width:325px;">
                                            <uc:DateAndTimeCalendar ID="ucDateTimeReceived" runat="server" TimeCssClass="dropdown" 
                                                IsRequired="true" ValidationGroup="vgTicket"
                                                HourRequiredErrorMessage="Received Hour is required" 
                                                MinuteRequiredErrorMessage="Received Minute is required" 
                                                DateRequiredErrorMessage="Received Date is required"/>
                                        </td>    
                                    </tr>
                                </table>
                                <!--end scheduled / received box--> 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </asp:Panel>
    
</asp:PlaceHolder>

<asp:PlaceHolder ID="phDetails" runat="server">
    
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeDetails" 
        runat="Server"
        TargetControlID="pnlDetails"
        ExpandControlID="pnlShowHideDetails"
        CollapseControlID="pnlShowHideDetails" 
        Collapsed="True" 
        BehaviorID="cpeBehaviorDetails"
        TextLabelID="lblDetailsHeader"
        ImageControlID="imgDetailsHeader"    
        ExpandedText="DETAILS (click to hide)"
        CollapsedText="DETAILS (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideDetails" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblDetailsHeader" runat="server">Show Details</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgDetailsHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="DETAILS (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlDetails" runat="server">

        <table border="0" class="gridEditForm">
            <tr>
                <th style="width:120px;" class="Required">Designation:</th>
                <td style="width:120px;">
                     <uc:TicketDesignationDDL ID="ucTicketDesignation" runat="server" 
                        Width="100" CssClass="dropdown" ValidationGroup="vgTicket"/>
                </td>
                 <th style="width:145px;">Received Via:</th>
                <td style="width:120px;">
                    <uc:TicketReceivedMethodDDL ID="ucTicketReceivedMethod" runat="server" Width="100" CssClass="dropdown" ValidationGroup="vgTicket"/>
                </td>
                <th style="width:95px;">Recurring</th>
                <td><asp:CheckBox ID="chkIsRecurring" runat="server" /></td>
            </tr>
            <tr>
                <th style="width:120px;">Archive/Library:</th>
                <td style="width:120px;">
                    <asp:CheckBox ID="chkArchive" runat="server" />
                </td>
                <th style="width:145px;">Review Case/Ticket:</th>
                <td style="width:120px;">
                    <asp:CheckBox ID="chkReview" runat="server" />
                </td>
                <th style="width:95px;">Auto Close:</th>
                <td>
                    <asp:CheckBox ID="chkAutoClose" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width:120px;">Internal Only:</th>
                <td style="width:120px;">
                    <asp:CheckBox ID="chkInternalOnly" runat="server" />
                </td>
                <th style="width:145px;">Use For Reporting:</th>
                <td style="width:120px;">
                    <asp:CheckBox ID="chkUseForReporting" runat="server" />
                </td>
                <th style="width:95px;">Silence:</th>
                <td><asp:CheckBox ID="chkSilentMode" runat="server" /></td>
            </tr>
            <tr>
                <th style="width:120px; vertical-align:top;">Timer:</th>
                <td style="width:120px;">
                    <asp:DropDownList ID="ddlTimer" runat="server" CssClass="dropdown" Width="100">
                        <asp:ListItem Text="None" Value=""></asp:ListItem>
                        <asp:ListItem Text="1 Hour" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2 Hours" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3 Hours" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4 Hours" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5 Hours" Value="5"></asp:ListItem>
                        <asp:ListItem Text="6 Hours" Value="6"></asp:ListItem>
                        <asp:ListItem Text="7 Hours" Value="7"></asp:ListItem>
                        <asp:ListItem Text="8 Hours" Value="8"></asp:ListItem>
                        <asp:ListItem Text="9 Hours" Value="9"></asp:ListItem>
                        <asp:ListItem Text="10 Hours" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11 Hours" Value="11"></asp:ListItem>
                        <asp:ListItem Text="12 Hours" Value="12"></asp:ListItem>
                        <asp:ListItem Text="1 Day" Value="24"></asp:ListItem>
                        <asp:ListItem Text="2 Days" Value="48"></asp:ListItem>
                        <asp:ListItem Text="3 Days" Value="72"></asp:ListItem>
                    </asp:DropDownList>
                </td>    
            </tr>
        </table>
    
    </asp:Panel>
    
</asp:PlaceHolder>
    
<asp:PlaceHolder ID="phEmployeeEmail" runat="server">

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeEmployeeEmail" 
        runat="Server"
        TargetControlID="pnlEmployeeEmail"
        ExpandControlID="pnlShowHideEmployeeEmail"
        CollapseControlID="pnlShowHideEmployeeEmail" 
        Collapsed="True"
        BehaviorID="cpeBehaviorEmployeeEmail"
        TextLabelID="lblEmployeeEmailHeader"
        ImageControlID="imgEmployeeEmailHeader"    
        ExpandedText="EMPLOYEE EMAIL (click to hide)"
        CollapsedText="EMPLOYEE EMAIL (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideEmployeeEmail" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblEmployeeEmailHeader" runat="server">Show Employee Email</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgEmployeeEmailHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="EMPLOYEE EMAIL (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEmployeeEmail" runat="server">
    
        <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%">
                    <uc:EmployeeCBL ID="ucEmployeeCBL" runat="server" />
                </td>
            </tr>
        </table>
        
    </asp:Panel>

</asp:PlaceHolder>

<asp:PlaceHolder ID="phClientEmail" runat="server">

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeClientEmail" 
        runat="Server"
        TargetControlID="pnlClientEmail"
        ExpandControlID="pnlShowHideClientEmail"
        CollapseControlID="pnlShowHideClientEmail" 
        Collapsed="True"
        BehaviorID="cpeBehaviorClientEmail"
        TextLabelID="lblClientEmailHeader"
        ImageControlID="imgClientEmailHeader"    
        ExpandedText="CLIENT EMAIL (click to hide)"
        CollapsedText="CLIENT EMAIL (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideClientEmail" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblClientEmailHeader" runat="server">Show Client Email</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgClientEmailHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="CLIENT EMAIL (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlClientEmail" runat="server">
    
        <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%">
                    <uc:ClientContactCBL ID="ucClientContactCBL" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:PlaceHolder>

<asp:PlaceHolder ID="phBottonNavigation" runat="server">

    <table border="0" class="gridEditForm">
        <tr>
            <td style="width:100%; text-align:center">
                <asp:MultiView ID="mvBottomButtons" runat="server">
                    
                    <asp:View ID="viewBottomGrid" runat="server">
                    
                        <asp:button id="btnAdd" text="ADD" runat="server" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" CommandName="PerformInsert" />
                        &nbsp;
                        <asp:Button ID="btnClose" Text="CLOSE" runat="server" CssClass="actionbutton2" 
                            CommandName="Cancel"/>
                        &nbsp;
                    </asp:View>
                    
                    <asp:View ID="viewBottomPage" runat="server">
                    
                        <asp:Button ID="btnAddPage" runat="server" Text="ADD" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" OnClick="btnAddPage_Click" />
                        &nbsp;
                    </asp:View>
                
                </asp:MultiView>
                
                <asp:Button ID="btnShowAllBottom" runat="server" Text="SHOW ALL" CausesValidation="false"
                    CssClass="actionbutton2" OnClientClick="ShowAll(); return false;"/>
                &nbsp;    
                <asp:Button ID="btnHideAllBottom" runat="server" Text="HIDE ALL" CausesValidation="false"
                    CssClass="actionbutton2" OnClientClick="HideAll(); return false;"/>
               
            </td>
        </tr>
    </table>                

</asp:PlaceHolder>

</div>

