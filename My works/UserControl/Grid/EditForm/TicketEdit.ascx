<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TicketEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_TicketEdit" %>
<%@ Register TagPrefix="ddl" TagName="TicketCheckListMaster" Src="~/UserControl/DropDownList/TicketCheckListMaster.ascx" %>
<%@ Register TagPrefix="uc" TagName="TicketHistoryGrid" Src="~/UserControl/Grid/TicketHistory.ascx" %>
<%@ Register TagPrefix="uc" TagName="TicketFileGrid" Src="~/UserControl/Grid/TicketFile.ascx" %>
<%@ Register TagPrefix="uc" TagName="TicketTagsGrid" Src="~/UserControl/Grid/TicketTags.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmployeeDDL" Src="~/UserControl/DropDownList/Employee.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketType" Src="~/UserControl/DropDownList/TicketType.ascx" %>
<%@ Register TagPrefix="ddl" TagName="TicketCategory" Src="~/UserControl/DropDownList/TicketCategory.ascx" %>
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        
        function ShowAll() {
            $find('cpeBehaviorGeneral').expandPanel();
            $find('cpeBehaviorCheckList').expandPanel();
            $find('cpeBehaviorClient').expandPanel();
            $find('cpeBehaviorDetails').expandPanel();
            //$find('cpeClientHistory').expandPanel();
            $find('cpeBehaviorEmployeeEmail').expandPanel();
            $find('cpeBehaviorClientEmail').expandPanel();
            $find('cpeBehaviorTags').expandPanel();
            $find('cpeBehaviorHistory').expandPanel();
            $find('cpeBehaviorFiles').expandPanel();
        }

        function HideAll() {
            $find('cpeBehaviorGeneral').collapsePanel();
            $find('cpeBehaviorCheckList').collapsePanel();
            $find('cpeBehaviorClient').collapsePanel();
            $find('cpeBehaviorDetails').collapsePanel();
            //$find('cpeClientHistory').collapsePanel();
            $find('cpeBehaviorEmployeeEmail').collapsePanel();
            $find('cpeBehaviorClientEmail').collapsePanel();
            $find('cpeBehaviorTags').collapsePanel();
            $find('cpeBehaviorHistory').collapsePanel();
            $find('cpeBehaviorFiles').collapsePanel();
        }

        $(document).ready(function () {
            SetupControls();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            SetupControls();
        });
        function SetupControls() {
            var clipboard = new ClipboardJS('.copy-btn');
            clipboard.on('success', function (e) {
                console.log(e);
            });
            clipboard.on('error', function (e) {
                console.log(e);
            });
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
                    
                        <%-- 4-7-2023: Updates not allowed from the old ticket page --%>
                        <asp:button id="btnUpdateTop" Visible="false" text="UPDATE-1" runat="server" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" CommandName="Update" />
                        &nbsp;
                        <asp:Button ID="btnCloseTop" Text="CLOSE" runat="server" CssClass="actionbutton2" 
                            CommandName="Cancel" />
                        &nbsp;
                            
                    </asp:View>
                    
                    <asp:View ID="viewTopPage" runat="server">
                        <%-- 4-7-2023: Updates not allowed from the old ticket page --%>
                        <asp:Button ID="btnUpdatePageTop" Visible="false" runat="server" Text="UPDATE-2" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" OnClick="btnUpdatePage_Click" />
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
        <asp:Literal ID="litDebug" runat="server" />
         <table border="0" class="gridEditForm" width="100%" style="width:100%">
            <asp:PlaceHolder ID="phClientOnHoldWarning" runat="server" Visible="false">
                <tr>
                    <th style="width:10%;" class="Required">&nbsp;</th>
                    <td colspan="3" style="font-size:16px; font-weight:bold; color:Red;">Client Status = On Hold.  Please Refer Client To Accounting.</td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <th style="width:10%;">Ticket #:</th>
                <td style="width:30%">
                    <asp:Literal ID="litTicketNumber" runat="server" />
                    <asp:HyperLink ID="hlTicketNumber" runat="server" />&nbsp;
                    <asp:HyperLink ID="hlDetail2" runat="server" Text="New Detail Screen" />
                </td>
                <th style="width:8%">Reported:</th>
                <td style="width:52%">
                    <asp:Literal ID="litReportedByName" runat="server" />&nbsp;
                    <asp:Literal ID="litReportedDate" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width:10%;" class="Required">Client:</th>
                <td style="width:30%">
                    <uc:ClientComboBox ID="ucClient" runat="server" Width="250"
                        IsRequired="true" ValidationGroup="vgTicket" 
                        ValidationErrorMessage="Client Name is required"/>
                </td>
                <th style="width:8%;">Contact:</th>
                <td style="width:52%;">
                    <asp:Literal ID="litContactFirst" runat="server" />
                    <asp:Literal ID="litContactLast" runat="server" />
                    <asp:Literal ID="litContactPhone" runat="server" />
                    <asp:Literal ID="litContactCellPhone" runat="server" />
                    <asp:Literal ID="litContactHomePhone" runat="server" />
                    <asp:Literal ID="litContactEmail" runat="server" />
                    &nbsp;
                    <asp:CheckBox ID="chkNotifyClient" runat="server" />
                    Notify Client
                </td>
            </tr>
            <tr>
                <th style="width:10%;" class="Required">Summary:</th>
                <td colspan="3">
                    <!--bug with IE expanding table cell if textbox has fixed width-->
                    <asp:TextBox ID="tbSummary" runat="server" CssClass="textbox" 
                        MaxLength="200" Width="60%" />
                    <asp:RequiredFieldValidator ID="rfvSummary" runat="server" ControlToValidate="tbSummary" 
                        ErrorMessage="Summary is required" ValidationGroup="vgTicket">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <asp:HyperLink ID="hlToolTipDetails" runat="server" Text="Description" CssClass="toolTip2"/>
                    <telerik:RadToolTip ID="rtpDetails" runat="server" TargetControlID="hlToolTipDetails" Title="Ticket Description"
                        Skin="BitByBit" EnableEmbeddedSkins="false" Animation="Slide" ManualClose="true" Width="400px" EnableEmbeddedBaseStylesheet="false"/>
                    <asp:HyperLink ID="hlNotifications" runat="server" Text="Email Notifications " CssClass="toolTip2" style="padding-left:20px;"/>
                    <telerik:RadToolTip ID="rtpNotifications" runat="server" TargetControlID="hlNotifications" Title="Email Notifications will be sent to"
                        Skin="BitByBit" EnableEmbeddedSkins="false" Animation="Slide" ManualClose="true" Width="400px" EnableEmbeddedBaseStylesheet="false"/>
                </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Priority:</th>
                <td colspan="3">
                    <uc:TicketPriorityDDL ID="ucTicketPriority" runat="server" Width="290" DefaultText="" DefaultValue="-1" CssClass="dropdown"/>
                    &nbsp;&nbsp;&nbsp;<a href="https://bitxbit.itglue.com/DOC-1106671-1572820" target="_blank"><img src="/Images/question.png" alt="Help" style="vertical-align:middle;" /></a>
                </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;" class="Required">Assigned To:</th>
                <td colspan="3">
                    <uc:EmployeeDDL ID="ucEmployeeAssignedTo" runat="server" DefaultValue="" 
                        RequiredErrorMessage="Assigned To is required" CssClass="dropdown"
                        ValidationGroup="vgTicket" Width="290" EmployeeTypeToDisplay="AssignTo" />
                    &nbsp;<asp:Literal ID="litCopySummary" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;" class="Required" colspan="1">Status:</th>
                <td style="width:30%">
                    <uc:TicketDispositionDDL ID="ucTicketDisposition" runat="server" DefaultValue=""
                        DefaultText="" CssClass="dropdown" Width="290" ValidationGroup="vgTicket"
                        RequiredErrorMessage="Status is required" />
                </td>
                <td colspan="2">&nbsp;<asp:Literal ID="litTest" runat="server" /></td>
            </tr>
            <tr>
                <th style="width:145px;">Type:</th>
                <td style="width:120px;" colspan="3">
                    <ddl:TicketType ID="ddlTicketType" runat="server" DefaultValue="" DefaultText="" CssClass="dropdown" Width="290" ValidationGroup="vgTicket" IsRequired="false" DisplayChosenScript="false"/>
                </td>
            </tr>
            <tr>
                <th style="width:145px;"><asp:Literal ID="litCategoryHeader" runat="server" Text="Category" />:</th>
                <td style="width:120px;" colspan="3">
                    <ddl:TicketCategory ID="ddlTicketCategory" runat="server" DefaultValue="" DefaultText="" CssClass="dropdown" Width="290" ValidationGroup="vgTicket" IsRequired="false" DisplayChosenScript="false"/>
                </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Product:</th>
                <td style="width:30%">
                    <uc:ProductDDL ID="ucTicketProduct" runat="server" DefaultText="" DefaultValue="" IsRequired="false" ValidationGroup="vgTicket" CssClass="dropdown" Width="290"/>
                </td>
              <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Check List:</th>
                <td colspan="3">
                    <ddl:TicketCheckListMaster ID="ddlTicketCheckListMaster" runat="server" DefaultText="" DefaultValue="" IsRequired="false" CssClass="dropdown" Width="290"/>
                    <asp:LinkButton ID="btnCopyCheckList" runat="server" Text="Copy To Notes" CausesValidation="false" OnClick="btnCopyCheckList_Click" style="padding-left:15px;"/>
              </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Estimate:</th>
                <td colspan="3">
                    <asp:TextBox ID="txtHoursToFix" runat="server" MaxLength="10" Width="95"></asp:TextBox>
                    &nbsp;
                    Time Spent:&nbsp;
                    <asp:HyperLink ID="hlTimeSpent" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width:10%; vertical-align:top;">Notes:</th>
                <td colspan="3">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                        <tr>
                            <td style="width:60%;  vertical-align:top;">
                                <!--bug with IE expanding table cell if textbox has fixed width-->
                                <asp:TextBox ID="tbNotes" runat="server" TextMode="MultiLine" Rows="12" Width="95%"/>
                            </td>
                            <td style="width:40%; vertical-align:top;">
                                <!--begin timesheet entry box--> 
                                <table style="border:solid 1px gray; width:450px;">
                                    <tr>
                                        <th style="width:130px;">Add Timesheet</th>
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
                <th style="width:10%; vertical-align:top;">Internal Notes:</th>
                <td colspan="3">
                    <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
                        <tr>
                            <td style="width:60%;  vertical-align:top;">
                                <asp:TextBox ID="tbInternalNotes" runat="server" TextMode="MultiLine" Rows="12" Width="95%"/>
                            </td>
                            <td style="width:40%; vertical-align:top;">
                                <!--begin scheduled / received box--> 
                                <table style="border:solid 1px gray; width:450px;">
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
                                                DateRequiredErrorMessage="Received Date is required" DisableTimeRequired="true"/>
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

<asp:PlaceHolder ID="phCheckList" runat="server">
    
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeCheckList" 
        runat="Server"
        TargetControlID="pnlCheckList"
        ExpandControlID="pnlShowHideCheckList"
        CollapseControlID="pnlShowHideCheckList" 
        Collapsed="True" 
        BehaviorID="cpeBehaviorCheckList"
        TextLabelID="lblCheckListHeader"
        ImageControlID="imgCheckListHeader"    
        ExpandedText="CHECK LIST (click to hide)"
        CollapsedText="CHECK LIST (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideCheckList" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblCheckListHeader" runat="server">Show Check List</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgCheckListHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" AlternateText="Check List (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlCheckList" runat="server">
        <div style="margin-left:10px;">
            <table border="0" class="gridEditForm">
                <asp:Repeater ID="rptCheckList" runat="server" OnItemDataBound="rptCheckList_ItemDataBound">
                    <ItemTemplate>
                        <asp:PlaceHolder ID="phHeader" runat="server" Visible="false">
                            <tr>
                                <td colspan="2"><strong><asp:Literal ID="litCheckListHeader" runat="server" /></strong></td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td style="width:30px;"><asp:CheckBox ID="chkCheckListCompleted" runat="server" /></td>
                            <td><asp:Literal ID="litCheckListName" runat="server" /><asp:Literal ID="litCheckListId" runat="server" Visible="false" /></td>
                        </tr>
                        
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </asp:Panel>

</asp:PlaceHolder>

<asp:PlaceHolder ID="phClient" runat="server">
    
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeClient" 
        runat="Server"
        TargetControlID="pnlClient"
        ExpandControlID="pnlShowHideClient"
        CollapseControlID="pnlShowHideClient" 
        Collapsed="True" 
        BehaviorID="cpeBehaviorClient"
        TextLabelID="lblClientHeader"
        ImageControlID="imgClientHeader"    
        ExpandedText="CLIENT (click to hide)"
        CollapsedText="CLIENT (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideClient" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblClientHeader" runat="server">Show Client</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgClientHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="CLIENT (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlClient" runat="server">
    
        <table border="0" class="gridEditForm">
            <tr>
                <th style="width:120px;" class="Required">Client Contact:</th>
                <td style="width:225px;">
                    <uc:ClientContactDDL ID="ucClientContact" runat="server" 
                        CssClass="dropdown" Width="210"/>
                </td>
                <th style="width:50px;">                    
                    Action 
                </th>
                <td>
                    <asp:DropDownList ID="ddlClientContactAction" runat="server">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Update" Value="U"></asp:ListItem>
                        <asp:ListItem Text="Add" Value="A"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th style="width:120px;" class="Required">First:</th>
                <td style="width:225px;">
                    <asp:TextBox ID="tbClientContactFirstName" runat="server" MaxLength="50" CssClass="textbox"/>
                    <asp:RequiredFieldValidator ID="rfvClientContactFirstName" runat="server" 
                        ControlToValidate="tbClientContactFirstName" ValidationGroup="vgTicket"
                        ErrorMessage="Client Contact First Name is required">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                </td>
                <th style="width:50px;" class="Required">Last:</th>
                <td>
                    <asp:TextBox ID="tbClientContactLastName" runat="server" MaxLength="50" CssClass="textbox"/>
                    <asp:RequiredFieldValidator ID="rfvClientContactLastName" runat="server" 
                        ControlToValidate="tbClientContactLastName" ValidationGroup="vgTicket"
                        ErrorMessage="Client Contact Last Name is required">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th style="width:120px;" class="Required">Email:</th>
                <td style="width:225px;">
                    <asp:TextBox ID="tbClientContactEmail" runat="server" MaxLength="75" CssClass="textbox"/>
                    <asp:RequiredFieldValidator ID="rfvClientContactEmail" runat="server" 
                        ControlToValidate="tbClientContactEmail" ValidationGroup="vgTicket"
                        ErrorMessage="Client Contact Email is required">
                        <span class="error">*</span>
                    </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvClientContactEmail" runat="server"
                        ControlToValidate="tbClientContactEmail" ValidationGroup="vgTicket"
                        ErrorMessage="Client Contact Email is already in use" 
                        OnServerValidate="cvClientContactEmail_ServerValidate" Display="None">
                        <span class="error">*</span>
                    </asp:CustomValidator>
                </td>
                <th style="width:50px;">Phone:</th>
                <td>
                    <asp:TextBox ID="tbClientContactPhone" runat="server" MaxLength="25" CssClass="textbox"/> 
                    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="tbClientContactPhone" ValidationGroup="vgTicket"><span class="error">*</span></asp:RegularExpressionValidator>
                </td>
            </tr>
             <tr>
                <th style="width:120px;">Cell:</th>
                <td style="width:225px;">
                    <asp:TextBox ID="txtCellPhone" runat="server" MaxLength="17" CssClass="textbox"/>
                    <asp:RegularExpressionValidator ID="revCellPhone" runat="server" ControlToValidate="txtCellPhone" ValidationGroup="vgTicket"><span class="error">*</span></asp:RegularExpressionValidator>
                </td>
                <th style="width:50px;">Home:</th>
                <td>
                    <asp:TextBox ID="txtHomePhone" runat="server" MaxLength="17" CssClass="textbox"/>
                    <asp:RegularExpressionValidator ID="revHomePhone" runat="server" ControlToValidate="txtHomePhone" ValidationGroup="vgTicket"><span class="error">*</span></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th style="width:120px;">Email On Close:</th>
                <td colspan="3">
                    <asp:CheckBox ID="chkEnteredByRecieveTicketOnClose" runat="server" Checked="true"/>
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
            <tr>
                <th style="width:120px; vertical-align:top;">Description:</th>
                <td colspan="5">
                    <asp:Label ID="lblDescription" runat="server" />
                </td>    
            </tr>
            <asp:PlaceHolder ID="phMerge" runat="server">
                <tr>
                    <th style="width:120px; vertical-align:top;">Merge Master Tkt #:</th>
                    <td colspan="5">
                        <asp:TextBox ID="tbMasterTicketId" runat="server" Width="100" />
                        <asp:Literal ID="litMasterticketId" runat="server" />
                        <asp:RegularExpressionValidator runat="server" ID="revMasterTicketId" 
                            ControlToValidate="tbMasterTicketId" ValidationExpression="\d+"
                            ErrorMessage="Invalid Merge Master Ticket" ValidationGroup="vgTicket">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cvMasterTicketId" runat="server"
                            ControlToValidate="tbMasterTicketId" ValidationGroup="vgTicket"
                            OnServerValidate="cvMasterTicketId_ServerValidate">
                            <span class="error">*</span>
                        </asp:CustomValidator>
                        <em>(Current Ticket will be merged into Master Ticket. Current Ticket will be closed.)</em>
                    </td>
                </tr>
            </asp:PlaceHolder>
    </table>
    
    </asp:Panel>
    
</asp:PlaceHolder>

<asp:PlaceHolder ID="phClientHistory" runat="server">
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeClientHistory" 
        runat="Server"
        TargetControlID="pnlClientHistory"
        ExpandControlID="pnlShowHideClientHistory"
        CollapseControlID="pnlShowHideClientHistory" 
        Collapsed="True"
        BehaviorID="cpeBehaviorClientHistory"
        TextLabelID="lblClientHistoryHeader"
        ImageControlID="imgClientHistoryHeader"    
        ExpandedText="CLIENT PREVIOUS TICKETS (click to hide)"
        CollapsedText="CLIENT PREVIOUS TICKETS (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
    <asp:Panel ID="pnlShowHideClientHistory" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblClientHistoryHeader" runat="server">Show Client Previous Tickets</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgClientHistoryHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="CLIENT PREVIOUS TICKETS (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlClientHistory" runat="server">

        <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%">
                    <asp:MultiView ID="mvClientHistory" runat="server" ActiveViewIndex="0">
                        <asp:View ID="viewClientHistoryButton" runat="server">
                            <br />
                            <asp:Button ID="btnViewClientHistoryGrid" Text="VIEW" runat="server" CssClass="actionbutton" OnClick="btnViewClientHistoryGrid_Click" />
                            <br /><br />
                        </asp:View>
                        <asp:View ID="viewClientHistoryGrid" runat="server">
                            <uc:TicketGrid ID="ucTicketClientHistoryGrid" runat="server" Type="ClientHistory" />
                        </asp:View>
                    </asp:MultiView>
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

<asp:PlaceHolder ID="phTags" runat="server">
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeTags"
        runat="Server"
        TargetControlID="pnlTags"
        ExpandControlID="pnlShowHideTags"
        CollapseControlID="pnlShowHideTags" 
        Collapsed="True"
        BehaviorID="cpeBehaviorTags"
        TextLabelID="lblTagsHeader"
        ImageControlID="imgTagsHeader"    
        ExpandedText="TAGS (click to hide)"
        CollapsedText="TAGS (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideTags" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblTagsHeader" runat="server">Show Tags</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgTagsHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="TAGS (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlTags" runat="server">

         <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%">
                    <uc:TicketTagsGrid ID="ucTicketTagsGrid" runat="server" />
                </td>
            </tr>
        </table>
    
    </asp:Panel>

</asp:PlaceHolder>


<asp:PlaceHolder ID="phHistory" runat="server">

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeHistory" 
        runat="Server"
        TargetControlID="pnlHistory"
        ExpandControlID="pnlShowHideHistory"
        CollapseControlID="pnlShowHideHistory" 
        Collapsed="True"
        BehaviorID="cpeBehaviorHistory"
        TextLabelID="lblHistoryHeader"
        ImageControlID="imgHistoryHeader"    
        ExpandedText="HISTORY (click to hide)"
        CollapsedText="HISTORY (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideHistory" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblHistoryHeader" runat="server">Show History</asp:Label>&nbsp;
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgHistoryHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" AlternateText="HISTORY (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlHistory" runat="server">
         <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%; padding-left:5px;">
                    <asp:CheckBox ID="chkHistoryExcludeAlerts" runat="server" AutoPostBack="true" OnCheckedChanged="chkHistoryExcludeAlerts_CheckedChanged" />&nbsp;Exclude Alerts&nbsp;
                    <asp:CheckBox ID="chkHistoryExcludeViews" runat="server" AutoPostBack="true" OnCheckedChanged="chkHistoryExcludeViews_CheckedChanged" />&nbsp;Exclude Non-Action Updates
                </td>
            </tr>
            <tr>
                <td style="width:100%">
                    <uc:TicketHistoryGrid ID="ucTicketHistoryGrid" runat="server" />
                </td>
            </tr>
        </table>
    
    </asp:Panel>

</asp:PlaceHolder>

<asp:PlaceHolder ID="phFiles" runat="server">

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFiles" 
        runat="Server"
        TargetControlID="pnlFiles"
        ExpandControlID="pnlShowHideFiles"
        CollapseControlID="pnlShowHideFiles" 
        Collapsed="True"
        BehaviorID="cpeBehaviorFiles"
        TextLabelID="lblFilesHeader"
        ImageControlID="imgFilesHeader"    
        ExpandedText="FILES (click to hide)"
        CollapsedText="FILES (click to show)"
        ExpandedImage="~/images/collapse_blue.jpg"
        CollapsedImage="~/Images/expand_blue.jpg"
        SuppressPostBack="true"
    />
                     
    <asp:Panel ID="pnlShowHideFiles" runat="server" CssClass="collapsePanelHeader" Height="25px">
        <div style="cursor: pointer; vertical-align: middle; padding:5px;">
            <div style="float: left;">
                <asp:Label ID="lblFilesHeader" runat="server">Show Files</asp:Label>
            </div>
            <div style="float: right; vertical-align: middle;">
                <asp:ImageButton ID="imgFilesHeader" runat="server" ImageUrl="~/images/expand_blue.jpg" 
                    AlternateText="FILES (click to show)"/>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFiles" runat="server">

        <table border="0" class="gridEditForm">
            <tr>
                <td style="width:100%">
                    <uc:TicketFileGrid ID="ucTicketFileGrid" runat="server" />
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
                    
                        <asp:button id="btnUpdate" text="UPDATE-3" runat="server" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" CommandName="Update" />
                        &nbsp;
                        <asp:Button ID="btnClose" Text="CLOSE" runat="server" CssClass="actionbutton2" 
                            CommandName="Cancel"/>
                        &nbsp;
                    </asp:View>
                    
                    <asp:View ID="viewBottomPage" runat="server">
                    
                        <asp:Button ID="btnUpdatePage" Visible="false" runat="server" Text="UPDATE-4" CssClass="actionbutton" 
                            ValidationGroup="vgTicket" OnClick="btnUpdatePage_Click" />
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