<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CurrentActivity.ascx.cs" Inherits="UserControl_Grid_CurrentActivity" %>
<telerik:radscriptblock id="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        //disable ajax for export button click
        function conditionalPostback(e, sender) {
            var theRegexp = new RegExp("\.btnExport$", "ig");
            if (sender.EventTarget.match(theRegexp)) {
                sender.EnableAjax = false;
            }
        }
        function rgCurrentActivity_ColumnResized(sender, eventArgs) {
            $.ajax({
                type: "POST",
                url: "/WebService/ActivityTracking.asmx/SetColumnWidth",
                data: "{'columnUniqueName': '" + eventArgs.get_gridColumn().get_uniqueName() + "','width': '" + eventArgs.get_gridColumn().get_element().offsetWidth + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });
        }
    </script>
</telerik:radscriptblock>

<asp:Panel ID="pnlHeader" runat="server" Visible="false">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
            <td style="width:33%; text-align:center;">&nbsp;</td>
            <td style="width:34%">&nbsp;</td>
        </tr>
    </table>
</asp:Panel>

<telerik:RadGrid ID="rgCurrentActivity" runat="server"
    OnNeedDataSource="rgCurrentActivity_NeedDataSource" 
    OnItemDataBound="rgCurrentActivity_ItemDataBound"
    OnItemCreated="rgCurrentActivity_ItemCreated"
    OnPreRender="rgCurrentActivity_PreRender"
    OnItemCommand="rgCurrentActivity_ItemCommand"
    OnSortCommand="rgCurrentActivity_SortCommand"
    OnDetailTableDataBind="rgCurrentActivity_DetailTableDataBind"
    PageSize="500"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="false" PagerStyle-Position="Bottom">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" >
        <Selecting AllowRowSelect="true" />
        <Resizing AllowColumnResize="true" ClipCellContentOnResize="false" />
        <ClientEvents OnColumnResized="rgCurrentActivity_ColumnResized" />
    </ClientSettings>
    
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" CommandItemSettings-ShowAddNewRecordButton="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="Top" EditMode="EditForms" Width="100%" Name="CurrentActivity"
        DataKeyNames="UserId,TicketDateEntered,TicketDescription,TicketEnteredByFirstName,TicketEnteredByLastName,TicketHistoryCreated,TicketHistoryNotes,TicketUpdatedByFirstName,TicketUpdatedByLastName,
        InternalTicketHistoryCreated,InternalTicketHistoryNotes,InternalTicketUpdatedByFirstName,InternalTicketUpdatedByLastName,UserActiveTicketStart,UserStartTime,UserEndTime,UserLastCheckInStartTime">
          
        <CommandItemTemplate>
            <table class="rgCommandRow" style="width:100%">
                <tr>
                    <td style="width:70%" align="left">
                        <uc:GridExportTypeDDL ID="ucGridExportType" runat="server" />&nbsp;
                        <asp:LinkButton ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click"/>
                    </td>
                    <td style="width:15%" align="right">
                        <asp:LinkButton ID="lbRefresh" runat="server" CommandName="RefreshGrid">
                            <img style="border:0px; padding-right:5px;" alt="" src="/RadControls/Skin/Grid/BitByBit/Refresh.gif" />Refresh
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
        </CommandItemTemplate> 
        <DetailTables>
            <telerik:GridTableView Name="ScheduledTicket" Width="100%" TableLayout="Fixed" DataKeyNames="Id,Summary,DateEntered,Description,EnteredByFirstName,EnteredByLastName,LastUpdated,LastUpdatedNotes,UpdatedByFirstName,UpdatedByLastName,ScheduledDate,TimeSpent" CommandItemDisplay="top">
                <CommandItemTemplate>
                    <table class="rgCommandRow" style="width:100%">
                        <tr><th style="width:99%; height:20px;">SCHEDULED TICKETS</th></tr>
                    </table>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="Id" DataField="Id" SortExpression="Id" HeaderText="No." HeaderTooltip="Ticket Number" DataType="System.Integer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="Summary" DataField="Summary" SortExpression="Summary" HeaderText="Summary" HeaderTooltip="Ticket Summary" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="CompanyName" DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company Name" HeaderTooltip="Company Name" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="DispositionName" DataField="DispositionName" SortExpression="DispositionDisplayorder" HeaderText="Status" HeaderTooltip="Ticket Status" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="ScheduledDate" DataField="ScheduledDate" SortExpression="ScheduledDate" HeaderText="Scheduled" HeaderTooltip="Ticket Scheduled Time" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="TimeSpent" DataField="TimeSpent" SortExpression="TimeSpent" HeaderText="Hrs So Far" HeaderTooltip="Total Hours Spent on Ticket" DataType="System.Double" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="PriorityName" DataField="PriorityName" SortExpression="PriorityName" HeaderText="Priority" HeaderTooltip="Ticket Priority" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="DateEntered" DataField="DateEntered" SortExpression="DateEntered" HeaderText="Entered" HeaderTooltip="Ticket Entered" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="LastUpdated" DataField="LastUpdated" SortExpression="LastUpdated" HeaderText="Updated" HeaderTooltip="Ticket Updated" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="HoursToFix" DataField="HoursToFix" SortExpression="HoursToFix" HeaderText="Estimate" HeaderTooltip="Ticket Estimate" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                </Columns>
            </telerik:GridTableView>
            <telerik:GridTableView Name="ActiveTicket" Width="100%" TableLayout="Fixed" DataKeyNames="TicketId,TicketSummary,TicketDateEntered,TicketDescription,TicketEnteredByFirstName,TicketEnteredByLastName,ActiveTicketCreated,ActiveTicketStartTime,ActiveTicketEndTime,TicketScheduledDate,TimeSpent" CommandItemDisplay="top">
                <CommandItemTemplate>
                    <table class="rgCommandRow" style="width:100%">
                        <tr><th style="width:99%; height:20px;">ACTIVE TICKETS</th></tr>
                    </table>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="Id" DataField="TicketId" SortExpression="TicketId" HeaderText="No." HeaderTooltip="Ticket Number" DataType="System.Integer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="Summary" DataField="TicketSummary" SortExpression="TicketSummary" HeaderText="Summary" HeaderTooltip="Ticket Summary" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="CompanyName" DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company Name" HeaderTooltip="Company Name" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="DispositionName" DataField="DispositionName" SortExpression="DispositionDisplayorder" HeaderText="Status" HeaderTooltip="Ticket Status" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="ScheduledDate" DataField="TicketScheduledDate" SortExpression="TicketScheduledDatee" HeaderText="Scheduled" HeaderTooltip="Ticket Scheduled Time" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="TimeSpent" DataField="TimeSpent" SortExpression="TimeSpent" HeaderText="Hrs So Far" HeaderTooltip="Total Hours Spent on Ticket" DataType="System.Double" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="PriorityName" DataField="PriorityName" SortExpression="PriorityName" HeaderText="Priority" HeaderTooltip="Ticket Priority" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="DateEntered" DataField="TicketDateEntered" SortExpression="TicketDateEntered" HeaderText="Entered" HeaderTooltip="Ticket Entered" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="StartTime" DataField="StartTime" SortExpression="StartTime" HeaderText="Time" HeaderTooltip="Ticket Active Time" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="HoursToFix" DataField="TicketHoursToFix" SortExpression="TicketHoursToFix" HeaderText="Estimate" HeaderTooltip="Ticket Estimate" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                </Columns>
            </telerik:GridTableView>
            <telerik:GridTableView Name="UpdatedTicket" Width="100%" TableLayout="Fixed" DataKeyNames="TicketId,TicketSummary,TicketDateEntered,TicketDescription,TicketEnteredByFirstName,TicketEnteredByLastName,HistoryCreated,HistoryNotes,TicketUpdatedByFirstName,TicketUpdatedByLastName,TicketScheduledDate,TimeSpent" CommandItemDisplay="top">
                <CommandItemTemplate>
                    <table class="rgCommandRow" style="width:100%">
                        <tr><th style="width:99%; height:20px;">UPDATED TICKETS</th></tr>
                    </table>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="Id" DataField="TicketId" SortExpression="TicketId" HeaderText="No." HeaderTooltip="Ticket Number" DataType="System.Integer" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="Summary" DataField="TicketSummary" SortExpression="TicketSummary" HeaderText="Summary" HeaderTooltip="Ticket Summary" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="CompanyName" DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company Name" HeaderTooltip="Company Name" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="DispositionName" DataField="DispositionName" SortExpression="DispositionDisplayorder" HeaderText="Status" HeaderTooltip="Ticket Status" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="ScheduledDate" DataField="TicketScheduledDate" SortExpression="TicketScheduledDatee" HeaderText="Scheduled" HeaderTooltip="Ticket Scheduled Time" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="TimeSpent" DataField="TimeSpent" SortExpression="TimeSpent" HeaderText="Hrs So Far" HeaderTooltip="Total Hours Spent on Ticket" DataType="System.Double" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="PriorityName" DataField="PriorityName" SortExpression="PriorityName" HeaderText="Priority" HeaderTooltip="Ticket Priority" DataType="System.String" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="DateEntered" DataField="TicketDateEntered" SortExpression="TicketDateEntered" HeaderText="Entered" HeaderTooltip="Ticket Entered" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="LastUpdated" DataField="HistoryCreated" SortExpression="HistoryCreated" HeaderText="Updated" HeaderTooltip="Ticket Updated" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                    <telerik:GridBoundColumn UniqueName="HoursToFix" DataField="TicketHoursToFix" SortExpression="TicketHoursToFix" HeaderText="Estimate" HeaderTooltip="Ticket Estimate" DataType="System.DateTime" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" />
                </Columns>
            </telerik:GridTableView>
        </DetailTables>       
        <Columns>
            <telerik:GridBoundColumn UniqueName="UserFirst" DataField="UserFirst" SortExpression="UserFirst" HeaderText="Employee" HeaderTooltip="Employee First Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="UserStartTime" DataField="UserStartTime" SortExpression="UserStartTime" HeaderText="E. Start" HeaderTooltip="Employee Work Day Start Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime"/>
            <telerik:GridBoundColumn UniqueName="UserLastCheckInStartTime" DataField="UserLastCheckInStartTime" SortExpression="UserLastCheckInStartTime" HeaderText="E. Check In" HeaderTooltip="Employee Work Day Last Check In Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime"/>
            <telerik:GridBoundColumn UniqueName="UserEndTime" DataField="UserEndTime" SortExpression="UserEndTime" HeaderText="E. End" HeaderTooltip="Employee Work Day End Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime"/>
            <telerik:GridBoundColumn UniqueName="UserActivityNotes" DataField="UserActivityNotes" SortExpression="UserActivityNotes" HeaderText="E. Notes" HeaderTooltip="Employee Notes" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="UserRoleOneName" DataField="UserRoleOneName" SortExpression="UserRoleOneName" HeaderText="E. Roles" HeaderTooltip="Employee Roles" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="UserLocationName" DataField="UserLocationName" SortExpression="UserLocationName" HeaderText="E. Location" HeaderTooltip="Employee Location" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="UserStatusName" DataField="UserStatusName" SortExpression="UserStatusName" HeaderText="E. Status" HeaderTooltip="Employee Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="TotalScheduledTickets" DataField="TotalScheduledTickets" SortExpression="TotalScheduledTickets" HeaderText="S.T." HeaderTooltip="Scheduled Tickets For Day" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.Int"/>
            <telerik:GridBoundColumn UniqueName="TotalActiveTickets" DataField="TotalActiveTickets" SortExpression="TotalActiveTickets" HeaderText="A.T." HeaderTooltip="Active Tickets For Day" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.Int"/>
            <telerik:GridBoundColumn UniqueName="TotalUpdatedTickets" DataField="TotalUpdatedTickets" SortExpression="TotalUpdatedTickets" HeaderText="U.T." HeaderTooltip="Updated Tickets For Day" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.Int"/>
            <telerik:GridBoundColumn UniqueName="TicketSummary" DataField="TicketSummary" SortExpression="TicketSummary" HeaderText="T. Summary" HeaderTooltip="Ticket Summary" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="UserActiveTicketStart" DataField="UserActiveTicketStart" SortExpression="UserActiveTicketStart" HeaderText="Start" HeaderTooltip="Start Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="TicketEstimate" DataField="TicketEstimate" SortExpression="TicketEstimate" HeaderText="T. Estimate" HeaderTooltip="Ticket Estimate" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="ClientCompany" DataField="ClientCompany" SortExpression="ClientCompany" HeaderText="T. Client" HeaderTooltip="Ticket Client Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.String"/>
            <telerik:GridBoundColumn UniqueName="TicketDateEntered" DataField="TicketDateEntered" SortExpression="TicketDateEntered" HeaderText="T. Entered" HeaderTooltip="Ticket Entered" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="TicketHistoryCreated" DataField="TicketHistoryCreated" SortExpression="TicketHistoryCreated" HeaderText="T. Updated Ext. " HeaderTooltip="Last External Ticket Update Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
            <telerik:GridBoundColumn UniqueName="InternalTicketHistoryCreated" DataField="InternalTicketHistoryCreated" SortExpression="InternalTicketHistoryCreated" HeaderText="T. Updated Int. " HeaderTooltip="Last Internal Ticket Update" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" DataType="System.DateTime" />
        </Columns>
        
    </MasterTableView>
    
</telerik:RadGrid>