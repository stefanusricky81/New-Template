<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimesheetEdit.ascx.cs" Inherits="UserControl_Grid_EditForm_TimesheetEdit" %>
<%@ Register TagPrefix="ddl" TagName="SmartClient" Src="~/UserControl/DropDownList/SmartClient.ascx"%>

<div id="divMain" runat="server" style="background-color:#fafbfc; padding:5px 5px 5px 5px; width:99%;">

<table border="0" class="gridEditForm" width="100%" style="width:100%">
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButtonTop" runat="server">
            
                <asp:View ID="viewGridTop" runat="server">
                    <asp:button id="btnUpdateTop" text="UPDATE" runat="server" CssClass="actionbutton" ValidationGroup="vgTimesheet"
                        CommandName="Update">
                    </asp:button>
                    <asp:button id="btnInsertTop" text="ADD" runat="server" CssClass="actionbutton" ValidationGroup="vgTimesheet"
                        CommandName="PerformInsert">
                    </asp:button>
                    &nbsp;
                    <asp:Button ID="btnCloseTop" Text="CLOSE" runat="server" CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>
                
                <asp:View ID="viewPageTop" runat="server">
                </asp:View>
                
            </asp:MultiView>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:ValidationSummary ID="vsTimesheet" runat="server" 
                    CssClass="ValidationSummary" ValidationGroup="vgTimesheet" />
        </td>
    </tr>
    <asp:Panel ID="pnlExistingReadOnly" runat="server">
        <tr>
            <th style="width:110px; vertical-align:top;">Timesheet ID:</th>
            <td><asp:Literal ID="litTimesheetId" runat="server" /></td>
        </tr>
        <tr>
            <th style="width:110px; vertical-align:top;">Created:</th>
            <td><asp:Literal ID="litCreated" runat="server" /></td>
        </tr>
        <tr>
            <th style="width:110px; vertical-align:top;">Last Updated:</th>
            <td>
                <asp:Literal ID="litLastUpdated" runat="server"/>
                (<asp:Literal ID="litLastUpdatedBy" runat="server"/>)
            </td>
        </tr>
        <tr>
            <th style="width:110px; vertical-align:top;">Employee:</th>
            <td><asp:Literal ID="litEmployeeCode" runat="server" /></td>
        </tr>
    </asp:Panel>
    
    <tr>
        <th style="width:110px;" class="Required">Date:</th>
        <td>
            <telerik:RadDatePicker id="rdpDate" Runat="server" SharedCalendarID="sharedCalendar" 
                Width="120px" EnableEmbeddedSkins="false" Skin="BitByBit">
            </telerik:RadDatePicker>
            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Date is required" 
                ControlToValidate="rdpDate" ValidationGroup="vgTimesheet">
                <span class="error">*</span>
            </asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvDate" runat="server" ControlToValidate="rdpDate" ValidationGroup="vgTimesheet" 
                OnServerValidate="cvDate_ServerValidate">
                <span class="error">*</span>
            </asp:CustomValidator>
            <telerik:RadCalendar ID="sharedCalendar" runat="server" EnableMultiSelect="false" EnableEmbeddedSkins="false" Skin="BitByBit" ShowRowHeaders="false">
                <FastNavigationSettings EnableTodayButtonSelection="true" />
            </telerik:RadCalendar>
        </td>
    </tr>
    <tr>
        <th style="width:110px;" class="Required">Start Time:</th>
        <td>
            <uc:TimeDDL ID="ucStartDDL" runat="server" 
                FifteenMinuteIncrements="true" DisplayDefaultValue="false"/>
        </td>
    </tr>
    <tr>
        <th style="width:110px;" class="Required">Time Spent:</th>
        <td>
            <uc:TimeDDL ID="ucTimeSpentDDL" runat="server" StartHourAtZero="true"
                FifteenMinuteIncrements="true" DisplayDefaultValue="false" />
            <asp:CustomValidator ID="cvTimeSpent" runat="server" 
                ControlToValidate="rdpDate" OnServerValidate="cvTimeSpent_ServerValidate"
                ValidationGroup="vgTimesheet" ErrorMessage="Time Spent is required">
                <span class="error">*</span>
            </asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <th style="width:110px;" class="Required">Client:</th>
        <td>
            <ddl:SmartClient ID="ddlSmartClient" runat="server" AutoPostback="true" Width="500" />
        </td>
    </tr>
    <tr>
        <th style="width:110px;" class="Required">Project:</th>
        <td>
            <uc:ProjectTaskDDL id="ucProject" runat="server" Width="500"
                CssClass="dropdown" DefaultValue="" DefaultText="" ValidationGroup="vgTimesheet"/>
        </td>
    </tr>
    <tr>
        <th style="width:110px;">My Tickets:</th>
        <td>
            <uc:EmployeeTicketDDL ID="ucEmployeeTicket" runat="server" Width="500" 
                CssClass="dropdown" DefaultValue="" DefaultText="" IsRequired="false" />
        </td>
    </tr>
    <tr>
        <th style="width:110px;">Open Tickets:</th>
        <td>
            <uc:TicketComboBox ID="ucOpenTicket" runat="server" Width="500" IsRequired="false" />
        </td>
    </tr>
    <tr>
        <th style="width:110px;" class="Required">Activity:</th>
        <td>
            <uc:TaskActionDDL ID="ucActivity" runat="server" Width="200" 
                CssClass="dropdown" DefaultValue="" DefaultText="" ValidationGroup="vgTimesheet" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phAdmin" runat="server" Visible="false">
        <tr>
            <th class="Required" style="width:80px;">Employee:</th>
            <td>
                <uc:EmployeeDDL ID="ucEmployee" runat="server" ValidationGroup="vgTimesheet"
                    UseEmployeeIdAsDataValue="true" DefaultValue="" Width="200" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phOvertime" runat="server" Visible="false">
        <tr>
            <th style="width:110px;">Is Overtime:</th>
            <td><asp:CheckBox ID="chkIsOvertime" runat="server" /></td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th style="width:110px; vertical-align:top;">Client Details:</th>
        <td>
            <asp:TextBox ID="tbClientDetails" runat="server" TextMode="MultiLine" 
                Rows="12" Width="700"/>
             <asp:CustomValidator ID="cvNotes" runat="server" ValidateEmptyText="true"
                ControlToValidate="tbClientDetails" OnServerValidate="cvNotes_ServerValidate"
                ValidationGroup="vgTimesheet" ErrorMessage="Client Details or Internal Notes are required">
                <span class="error">*</span>
            </asp:CustomValidator>
        </td>
    </tr>        
    <tr>
        <th style="width:110px; vertical-align:top;">Internal Notes:</th>
        <td>
            <asp:TextBox ID="tbInternalNotes" runat="server" TextMode="MultiLine" 
                Rows="12" Width="700"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <th style="width:110px; color:#505050" class="Required">EXPENSES</th>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2" style="padding:0; margin:0;">
            <table border="0" class="gridEditForm" style="width:840px;">
                <tr>
                    <th style="width:110px;">Location:</th>
                    <td style="width:100px;">
                        <asp:TextBox ID="tbLocationExpense" runat="server" MaxLength="12" Width="80"/>
                    </td>
                    <th style="width:80px;">Miles:</th>
                    <td style="width:110px;">
                        <asp:TextBox ID="tbMilesExpense" runat="server" MaxLength="10" Width="80"/>
                        <asp:RequiredFieldValidator ID="rfvMilesExpense" runat="server" 
                            ErrorMessage="Miles Expense is required" 
                            ControlToValidate="tbMilesExpense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
		                <asp:RegularExpressionValidator runat="server" ID="revMilesExpense" 
                            ControlToValidate="tbMilesExpense" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"  
                            ErrorMessage="Invalid Miles Expense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                    </td>
                    <th style="width:80px;">Tolls:</th>
                    <td style="width:110px;">
                        <asp:TextBox ID="tbTollsExpense" runat="server" MaxLength="10" Width="80"/>
		                <asp:RequiredFieldValidator ID="rfvTollsExpense" runat="server" 
                            ErrorMessage="Tolls Expense is required" 
                            ControlToValidate="tbTollsExpense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
		                <asp:RegularExpressionValidator runat="server" ID="revTollsExpense" 
                            ControlToValidate="tbTollsExpense" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"  
                            ErrorMessage="Invalid Tolls Expense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                    </td>
                    <th style="width:80px;">Parking:</th>
                    <td style="width:110px;">
                        <asp:TextBox ID="tbParkingExpense" runat="server" MaxLength="10" Width="80"/>
		                <asp:RequiredFieldValidator ID="rfvParkingExpense" runat="server" 
                            ErrorMessage="Parking Expense is required" 
                            ControlToValidate="tbParkingExpense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
		                <asp:RegularExpressionValidator runat="server" ID="revParkingExpense" 
                            ControlToValidate="tbParkingExpense" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"  
                            ErrorMessage="Invalid Parking Expense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width:110px;">Mass Tran:</th>
                    <td style="width:110px;">
                        <asp:TextBox ID="tbMassTranExpense" runat="server" MaxLength="10" Width="80"/>
		                <asp:RequiredFieldValidator ID="rfvMassTranExpense" runat="server" 
                            ErrorMessage="Mass Tran Expense is required" 
                            ControlToValidate="tbMassTranExpense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
		                <asp:RegularExpressionValidator runat="server" ID="revMassTranExpense" 
                            ControlToValidate="tbMassTranExpense" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"  
                            ErrorMessage="Invalid Mass Tran Expense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                    </td>
                    <th style="width:80px;">Misc:</th>
                    <td style="width:110px;">
                        <asp:TextBox ID="tbMiscExpense" runat="server" MaxLength="10" Width="80"/>
		                <asp:RequiredFieldValidator ID="rfvMiscExpense" runat="server" 
                            ErrorMessage="Misc Expense is required" 
                            ControlToValidate="tbMiscExpense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
		                <asp:RegularExpressionValidator runat="server" ID="revMiscExpense" 
                            ControlToValidate="tbMiscExpense" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"  
                            ErrorMessage="Invalid Misc Expense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                    </td>
                     <th style="width:80px;">Comm Amt:</th>
                    <td style="width:110px;">
                        <asp:TextBox ID="tbCommAmtExpense" runat="server" MaxLength="10" Width="80"/>
		                <asp:RequiredFieldValidator ID="rfvCommAmtExpense" runat="server" 
                            ErrorMessage="Comm Amt Expense is required" 
                            ControlToValidate="tbCommAmtExpense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RequiredFieldValidator>
		                <asp:RegularExpressionValidator runat="server" ID="revCommAmtExpense" 
                            ControlToValidate="tbCommAmtExpense" ValidationExpression="(^\d*\.?\d*[0-9]+\d*$)|(^[0-9]+\d*\.\d*$)"  
                            ErrorMessage="Invalid Comm Amt Expense" ValidationGroup="vgTimesheet">
                            <span class="error">*</span>
                        </asp:RegularExpressionValidator>
                    </td>
                    <td colspan="2">&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2" style="text-align:center;">
            <asp:MultiView ID="mvButton" runat="server">
            
                <asp:View ID="viewGrid" runat="server">
                    <asp:button id="btnUpdate" text="UPDATE" runat="server" CssClass="actionbutton" ValidationGroup="vgTimesheet"
                        CommandName="Update">
                    </asp:button>
                    <asp:button id="btnInsert" text="ADD" runat="server" CssClass="actionbutton" ValidationGroup="vgTimesheet"
                        CommandName="PerformInsert">
                    </asp:button>
                    &nbsp;
                    <asp:Button ID="btnClose" Text="CLOSE" runat="server" CssClass="actionbutton2" CommandName="Cancel"/>
                </asp:View>
                
                <asp:View ID="viewPage" runat="server">
                    <asp:Button ID="btnUpdatePage" runat="server" Text="UPDATE" CssClass="actionbutton" 
                        ValidationGroup="vgTimesheet" OnClick="btnUpdatePage_Click" />
                </asp:View>
                
            </asp:MultiView>
        </td>
    </tr>
</table>

</div>
