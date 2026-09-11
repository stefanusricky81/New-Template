<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimesheetTicketSummary.ascx.cs" Inherits="UserControl_Grid_TimesheetTicketSummary" %>
<asp:Panel ID="pnlHeader" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="GridHeaderTable">
        <tr>
            <th style="width:33%"><asp:Literal ID="litHeader" runat="server"></asp:Literal></th>
            <td style="width:33%; text-align:center;">&nbsp;</td>
            <td style="width:33%">Records Per Page:&nbsp;<uc:RecordPerPageDDL ID="ucRecordsPerPage" runat="server" /></td>
        </tr>
    </table>
</asp:Panel>

<telerik:RadGrid ID="rgTimesheetTicketSummary" runat="server"
    OnNeedDataSource="rgTimesheetTicketSummary_NeedDataSource" 
    OnItemDataBound="rgTimesheetTicketSummary_ItemDataBound"
    OnDetailTableDataBind="rgTimesheetTicketSummary_DetailTableDataBind"
    Skin="BitByBit" EnableEmbeddedSkins="false" ImagesPath="/RadControls/Skin/Grid/BitByBit/"
    AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" AllowMultiRowSelection="false"
    PagerStyle-Mode="NumericPages" PagerStyle-AlwaysVisible="true" ShowFooter="true" PagerStyle-Position="Bottom"
    MasterTableView-NoMasterRecordsText="No Records found.">
    
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="false">
        <Selecting AllowRowSelect="false" />
        <Resizing AllowColumnResize="false" EnableRealTimeResize="false" />
    </ClientSettings>
        
    <MasterTableView ShowHeadersWhenNoRecords="true" TableLayout="fixed" Width="100%" DataKeyNames="TicketId">
        <DetailTables>
            <telerik:GridTableView DataKeyNames="EmployeeId,TicketId" Name="Ticket" Width="100%" TableLayout="Fixed">
                <DetailTables>
                    <telerik:GridTableView DataKeyNames="Ptimesheet" Name="Employee" Width="100%" TableLayout="Fixed">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="Date" DataField="Date" SortExpression="Date" HeaderText="Date" DataType="System.DateTime"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top" DataFormatString="{0:MM/dd/yyyy}">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Hrs" DataField="Hrs" SortExpression="Hrs" HeaderText="Hours" DataType="System.Decimal"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-VerticalAlign="Top">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="Memo" DataField="Memo" SortExpression="Memo" HeaderText="Client Details" DataType="System.String"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60%" ItemStyle-VerticalAlign="Top">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="InternalComments" DataField="InternalComments" SortExpression="InternalComments" HeaderText="Internal Notes" DataType="System.String"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-VerticalAlign="Top">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="EmployeeFirst" DataField="EmployeeFirst" SortExpression="EmployeeFirst" HeaderText="First Name" DataType="System.String"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" ItemStyle-VerticalAlign="Top">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="EmployeeLast" DataField="EmployeeLast" SortExpression="EmployeeLast" HeaderText="Last Name" DataType="System.String"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40%" ItemStyle-VerticalAlign="Top">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="TotalHours" DataField="TotalHours" SortExpression="TotalHours" HeaderText="Total Hours" DataType="System.Decimal"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%" ItemStyle-VerticalAlign="Top">
                    </telerik:GridBoundColumn>
                </Columns>
            </telerik:GridTableView>
        </DetailTables>
        <Columns>
            <telerik:GridBoundColumn UniqueName="TicketId" DataField="TicketId" SortExpression="TicketId" HeaderText="Ticket #" DataType="System.Int32"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="TicketSummary" DataField="TicketSummary" SortExpression="TicketSummary" HeaderText="Summary" DataType="System.String"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn UniqueName="TotalHours" DataField="TotalHours" SortExpression="TotalHours" HeaderText="Total Hours" DataType="System.Decimal"
                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-VerticalAlign="Top">
            </telerik:GridBoundColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>